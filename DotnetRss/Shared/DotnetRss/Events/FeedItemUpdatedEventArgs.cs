﻿// <copyright file="FeedItemUpdatedEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.Feed.Models;

namespace DotnetRss
{
    public class FeedItemUpdatedEventArgs : EventArgs
    {
        private readonly FeedItem feedItem;

        private readonly FeedListItem feedListItem;

        public FeedItemUpdatedEventArgs(FeedListItem feedItem, FeedItem item)
        {
            this.feedListItem = feedItem;
            this.feedItem = item;
        }

        public FeedListItem FeedListItem => this.feedListItem;

        public FeedItem FeedItem => this.feedItem;
    }
}
