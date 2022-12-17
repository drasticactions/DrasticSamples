using Bogus;
using Drastic.Media.Core.Model;
using Drastic.ViewModels;
using System.Collections.ObjectModel;

namespace SharedPlayground.ViewModels
{
    public class PodcastShowListViewModel : BaseViewModel
    {
        public ObservableCollection<PodcastShowItem> Items { get; set; } = new ObservableCollection<PodcastShowItem>();

        public PodcastShowListViewModel(IServiceProvider services)
            : base(services)
        {
            var faker = new Faker<PodcastShowItem>()
                .RuleFor(p => p.Id, f => f.Random.Int())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence())
                .RuleFor(p => p.Email, f => f.Internet.Email())
                .RuleFor(p => p.Language, f => f.Locale)
                .RuleFor(p => p.SiteUri, f => new Uri(f.Internet.Url()))
                .RuleFor(p => p.Author, f => f.Name.FullName())
                .RuleFor(p => p.Description, f => f.Lorem.Word())
                .RuleFor(p => p.PodcastFeed, f => new Uri(f.Internet.Url()))
                .RuleFor(p => p.Image, f => new Uri(f.Image.PicsumUrl()))
                .RuleFor(p => p.Copyright, f => f.Lorem.Word())
                .RuleFor(p => p.Updated, f => f.Date.Past())
                .RuleFor(p => p.Episodes, f => default);

            for (var i = 0; i < 100; i++)
            {
                this.Items.Add(faker.Generate());
            }
        }
    }
}
