namespace Docker.DotNet.Models;

public enum FileSystemChangeKind
{
    [EnumMember(Value = "modify")]
    Modify,

    [EnumMember(Value = "add")]
    Add,

    [EnumMember(Value = "delete")]
    Delete
}