using System.Text;

namespace KolibSoft.Votes.Core;

public class VoteMessage
{

    public VoteHash Node { get; set; }
    public VoteHash NodeSignature { get; set; }
    public VoteValue Value { get; set; }

    public VoteHash Author { get; set; }
    public VoteHash AuthorSignature { get; set; }
    public VoteValue Issue { get; set; }

    public VoteContent Content { get; set; }

    public int Length => Node.Data.Count + NodeSignature.Data.Count + Value.Data.Count + 3 +
                         Author.Data.Count + AuthorSignature.Data.Count + Issue.Data.Count + Content.Data.Count + 3;

    public override string ToString()
    {
        return $"{Node}.{NodeSignature} {Value}\n{Author}.{AuthorSignature} {Issue}\n{Content}";
    }

    public VoteMessage(ArraySegment<byte> utf8)
    {
        Node = new VoteHash(utf8.Slice(0, 32));
        NodeSignature = new VoteHash(utf8.Slice(33, 32));
        Value = new VoteValue(utf8.Slice(66, 8));
        Author = new VoteHash(utf8.Slice(75, 32));
        AuthorSignature = new VoteHash(utf8.Slice(108, 32));
        Issue = new VoteValue(utf8.Slice(141, 8));
        Content = new VoteContent(utf8.Slice(150));
    }

    public VoteMessage()
    {
        Content = VoteContent.None;
    }

    public static bool Verify(ReadOnlySpan<byte> utf8)
    {
        if (utf8.Length < 150) return false;
        var result = VoteHash.Verify(utf8.Slice(0, 32)) && VoteHash.Verify(utf8.Slice(33, 32)) && VoteValue.Verify(utf8.Slice(66, 8)) &&
                     VoteHash.Verify(utf8.Slice(75, 32)) && VoteHash.Verify(utf8.Slice(108, 32)) && VoteValue.Verify(utf8.Slice(141, 8));
        return result;
    }

    public static bool Verify(ReadOnlySpan<char> @string)
    {
        if (@string.Length < 150) return false;
        var result = VoteHash.Verify(@string.Slice(0, 32)) && VoteHash.Verify(@string.Slice(33, 32)) && VoteValue.Verify(@string.Slice(66, 8)) &&
                     VoteHash.Verify(@string.Slice(75, 32)) && VoteHash.Verify(@string.Slice(108, 32)) && VoteValue.Verify(@string.Slice(141, 8));
        return result;
    }

    public static VoteMessage Parse(ReadOnlySpan<byte> utf8)
    {
        if (!Verify(utf8))
            throw new FormatException($"Invalid message format: {Encoding.UTF8.GetString(utf8)}");
        var message = new VoteMessage(utf8.ToArray());
        return message;
    }

    public static VoteMessage Parse(ReadOnlySpan<char> @string)
    {
        if (!Verify(@string))
            throw new FormatException($"Invalid message format: {@string}");
        var utf8 = new byte[Encoding.UTF8.GetByteCount(@string)];
        Encoding.UTF8.GetBytes(@string, utf8);
        var message = new VoteMessage(utf8);
        return message;
    }

}