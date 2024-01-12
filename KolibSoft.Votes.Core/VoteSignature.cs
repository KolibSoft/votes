using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace KolibSoft.Votes.Core;

/// <summary>
/// Represents an 32-digit hexadecimal number that represents a 128-bit unsigned integer.
/// </summary>
/// <param name="utf8">UTF8 text.</param>
public struct VoteSignature(ArraySegment<byte> utf8)
{

    /// <summary>
    /// UTF8 internal data.
    /// </summary>
    public ArraySegment<byte> Data { get; } = utf8;

    /// <summary>
    /// Gets the string representation of the signature.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Encoding.UTF8.GetString(Data);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (VoteSignature)obj;
        return this == other;
    }

    public override int GetHashCode()
    {
        return Data.GetHashCode();
    }

    /// <summary>
    /// Verify if the provided UTF8 text is a valid signature.
    /// </summary>
    /// <param name="utf8">UTF8 text.</param>
    /// <returns></returns>
    public static bool Verify(ReadOnlySpan<byte> utf8)
    {
        if (utf8.Length != 32) return false;
        for (var i = 0; i < utf8.Length; i++)
        {
            var c = utf8[i];
            if (c >= '0' && c <= '9' || c >= 'A' && c <= 'F' || c >= 'a' && c <= 'f')
                continue;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Verify if the provided string is a valid signature.
    /// </summary>
    /// <param name="string">String.</param>
    /// <returns></returns>
    public static bool Verify(ReadOnlySpan<char> @string)
    {
        if (@string.Length != 32) return false;
        for (var i = 0; i < @string.Length; i++)
        {
            var c = @string[i];
            if (c >= '0' && c <= '9' || c >= 'A' && c <= 'F' || c >= 'a' && c <= 'f')
                continue;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Parses an UTF8 text into a signature.
    /// </summary>
    /// <param name="utf8">UTF8 text.</param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static VoteSignature Parse(ReadOnlySpan<byte> utf8)
    {
        if (!Verify(utf8))
            throw new FormatException($"Invalid signature format: {Encoding.UTF8.GetString(utf8)}");
        return new VoteSignature(utf8.ToArray());
    }

    /// <summary>
    /// Parses an string into a signature.
    /// </summary>
    /// <param name="string">String.</param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static VoteSignature Parse(ReadOnlySpan<char> @string)
    {
        if (!Verify(@string))
            throw new FormatException($"Invalid signature format: {@string}");
        var utf8 = new byte[32];
        Encoding.UTF8.GetBytes(@string, utf8);
        return new VoteSignature(utf8);
    }

    public static bool operator ==(VoteSignature lhs, VoteSignature rhs)
    {
        return lhs.Data.SequenceEqual(rhs.Data) || (BigInteger)lhs == (BigInteger)rhs;
    }

    public static bool operator !=(VoteSignature lhs, VoteSignature rhs)
    {
        return !lhs.Data.SequenceEqual(rhs.Data) && (BigInteger)lhs != (BigInteger)rhs;
    }

    public static implicit operator BigInteger(VoteSignature signature)
    {
        var @string = signature.ToString();
        var @int = BigInteger.Parse(@string, NumberStyles.HexNumber);
        return @int;
    }

    public static implicit operator VoteSignature(BigInteger @int)
    {
        var @string = @int.ToString();
        var signature = Parse(@string);
        return signature;
    }

}