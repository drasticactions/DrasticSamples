// <copyright file="MauiProgram.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.Feed.Database.LiteDB;
using Drastic.Feed.Rss.FeedReader;
using Drastic.Feed.Services;
using Drastic.Feed.Templates.Handlebars;
using Drastic.Services;
using Microsoft.Extensions.Logging;

namespace DotnetRss.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Services.AddSingleton<IDatabaseService, LiteDBDatabaseContext>();
        builder.Services.AddSingleton<IAppDispatcher, MauiApplicationDispatcher>();
        builder.Services.AddSingleton<IErrorHandlerService, MauiErrorHandlerService>();
        builder.Services.AddSingleton<IFeedService, FeedReaderService>();
        builder.Services.AddSingleton<ITemplateService, HandlebarsTemplateService>();
        builder.Services.AddTransient<DotnetRss.ViewModels.RssFeedArticleViewModel>();
        builder.Services.AddTransient<DotnetRss.ViewModels.RssFeedItemListViewModel>();
        builder.Services.AddTransient<DotnetRss.ViewModels.RssFeedListViewModel>();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
