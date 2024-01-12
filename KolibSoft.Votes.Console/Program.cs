using KolibSoft.Votes.Core;

var message = new VoteMessage
{
    Node = VoteNode.Parse("11111111111111111111111111111111"),
    NodeSignature = VoteSignature.Parse("11111111111111111111111111111111"),
    Value = VoteValue.Parse("aaaaaaaa"),
    Author = VoteNode.Parse("22222222222222222222222222222222"),
    AuthorSignature = VoteSignature.Parse("22222222222222222222222222222222"),
    Issue = VoteIssue.Parse("bbbbbbbb"),
    Content = VoteContent.Parse("TEXT DESCRIPTION")
};

Console.WriteLine(message);