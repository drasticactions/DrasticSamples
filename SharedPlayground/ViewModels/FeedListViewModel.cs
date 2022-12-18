// <copyright file="FeedListViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using Bogus;
using Drastic.Feed.Models;
using Drastic.ViewModels;

namespace SharedPlayground.ViewModels
{
    public class FeedListViewModel : BaseViewModel
    {
        public ObservableCollection<FeedListItem> FeedListItems { get; set; } = new ObservableCollection<FeedListItem>();

        public FeedListViewModel(IServiceProvider services)
            : base(services)
        {
            var faker = new Faker<FeedListItem>()
                .RuleFor(f => f.Id, f => f.Random.Int())
                .RuleFor(f => f.Name, f => f.Lorem.Word())
                .RuleFor(f => f.Description, f => f.WaffleHtml(1, false))
                .RuleFor(f => f.Language, f => f.Locale)
                .RuleFor(f => f.LastUpdatedDate, f => f.Date.Past())
                .RuleFor(f => f.ImageUri, f => new Uri(f.Image.PicsumUrl()))
                .RuleFor(f => f.Uri, f => new Uri(f.Internet.Url()))
                .RuleFor(f => f.Image, f => default)
                .RuleFor(f => f.Link, f => f.Internet.Url());

            for (var i = 0; i < 100; i++)
            {
               this.FeedListItems.Add(faker.Generate());
            }
        }
    }
}
