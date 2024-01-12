using System.Security.Cryptography;
using System.Text;
using KolibSoft.Votes.Core;

var rsa = RSA.Create();
var pubkey = string.Join("", rsa.ExportRSAPublicKey().Select(x => x.ToString("x2")));
var privkey = string.Join("", rsa.ExportRSAPrivateKey().Select(x => x.ToString("x2")));
Console.WriteLine(pubkey);
Console.WriteLine(privkey);

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