// <copyright file="FeedListPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using DotnetRss.ViewModels;
using Drastic.Feed.Models;
using Drastic.Tools;

namespace DotnetRss.Maui;

public partial class FeedListPage : BasePage
{
    public FeedListPage(IServiceProvider services)
        : base(services)
    {
        this.InitializeComponent();
        this.Vm = services.ResolveWith<RssFeedListViewModel>();
    }

    async void AddFeedButton_Clicked(System.Object sender, System.EventArgs e)
    {
        RssFeedListViewModel vm = (RssFeedListViewModel)this.Vm!;
        string result = await this.DisplayPromptAsync("New Feed", "RSS Feed Address");
        if (Uri.TryCreate(result, UriKind.Absolute, out Uri? uriResult))
        {
            await vm.AddOrUpdateNewFeedListItemAsync(uriResult.ToString());
        }
    }

    async void FeedListItemsCollection_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (sender is not CollectionView view)
            return;

        if (view.SelectedItem is not FeedListItem item)
            return;

        await this.Navigation.PushAsync(new ContentPage());

        view.SelectedItem = null;
    }
}
