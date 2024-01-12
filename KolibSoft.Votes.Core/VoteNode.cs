using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace KolibSoft.Votes.Core;

/// <summary>
/// Represents an 32-digit hexadecimal number that represents a 128-bit unsigned integer.
/// </summary>
/// <param name="utf8">UTF8 text.</param>
public struct VoteNode(ArraySegment<byte> utf8)
{

    /// <summary>
    /// UTF8 internal data.
    /// </summary>
    public ArraySegment<byte> Data { get; } = utf8;

    /// <summary>
    /// Gets the string representation of the node.
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

        var other = (VoteNode)obj;
        return this == other;
    }

    public override int GetHashCode()
    {
        return Data.GetHashCode();
    }

    /// <summary>
    /// Verify if the provided UTF8 text is a valid node.
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
    /// Verify if the provided string is a valid node.
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
    /// Parses an UTF8 text into a node.
    /// </summary>
    /// <param name="utf8">UTF8 text.</param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static VoteNode Parse(ReadOnlySpan<byte> utf8)
    {
        if (!Verify(utf8))
            throw new FormatException($"Invalid node format: {Encoding.UTF8.GetString(utf8)}");
        return new VoteNode(utf8.ToArray());
    }

    /// <summary>
    /// Parses an string into a node.
    /// </summary>
    /// <param name="string">String.</param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static VoteNode Parse(ReadOnlySpan<char> @string)
    {
        if (!Verify(@string))
            throw new FormatException($"Invalid node format: {@string}");
        var utf8 = new byte[32];
        Encoding.UTF8.GetBytes(@string, utf8);
        return new VoteNode(utf8);
    }

    public static bool operator ==(VoteNode lhs, VoteNode rhs)
    {
        return lhs.Data.SequenceEqual(rhs.Data) || (BigInteger)lhs == (BigInteger)rhs;
    }

    public static bool operator !=(VoteNode lhs, VoteNode rhs)
    {
        return !lhs.Data.SequenceEqual(rhs.Data) && (BigInteger)lhs != (BigInteger)rhs;
    }

    public static implicit operator BigInteger(VoteNode node)
    {
        var @string = node.ToString();
        var @int = BigInteger.Parse(@string, NumberStyles.HexNumber);
        return @int;
    }

    public static implicit operator VoteNode(BigInteger @int)
    {
        var @string = @int.ToString();
        var node = Parse(@string);
        return node;
    }

}