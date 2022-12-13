// <copyright file="FeedBrowser.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace DotnetRss.Maui
{
    public class FeedBrowser : WebView, IBrowser
    {
        public Task<bool> OpenAsync(Uri uri, BrowserLaunchOptions options)
        {
            throw new NotImplementedException();
        }
    }
}