using System.Buffers.Text;
using System.Text;

namespace SpreadCheetah.Helpers;

internal static class Utf8Helper
{
    public const int MaxBytePerChar = 6;

    private static readonly UTF8Encoding Utf8NoBom = new(false);

    public static int GetByteCount(string chars) => Utf8NoBom.GetByteCount(chars);
    public static byte[] GetBytes(string s) => Utf8NoBom.GetBytes(s);

    public static int GetBytes(int number, Span<byte> destination)
    {
        Utf8Formatter.TryFormat(number, destination, out var bytesWritten);
        return bytesWritten;
    }

    public static int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes) => Utf8NoBom.GetBytes(chars, bytes);

#if NETSTANDARD2_0
    public static int GetBytes(string chars, Span<byte> bytes) => GetBytes(chars.AsSpan(), bytes);
    public static bool TryGetBytes(string? chars, Span<byte> bytes, out int bytesWritten) => TryGetBytes(chars.AsSpan(), bytes, out bytesWritten);
#endif

    public static bool TryGetBytes(ReadOnlySpan<char> chars, Span<byte> bytes, out int bytesWritten) => Utf8NoBom.TryGetBytes(chars, bytes, out bytesWritten);
}
