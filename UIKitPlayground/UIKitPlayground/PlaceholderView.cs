using System;
using Drastic.PureLayout;

namespace UIKitPlayground
{
    public class PlaceholderView : UIView
    {
        private bool didSetupConstraints;
        private UIImageView imageView;
        private UILabel title = new UILabel() {
            BackgroundColor = UIColor.Orange,
            TextColor = UIColor.White,
            Text = "Welcome to the UIKit Playground",
            TextAlignment = UITextAlignment.Center };

        public PlaceholderView()
        {
            this.BackgroundColor = UIColor.Green;
            this.imageView = new UIImageView() { BackgroundColor = UIColor.Orange, Image = UIImage.FromBundle("DotNetBot") };
            this.AddSubview(this.imageView);
            this.AddSubview(this.title);
        }

        public override void UpdateConstraints()
        {
            if (!this.didSetupConstraints)
            {
                this.imageView.AutoCenterInSuperview();
                this.title.AutoAlignAxisToSuperviewAxis(ALAxis.Vertical);
                this.title.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.imageView, 10f);
                this.didSetupConstraints = true;
            }

            base.UpdateConstraints();
        }
    }
}