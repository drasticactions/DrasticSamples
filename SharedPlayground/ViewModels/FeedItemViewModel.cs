// <copyright file="FeedItemViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using Bogus;
using Drastic.Feed.Models;
using Drastic.ViewModels;

namespace SharedPlayground.ViewModels
{
    public class FeedItemViewModel : BaseViewModel
    {
        public ObservableCollection<FeedItem> Items { get; set; } = new ObservableCollection<FeedItem>();

        public FeedItemViewModel(IServiceProvider services)
            : base(services)
        {
            var faker = new Faker<FeedItem>()
                .RuleFor(f => f.Id, f => f.Random.Int())
                .RuleFor(f => f.RssId, f => f.Lorem.Word())
                .RuleFor(f => f.FeedListItemId, f => f.Random.Int())
                .RuleFor(f => f.Title, f => string.Concat(" ", f.Lorem.Words()))
                .RuleFor(f => f.Link, f => f.Internet.Url())
                .RuleFor(f => f.Description, f => f.WaffleHtml(1, false))
                .RuleFor(f => f.PublishingDate, f => f.Date.Past())
                .RuleFor(f => f.Author, f => f.Name.FullName())
                .RuleFor(f => f.Content, f => f.Lorem.Paragraphs())
                .RuleFor(f => f.Html, f => f.WaffleHtml(1, false))
                .RuleFor(f => f.ImageUrl, f => f.Lorem.Word());

            for (var i = 0; i < 100;  i++)
            {
                this.Items.Add(faker.Generate());
            }
        }
    }
}