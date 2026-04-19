namespace Docker.DotNet.SourceAnalyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class JsonSerializerAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "DDN001";
    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        "Missing JsonSerializable attribute",
        "Type '{0}' used in {1} is not registered in DockerExtendedJsonSerializerContext",
        "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // Handle method calls (MakeRequestAsync, etc.)
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);

        // Handle 'new JsonRequestContent<T>'
        context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;

        if (context.SemanticModel.GetSymbolInfo(invocation).Symbol is not IMethodSymbol methodSymbol)
            return;

        bool isStjEntryPoint =
            (methodSymbol.Name == "MakeRequestAsync" && methodSymbol.ContainingType?.Name == "DockerClient") ||
            (methodSymbol.Name == "MonitorStreamForMessagesAsync" && methodSymbol.ContainingType?.Name == "StreamUtil") ||
            (methodSymbol.ContainingType?.Name == "JsonSerializer" && methodSymbol.ContainingNamespace.ToDisplayString() == "Docker.DotNet");

        if (isStjEntryPoint && methodSymbol.TypeArguments.Length == 1)
        {
            var methodName = $"{methodSymbol.ContainingType.Name}.{methodSymbol.Name}";
            CheckAndReportType(context, methodSymbol.TypeArguments[0], invocation.GetLocation(), methodName);
        }
    }

    private static void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
    {
        var creation = (ObjectCreationExpressionSyntax)context.Node;

        if (context.SemanticModel.GetSymbolInfo(creation).Symbol is not IMethodSymbol constructorSymbol)
            return;

        var typeSymbol = constructorSymbol.ContainingType;

        // Check if we are instantiating JsonRequestContent<T>
        if (typeSymbol.Name == "JsonRequestContent" &&
            typeSymbol.ContainingNamespace.ToDisplayString() == "Docker.DotNet")
        {
            if (typeSymbol.TypeArguments.Length == 1)
            {
                var typeName = typeSymbol.Name;
                CheckAndReportType(context, typeSymbol.TypeArguments[0], creation.GetLocation(), typeName);
            }
        }
    }

    private static void CheckAndReportType(SyntaxNodeAnalysisContext context, ITypeSymbol targetType, Location location, string sourceName)
    {
        // Skip generic type parameters, Object, and the internal NoContent marker
        if (targetType.TypeKind == TypeKind.TypeParameter ||
            targetType.SpecialType == SpecialType.System_Object ||
            targetType.Name == "NoContent")
        {
            return;
        }

        var jsonSerializableAttributeTypeSymbol = context.Compilation.GetTypeByMetadataName("System.Text.Json.Serialization.JsonSerializableAttribute");
        if (jsonSerializableAttributeTypeSymbol == null) return;

        var isRegistered = false;
        var jsonSerializerContextNames = new[]
        {
            "Docker.DotNet.DockerExtendedJsonSerializerContext",
            "Docker.DotNet.Models.DockerModelsJsonSerializerContext"
        };

        foreach (var jsonSerializerContextName in jsonSerializerContextNames)
        {
            var jsonSerializerContextTypeSymbol = context.Compilation.GetTypeByMetadataName(jsonSerializerContextName);
            if (jsonSerializerContextTypeSymbol == null) continue;

            if (jsonSerializerContextTypeSymbol.GetAttributes().Any(attr =>
                SymbolEqualityComparer.Default.Equals(attr.AttributeClass, jsonSerializableAttributeTypeSymbol) &&
                attr.ConstructorArguments.Any(arg =>
                    arg.Value is ITypeSymbol reg && SymbolEqualityComparer.Default.Equals(reg, targetType))))
            {
                isRegistered = true;
                break;
            }
        }

        if (!isRegistered)
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, location, targetType.ToDisplayString(), sourceName));
        }
    }
}
