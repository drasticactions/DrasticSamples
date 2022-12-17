using Bogus;
using Drastic.Media.Core.Model;
using Drastic.ViewModels;
using System.Collections.ObjectModel;

namespace SharedPlayground.ViewModels
{
    public class PodcastEpisodeListViewModel : BaseViewModel
    {
        public ObservableCollection<PodcastEpisodeItem> Items { get; set; } = new ObservableCollection<PodcastEpisodeItem>();

        public PodcastEpisodeListViewModel(PodcastShowItem item, IServiceProvider services)
            : base(services)
        {
            var faker = new Faker<PodcastEpisodeItem>()
                .RuleFor(p => p.Id, f => f.Random.Int())
                .RuleFor(p => p.PodcastShowItemId, f => f.Random.Int())
                .RuleFor(p => p.ReleaseDate, f => f.Date.Past())
                .RuleFor(p => p.IsDownloaded, f => f.Random.Bool())
                .RuleFor(p => p.Description, f => f.Lorem.Word())
                .RuleFor(p => p.Explicit, f => f.Random.Bool())
                .RuleFor(p => p.PodcastShowItem, f => item)
                .RuleFor(p => p.Path, f => f.Internet.Url())
                .RuleFor(p => p.OnlinePath, f => new Uri(f.Internet.Url()))
                .RuleFor(p => p.Album, f => f.Lorem.Word())
                .RuleFor(p => p.Artist, f => f.Lorem.Word())
                .RuleFor(p => p.AlbumArtist, f => f.Lorem.Word())
                .RuleFor(p => p.Duration, f => f.Date.Timespan())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence())
                .RuleFor(p => p.Tracknumber, f => default)
                .RuleFor(p => p.Year, f => f.Random.Int())
                .RuleFor(p => p.Genre, f => f.Lorem.Word())
                .RuleFor(p => p.AlbumArt, f => f.Image.PicsumUrl())
                .RuleFor(p => p.AlbumArtUri, f => new Uri(f.Image.PicsumUrl()))
                .RuleFor(p => p.DiscNumber, f => default)
                .RuleFor(p => p.Season, f => default)
                .RuleFor(p => p.Episode, f => default)
                .RuleFor(p => p.Episodes, f => default)
                .RuleFor(p => p.ShowTitle, f => default)
                .RuleFor(p => p.ThumbnailPath, f => default)
                .RuleFor(p => p.PosterPath, f => default)
                .RuleFor(p => p.Height, f => default)
                .RuleFor(p => p.Width, f => default)
                .RuleFor(p => p.PlayCount, f => f.Random.Int())
                .RuleFor(p => p.LastPosition, f => default);

            for (var i = 0; i < 100; i++)
            {
                this.Items.Add(faker.Generate());
            }
        }
    }
}
