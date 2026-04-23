#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
namespace Microsoft.Net.Http.Client;

internal sealed class ArrayBufferWriterRequestBuffer
{
    private readonly ArrayBufferWriter<byte> _buffer = new();

    public ReadOnlyMemory<byte> GetWrittenMemory()
    {
        return _buffer.WrittenMemory;
    }

    public void WriteString(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

#if NET
        Encoding.ASCII.GetBytes(value, _buffer);
#else
        var length = Encoding.ASCII.GetByteCount(value);
        var bytes = _buffer.GetSpan(length);
        var written = Encoding.ASCII.GetBytes(value, bytes);
        _buffer.Advance(written);
#endif
    }

    public void WriteBytes(ReadOnlySpan<byte> value)
    {
        _buffer.Write(value);
    }
}
#endif