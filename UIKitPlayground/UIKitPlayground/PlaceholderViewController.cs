// <copyright file="PlaceholderViewController.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.PureLayout;

namespace UIKitPlayground
{
    public class PlaceholderViewController : UIViewController
    {
        private bool didSetupConstraints;
        private PlaceholderView view;

        public PlaceholderViewController()
        {
            this.view = new PlaceholderView();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View!.BackgroundColor = UIColor.Blue;
            this.View!.AddSubview(this.view);
            this.View!.SetNeedsUpdateConstraints();
        }

        public override void UpdateViewConstraints()
        {
            if (!this.didSetupConstraints)
            {
                this.view.AutoCenterInSuperview();
            }

            base.UpdateViewConstraints();
        }
    }
}