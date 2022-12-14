using System;
using Drastic.PureLayout;

namespace PureLayoutSample
{
    public class ScrollViewController : UIViewController
    {
        private UIScrollView scrollView = new UIScrollView();
        private UIView contentView = new UIView();
        private UILabel blueLabel;

        private bool didSetupConstraints;

        public ScrollViewController()
        {
            this.blueLabel = new UILabel();
            this.blueLabel.BackgroundColor = UIColor.Blue;
            this.blueLabel.Lines = 0;
            this.blueLabel.LineBreakMode = UILineBreakMode.Clip;
            this.blueLabel.TextColor = UIColor.White;
            var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";
            this.blueLabel.Text = $"{text} {text} {text}";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View!.AddSubview(this.scrollView);
            this.scrollView.AddSubview(this.contentView);
            this.contentView.AddSubview(this.blueLabel);
            this.View!.SetNeedsUpdateConstraints();
        }

        public override void UpdateViewConstraints()
        {
            if (!this.didSetupConstraints)
            {
                this.scrollView.AutoPinEdgesToSuperviewEdges(UIEdgeInsets.Zero);
                this.contentView.AutoPinEdgesToSuperviewEdges(UIEdgeInsets.Zero);

                this.contentView.AutoMatchDimension(ALDimension.Width, ALDimension.Width, this.View!);
                this.blueLabel.AutoPinEdgeToSuperviewEdge(ALEdge.Top, 20f);
                this.blueLabel.AutoPinEdgeToSuperviewEdge(ALEdge.Leading, 20f);
                this.blueLabel.AutoPinEdgeToSuperviewEdge(ALEdge.Trailing, 20f);
                this.blueLabel.AutoPinEdgeToSuperviewEdge(ALEdge.Bottom, 20f);
                this.didSetupConstraints = true;
            }

            base.UpdateViewConstraints();
        }
    }
}