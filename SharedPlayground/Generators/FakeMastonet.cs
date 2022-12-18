using Bogus;
using Mastonet.Entities;
using System.Security.Principal;


namespace SharedPlayground.Generators
{
    public class FakeStatus : Bogus.Faker<Mastonet.Entities.Status>
    {
        private FakeAccount account;

        public FakeStatus(FakeAccount? account = default)
        {
            this.account = account ?? new FakeAccount();
            RuleFor(s => s.Id, f => f.Lorem.Word());
            RuleFor(s => s.Uri, f => f.Lorem.Word());
            RuleFor(s => s.CreatedAt, f => f.Date.Past());
            RuleFor(s => s.Account, f => this.account.Generate());
            RuleFor(s => s.Content, f => f.Lorem.Word());
            RuleFor(s => s.Visibility, f => default);
            RuleFor(s => s.Sensitive, f => default);
            RuleFor(s => s.SpoilerText, f => f.WaffleHtml(1, false));
            RuleFor(s => s.MediaAttachments, f => default);
            RuleFor(s => s.Application, f => default);
            RuleFor(s => s.Mentions, f => default);
            RuleFor(s => s.Tags, f => default);
            RuleFor(s => s.Emojis, f => default);
            RuleFor(s => s.ReblogCount, f => f.Random.Int());
            RuleFor(s => s.FavouritesCount, f => f.Random.Int());
            RuleFor(s => s.RepliesCount, f => f.Random.Int());
            RuleFor(s => s.Url, f => f.Lorem.Word());
            RuleFor(s => s.InReplyToId, f => f.Lorem.Word());
            RuleFor(s => s.InReplyToAccountId, f => f.Lorem.Word());
            RuleFor(s => s.Reblog, f => default);
            RuleFor(s => s.Poll, f => default);
            RuleFor(s => s.Card, f => default);
            RuleFor(s => s.Language, f => f.Locale);
            RuleFor(s => s.Text, f => f.WaffleHtml(1, false));
            RuleFor(s => s.Favourited, f => default);
            RuleFor(s => s.Reblogged, f => default);
            RuleFor(s => s.Muted, f => default);
            RuleFor(s => s.Bookmarked, f => default);
            RuleFor(s => s.Pinned, f => default);
        }
    }

    public class FakeAccount : Bogus.Faker<Mastonet.Entities.Account>
    {
        public FakeAccount()
        {
            RuleFor(o => o.AccountName, f => f.Internet.UserName());
            RuleFor(o => o.DisplayName, f => f.Internet.UserNameUnicode());
            RuleFor(o => o.AvatarUrl, f => f.Internet.Avatar());

            RuleFor(a => a.Id, f => f.Lorem.Word());
            RuleFor(a => a.UserName, f => f.Lorem.Word());
            RuleFor(a => a.ProfileUrl, f => f.Internet.Url());
            RuleFor(a => a.Note, f => f.WaffleHtml(1, false));
            RuleFor(a => a.StaticAvatarUrl, f => f.Internet.Avatar());
            RuleFor(a => a.HeaderUrl, f => f.Image.PicsumUrl());
            RuleFor(a => a.StaticHeaderUrl, f => f.Image.PicsumUrl());
            RuleFor(a => a.Locked, f => f.Random.Bool());
            RuleFor(a => a.Emojis, f => default);
            RuleFor(a => a.Discoverable, f => default);
            RuleFor(a => a.CreatedAt, f => f.Date.Past());
            RuleFor(a => a.LastStatusAt, f => default);
            RuleFor(a => a.StatusesCount, f => f.Random.Int());
            RuleFor(a => a.FollowersCount, f => f.Random.Int());
            RuleFor(a => a.FollowingCount, f => f.Random.Int());
            RuleFor(a => a.Moved, f => default);
            RuleFor(a => a.Fields, f => default);
            RuleFor(a => a.Bot, f => default);
            RuleFor(a => a.Source, f => default);
            RuleFor(a => a.Suspended, f => f.Random.Bool());
            RuleFor(a => a.MuteExpiresAt, f => default);
        }
    }
}
