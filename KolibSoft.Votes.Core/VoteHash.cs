using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace KolibSoft.Votes.Core;

/// <summary>
/// Represents an 32-digit hexadecimal number that represents a 128-bit unsigned integer.
/// </summary>
/// <param name="utf8">UTF8 text.</param>
public struct VoteHash(ArraySegment<byte> utf8)
{

    /// <summary>
    /// UTF8 internal data.
    /// </summary>
    public ArraySegment<byte> Data { get; } = utf8;

    /// <summary>
    /// Gets the string representation of the hash.
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

        var other = (VoteHash)obj;
        return this == other;
    }

    public override int GetHashCode()
    {
        return Data.GetHashCode();
    }

    /// <summary>
    /// Verify if the provided UTF8 text is a valid hash.
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
    /// Verify if the provided string is a valid hash.
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
    /// Parses an UTF8 text into a hash.
    /// </summary>
    /// <param name="utf8">UTF8 text.</param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static VoteHash Parse(ReadOnlySpan<byte> utf8)
    {
        if (!Verify(utf8))
            throw new FormatException($"Invalid hash format: {Encoding.UTF8.GetString(utf8)}");
        return new VoteHash(utf8.ToArray());
    }

    /// <summary>
    /// Parses an string into a hash.
    /// </summary>
    /// <param name="string">String.</param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static VoteHash Parse(ReadOnlySpan<char> @string)
    {
        if (!Verify(@string))
            throw new FormatException($"Invalid hash format: {@string}");
        var utf8 = new byte[32];
        Encoding.UTF8.GetBytes(@string, utf8);
        return new VoteHash(utf8);
    }

    public static bool operator ==(VoteHash lhs, VoteHash rhs)
    {
        return lhs.Data.SequenceEqual(rhs.Data) || (BigInteger)lhs == (BigInteger)rhs;
    }

    public static bool operator !=(VoteHash lhs, VoteHash rhs)
    {
        return !lhs.Data.SequenceEqual(rhs.Data) && (BigInteger)lhs != (BigInteger)rhs;
    }

    public static implicit operator BigInteger(VoteHash hash)
    {
        var @string = hash.ToString();
        var @int = BigInteger.Parse(@string, NumberStyles.HexNumber);
        return @int;
    }

    public static implicit operator VoteHash(BigInteger @int)
    {
        var @string = @int.ToString();
        var hash = Parse(@string);
        return hash;
    }

    /// <summary>
    /// Vote hash 00000000000000000000000000000000.
    /// </summary>
    public static readonly VoteHash None = Parse("00000000000000000000000000000000");

}