// <copyright file="RssFeedListViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using Drastic.Feed.Models;
using Drastic.Tools;

namespace DotnetRss.ViewModels
{
    /// <summary>
    /// Rss Feed List View Model.
    /// </summary>
    public class RssFeedListViewModel : DotnetRssBaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeedListViewModel"/> class.
        /// </summary>
        /// <param name="services"><see cref="IServiceProvider"/>.</param>
        public RssFeedListViewModel(IServiceProvider services)
            : base(services)
        {
            this.FeedListItems = new ObservableCollection<FeedListItem>();
            this.FeedListItemSelectedCommand = new AsyncCommand<FeedListItem>(
            async (item) => this.OnFeedListItemSelected?.Invoke(this, new FeedListItemSelectedEventArgs(item)),
            null,
            this.ErrorHandler);
            this.OnFeedListItemUpdated += this.RssFeedListViewModel_OnFeedListItemUpdated;
        }

        /// <summary>
        /// Fired when a feed list item updates.
        /// </summary>
        public event EventHandler<FeedListItemSelectedEventArgs>? OnFeedListItemSelected;

        /// <summary>
        /// Gets the UpdateFeedListItem.
        /// </summary>
        public AsyncCommand<FeedListItem> FeedListItemSelectedCommand { get; private set; }

        /// <summary>
        /// Gets the list of feed list items.
        /// </summary>
        public ObservableCollection<FeedListItem> FeedListItems { get; }

        /// <inheritdoc/>
        public override async Task OnLoad()
        {
            await base.OnLoad();

            // If we don't have items when loading the VM, load it with the cache.
            // Otherwise, have the user load it themselves.
            // Ex. Navigating back in a Navigation View.
            if (!this.FeedListItems.Any())
            {
                this.UpdateFeeds();
            }
        }

        private void UpdateFeeds()
        {
            this.FeedListItems.Clear();

            var feedItems = this.Context.GetFeedListItems();
            foreach (var item in feedItems)
            {
                this.FeedListItems.Add(item);
            }
        }

        private void RssFeedListViewModel_OnFeedListItemUpdated(object? sender, FeedListItemUpdatedEventArgs e)
        {
            var item = this.FeedListItems.FirstOrDefault(n => n.Uri == e.FeedListItem.Uri);
            if (item is not null)
            {
                this.FeedListItems[this.FeedListItems.IndexOf(item)] = e.FeedListItem;
            }
            else
            {
                this.FeedListItems.Add(e.FeedListItem);
            }
        }
    }
}