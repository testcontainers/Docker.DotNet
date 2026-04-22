#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP2_1_OR_GREATER
namespace Microsoft.Net.Http.Client;

internal sealed class MemoryStreamRequestBuffer : IDisposable
{
    private readonly MemoryStream _buffer = new();

    public void Dispose()
    {
        _buffer.Dispose();
    }

    public ReadOnlyMemory<byte> GetWrittenMemory()
    {
        if (_buffer.TryGetBuffer(out var buffer))
        {
            return buffer;
        }

        return _buffer.ToArray();
    }

    public void WriteString(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        var bytes = Encoding.ASCII.GetBytes(value);
        _buffer.Write(bytes, 0, bytes.Length);
    }

    public void WriteBytes(ReadOnlySpan<byte> value)
    {
        if (value.Length == 0)
        {
            return;
        }

        var buffer = value.ToArray();
        _buffer.Write(buffer, 0, buffer.Length);
    }
}
#endif