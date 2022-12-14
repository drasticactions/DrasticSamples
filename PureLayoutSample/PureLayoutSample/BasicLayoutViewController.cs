// <copyright file="BasicLayoutViewController.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.PureLayout;

namespace PureLayoutSample
{
    public class BasicLayoutViewController : UIViewController
    {
        private bool didSetupConstraints;

        private UIView blueView = new UIView() { BackgroundColor = UIColor.Blue };
        private UIView redView = new UIView() { BackgroundColor = UIColor.Red };
        private UIView yellowView = new UIView() { BackgroundColor = UIColor.Yellow };
        private UIView greenView = new UIView() { BackgroundColor = UIColor.Green };

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View!.AddSubview(this.blueView);
            this.View!.AddSubview(this.redView);
            this.View!.AddSubview(this.yellowView);
            this.View!.AddSubview(this.greenView);

            this.View!.SetNeedsUpdateConstraints();
        }

        public override void UpdateViewConstraints()
        {
            if (!this.didSetupConstraints)
            {
                this.blueView.AutoSetDimensionsToSize(new CGSize(50, 50));
                this.blueView.AutoCenterInSuperview();

                // Red view is positioned at the bottom right corner of the blue view, with the same width, and a height of 40 pt
                this.redView.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.blueView);
                this.redView.AutoPinEdge(ALEdge.Left, ALEdge.Right, this.blueView);
                this.redView.AutoMatchDimension(ALDimension.Width, ALDimension.Width, this.blueView);
                this.redView.AutoSetDimension(ALDimension.Height, 40f);

                // Yellow view is positioned 10 pt below the red view, extending across the screen with 20 pt insets from the edges,
                // and with a fixed height of 25 pt
                this.yellowView.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.redView, 10f);
                this.yellowView.AutoSetDimension(ALDimension.Height, 25f);
                this.yellowView.AutoPinEdgeToSuperviewEdge(ALEdge.Left, 20f);
                this.yellowView.AutoPinEdgeToSuperviewEdge(ALEdge.Right, 20f);

                // Green view is positioned 10 pt below the yellow view, aligned to the vertical axis of its superview,
                // with its height twice the height of the yellow view and its width fixed to 150 pt
                this.greenView.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.yellowView, 10f);
                this.greenView.AutoAlignAxisToSuperviewAxis(ALAxis.Vertical);
                this.greenView.AutoMatchDimensionWithMultiplier(ALDimension.Height, ALDimension.Height, this.yellowView, 2f);
                this.greenView.AutoSetDimension(ALDimension.Width, 150f);

                this.didSetupConstraints = true;
            }

            base.UpdateViewConstraints();
        }
    }
}