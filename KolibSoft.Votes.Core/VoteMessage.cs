namespace KolibSoft.Votes.Core;

public class VoteMessage
{

    public VoteNode Node { get; set; }
    public VoteSignature NodeSignature { get; set; }
    public VoteValue Value { get; set; }

    public VoteNode Author { get; set; }
    public VoteSignature AuthorSignature { get; set; }
    public VoteIssue Issue { get; set; }

    public VoteContent Content { get; set; }

    public int Length => Node.Data.Count + NodeSignature.Data.Count + Value.Data.Count + Author.Data.Count + AuthorSignature.Data.Count + Issue.Data.Count + Content.Data.Count + 6;

    public override string ToString()
    {
        return $"{Node}.{NodeSignature} {Value}\n{Author}.{AuthorSignature} {Issue}\n{Content}";
    }

}