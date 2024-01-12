using KolibSoft.Votes.Core;

var message = new VoteMessage
{
    Node = VoteHash.Parse("11111111111111111111111111111111"),
    NodeSignature = VoteHash.Parse("11111111111111111111111111111111"),
    Value = VoteValue.Parse("aaaaaaaa"),
    Author = VoteHash.Parse("22222222222222222222222222222222"),
    AuthorSignature = VoteHash.Parse("22222222222222222222222222222222"),
    Issue = VoteValue.Parse("bbbbbbbb"),
    Content = VoteContent.Parse("TEXT DESCRIPTION")
};

var @string = message.ToString();
message = VoteMessage.Parse(@string);

Console.WriteLine(@string);
Console.WriteLine(message);