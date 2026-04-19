namespace Docker.DotNet.SourceAnalyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class JsonSerializerAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "DDN001";
    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        "Missing JsonSerializable attribute",
        "Type '{0}' used in Docker.DotNet.DockerClient.MakeRequestAsync is not registered in the DockerExtendedJsonSerializerContext",
        "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;

        if (context.SemanticModel.GetSymbolInfo(invocation).Symbol is not IMethodSymbol methodSymbol)
            return;

        if (methodSymbol.Name != "MakeRequestAsync")
            return;

        if (methodSymbol.ContainingType?.ToDisplayString() != "Docker.DotNet.DockerClient")
            return;

        if (methodSymbol.TypeArguments.Length == 0)
            return;

        var targetType = methodSymbol.TypeArguments[0];

        if (targetType.TypeKind == TypeKind.TypeParameter)
            return;

        if (targetType.Name == "NoContent" && targetType.ContainingType?.ToDisplayString() == "Docker.DotNet.DockerClient")
            return;

        var jsonSerializableAttributeSymbol = context.Compilation.GetTypeByMetadataName("System.Text.Json.Serialization.JsonSerializableAttribute");
        if (jsonSerializableAttributeSymbol == null)
            return;

        var isRegistered = false;

        foreach (var jsonSerializerContextTypeName in new[] { "Docker.DotNet.DockerExtendedJsonSerializerContext", "Docker.DotNet.Models.DockerModelsJsonSerializerContext" })
        {
            var jsonSerializerContextType = context.Compilation.GetTypeByMetadataName(jsonSerializerContextTypeName);
            if (jsonSerializerContextType == null) continue;

            isRegistered =
                jsonSerializerContextType.GetAttributes().Any(attr =>
                    SymbolEqualityComparer.Default.Equals(attr.AttributeClass, jsonSerializableAttributeSymbol) &&
                    attr.ConstructorArguments.Any(arg =>
                        arg.Value is ITypeSymbol registeredType &&
                        SymbolEqualityComparer.Default.Equals(registeredType, targetType)));

            if (isRegistered) break;
        }

        if (!isRegistered)
        {
            var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation(), targetType.ToDisplayString());
            context.ReportDiagnostic(diagnostic);
        }
    }
}
