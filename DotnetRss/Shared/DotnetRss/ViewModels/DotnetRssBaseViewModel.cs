// <copyright file="DotnetRssBaseViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using Drastic.Feed.Models;
using Drastic.Feed.Services;
using Drastic.ViewModels;

namespace DotnetRss.ViewModels
{
    public class DotnetRssBaseViewModel : BaseViewModel
    {
        public DotnetRssBaseViewModel(IServiceProvider services)
            : base(services)
        {
            this.Templates = (services.GetService(typeof(ITemplateService)) as ITemplateService)!;
            this.Context = (services.GetService(typeof(IDatabaseService)) as IDatabaseService)!;
            this.Feed = (services.GetService(typeof(IFeedService)) as IFeedService)!;
        }

        /// <summary>
        /// Fired when a feed list item updates.
        /// </summary>
        public event EventHandler<FeedListItemUpdatedEventArgs>? OnFeedListItemUpdated;

        /// <summary>
        /// Fired when a feed item updates.
        /// </summary>
        public event EventHandler<FeedItemUpdatedEventArgs>? OnFeedItemUpdated;

        /// <summary>
        /// Gets the templates context.
        /// </summary>
        internal ITemplateService Templates { get; }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        internal IDatabaseService Context { get; }

        /// <summary>
        /// Gets the Feed context.
        /// </summary>
        internal IFeedService Feed { get; }

        /// <summary>
        /// Adds New Feed List.
        /// </summary>
        /// <param name="feedUri">The Feed Uri.</param>
        /// <returns>Task.</returns>
        public async Task<FeedListItem?> AddOrUpdateNewFeedListItemAsync(string feedUri)
        {
            try
            {
                (var feed, var feedListItems) = await this.Feed.ReadFeedAsync(feedUri);
                var item = this.Context.GetFeedListItem(new Uri(feedUri));
                if (item is null)
                {
                    item = feed;
                }

                if (item is null || feedListItems is null)
                {
                    // TODO: Handle error. It shouldn't be null.
                    return null;
                }

                var result = this.Context.AddOrUpdateFeedListItem(item);

                foreach (var feedItem in feedListItems)
                {
                    feedItem.FeedListItemId = item.Id;
                    this.Context.AddOrUpdateFeedItem(feedItem);
                    this.SendFeedUpdateRequest(item, feedItem);
                }

                this.SendFeedListUpdateRequest(item);

                return item;
            }
            catch (Exception ex)
            {
                this.ErrorHandler.HandleError(ex);
            }

            return null;
        }

        /// <summary>
        /// Call OnFeedListItemUpdated event handler.
        /// </summary>
        /// <param name="item">Feed List Item.</param>
        internal void SendFeedListUpdateRequest(FeedListItem item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));
            this.OnFeedListItemUpdated?.Invoke(this, new FeedListItemUpdatedEventArgs(item));
        }

        /// <summary>
        /// Call OnFeedListItemUpdated event handler..
        /// </summary>
        /// <param name="feedItem">Feed List Item.</param>
        /// <param name="item">Feed Item.</param>
        internal void SendFeedUpdateRequest(FeedListItem feedItem, FeedItem item)
        {
            ArgumentNullException.ThrowIfNull(feedItem, nameof(feedItem));
            ArgumentNullException.ThrowIfNull(item, nameof(item));
            this.OnFeedItemUpdated?.Invoke(this, new FeedItemUpdatedEventArgs(feedItem, item));
        }

        internal Task SetIsFavoriteFeedItemAsync(FeedItem item)
        {
            item.IsFavorite = !item.IsFavorite;
            this.Context.AddOrUpdateFeedItem(item);
            return Task.CompletedTask;
        }

        internal Task SetIsReadFeedItemAsync(FeedItem item)
        {
            item.IsRead = !item.IsRead;
            this.Context.AddOrUpdateFeedItem(item);
            return Task.CompletedTask;
        }

        private static byte[] GetPlaceholderIcon()
        {
            var resource = GetResourceFileContent("Icon.favicon.ico");
            if (resource is null)
            {
                throw new Exception("Failed to get placeholder icon.");
            }

            using MemoryStream ms = new MemoryStream();
            resource.CopyTo(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Get Resource File Content via FileName.
        /// </summary>
        /// <param name="fileName">Filename.</param>
        /// <returns>Stream.</returns>
        private static Stream? GetResourceFileContent(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "DotnetRss.Assets." + fileName;
            if (assembly is null)
            {
                return null;
            }

            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}