using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace KolibSoft.Votes.Core;

/// <summary>
/// Represents an 8-digit hexadecimal number that represents a 128-bit unsigned integer.
/// </summary>
/// <param name="utf8">UTF8 text.</param>
public struct VoteIssue(ArraySegment<byte> utf8)
{

    /// <summary>
    /// UTF8 internal data.
    /// </summary>
    public ArraySegment<byte> Data { get; } = utf8;

    /// <summary>
    /// Gets the string representation of the issue.
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

        var other = (VoteIssue)obj;
        return this == other;
    }

    public override int GetHashCode()
    {
        return Data.GetHashCode();
    }

    /// <summary>
    /// Verify if the provided UTF8 text is a valid issue.
    /// </summary>
    /// <param name="utf8">UTF8 text.</param>
    /// <returns></returns>
    public static bool Verify(ReadOnlySpan<byte> utf8)
    {
        if (utf8.Length != 8) return false;
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
    /// Verify if the provided string is a valid issue.
    /// </summary>
    /// <param name="string">String.</param>
    /// <returns></returns>
    public static bool Verify(ReadOnlySpan<char> @string)
    {
        if (@string.Length != 8) return false;
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
    /// Parses an UTF8 text into a issue.
    /// </summary>
    /// <param name="utf8">UTF8 text.</param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static VoteIssue Parse(ReadOnlySpan<byte> utf8)
    {
        if (!Verify(utf8))
            throw new FormatException($"Invalid issue format: {Encoding.UTF8.GetString(utf8)}");
        return new VoteIssue(utf8.ToArray());
    }

    /// <summary>
    /// Parses an string into a issue.
    /// </summary>
    /// <param name="string">String.</param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    public static VoteIssue Parse(ReadOnlySpan<char> @string)
    {
        if (!Verify(@string))
            throw new FormatException($"Invalid issue format: {@string}");
        var utf8 = new byte[8];
        Encoding.UTF8.GetBytes(@string, utf8);
        return new VoteIssue(utf8);
    }

    public static bool operator ==(VoteIssue lhs, VoteIssue rhs)
    {
        return lhs.Data.SequenceEqual(rhs.Data) || (int)lhs == (int)rhs;
    }

    public static bool operator !=(VoteIssue lhs, VoteIssue rhs)
    {
        return !lhs.Data.SequenceEqual(rhs.Data) && (int)lhs != (int)rhs;
    }

    public static implicit operator int(VoteIssue issue)
    {
        var @string = issue.ToString();
        var @int = Convert.ToInt32(@string, 16);
        return @int;
    }

    public static implicit operator VoteIssue(int @int)
    {
        var @string = @int.ToString();
        var issue = Parse(@string);
        return issue;
    }

}