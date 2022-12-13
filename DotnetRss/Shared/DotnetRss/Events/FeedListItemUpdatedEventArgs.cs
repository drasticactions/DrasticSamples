// <copyright file="FeedListItemUpdatedEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.Feed.Models;

namespace DotnetRss
{
    public class FeedListItemUpdatedEventArgs : EventArgs
    {
        private readonly FeedListItem feedItem;

        public FeedListItemUpdatedEventArgs(FeedListItem item)
        {
            this.feedItem = item;
        }

        public FeedListItem FeedListItem => this.feedItem;
    }
}
