# SpecGen

A tool that reflects the Docker client [engine-api](https://github.com/docker/engine-api) in order to generate C# classes that match its model for [Docker.DotNet Models](../../src/Docker.DotNet/Models).

----

## How to use:

Use the update scripts from the repository root to fetch Docker API/client definitions for a specific [release tag](https://github.com/moby/moby/releases/) and regenerate [Docker.DotNet Models](../../src/Docker.DotNet/Models):

```powershell
.\tools\specgen\update-generated-code.ps1 -ReleaseTag docker-v29.1.5
```

```bash
./tools/specgen/update-generated-code.sh docker-v29.1.5
```

The scripts will:

- update `github.com/moby/moby/api` and `github.com/moby/moby/client` to the given release tag,
- build `specgen`,
- remove existing `*.Generated.cs` model files,
- regenerate model files into [Docker.DotNet/Models](../../src/Docker.DotNet/Models).

----

## About the structure of the tool:

Many of Docker's engine-api types are used for both the query string and json body. Because there is no way to attribute this on the engine-api types themselves we have broken the tool into a few specific areas:

`Csharptype.go` : Contains the translation/serialization code for writing the C# classes.

`Modeldefs.go` : Contains the parts of engine-api that are used as parameters or require custom serialization that needs to be explicitly handled differently.

`Specgen.go` : Contains the majority of the code that reflects the engine-api structs and converts them to the C# in-memory abstractions.

----

## About the structure of the output:

The resulting C# type contains both the `QueryString` parameters as well as the `JSON` body models in one object. This simplifies the calling API quite dramatically. For example:

```C#
namespace Docker.DotNet.Models
{
    public class ContainerAttachParameters // (main.ContainerAttachParameters)
    {
        [QueryStringParameter("stream", false, typeof(BoolQueryStringConverter))]
        public bool? Stream { get; set; }

        [QueryStringParameter("stdin", false, typeof(BoolQueryStringConverter))]
        public bool? Stdin { get; set; }

        // etc...
    }
}
```

What you are seeing here is that in order to interact with the remote API the query string allows `optional` `stream` and `stdin` boolean parameters. Because they are optional the generated code adds the `?` to signify the absence of the value versus passing a `false` as the value.

```C#
namespace Docker.DotNet.Models
{
    public class ContainerConfig // (container.Config)
    {
        [JsonPropertyName("Hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("Domainname")]
        public string Domainname { get; set; }

        // etc...
    }
}
```

Here you are actually seeing that the field values are marshalled in the request body based on the `JsonPropertyName` attribute. The resulting `JSON` will not contain the field if its value is equal to its default value in C#.

A few customizations are taken in order to simplify the API even more. Take for example [RestartPolicyKind.cs](../../src/Docker.DotNet/Models/RestartPolicyKind.cs). You will see the generated model contains:

```C#
namespace Docker.DotNet.Models
{
    public class RestartPolicy // (container.RestartPolicy)
    {
        [JsonPropertyName("Name")]
        public RestartPolicyKind Name { get; set; }

        [JsonPropertyName("MaximumRetryCount")]
        public long MaximumRetryCount { get; set; }
    }
}
```

The property `Name` actually uses the `EnumMember` value instead of its integer value. In order to do this because Go does not have enum values if you look at `specgen.go` you will see a `typeCustomizations` map where this field has been explicitly overridden in how its generated. You can use this model to accomplish more of the same where you see fit.
