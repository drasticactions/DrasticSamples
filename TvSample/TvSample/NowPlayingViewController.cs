// <copyright file="NowPlayingViewController.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.PureLayout;

namespace TvSample
{
    public class NowPlayingViewController : UIViewController
    {
        private UIProgressView SongProgressView { get; } = new UIProgressView();
        private UILabel ArtistName { get; } = new UILabel();

        public NowPlayingViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View!.Add(this.ArtistName);

            ArtistName.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
            ArtistName.ContentMode = UIViewContentMode.Left;
            ArtistName.Font = UIFont.SystemFontOfSize(25, UIFontWeight.Regular);
            ArtistName.Frame = new CGRect(685, 870, 550, 40);
            ArtistName.SetContentHuggingPriority((float)251, UILayoutConstraintAxis.Horizontal);
            ArtistName.SetContentHuggingPriority((float)251, UILayoutConstraintAxis.Vertical);
            ArtistName.TextAlignment = UITextAlignment.Center;
            ArtistName.TextColor = UIColor.White;

            ArtistName.Text = "Test";
        }
    }
}