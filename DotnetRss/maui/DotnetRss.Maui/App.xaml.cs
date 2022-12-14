// <copyright file="App.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DotnetRss.Maui;

public partial class App : Application
{
    public App(IServiceProvider services)
    {
        this.MainPage = new NavigationPage(new FeedListPage(services));
    }
}
