using System;
using Drastic.PureLayout;

namespace UIKitPlayground.tvOS
{
    public class PlaygroundViewController : UIViewController, IUIGestureRecognizerDelegate
    {
        private bool didSetupConstraints;

        private UIView blueView = new UIView() { BackgroundColor = UIColor.Blue };
        private UIView redView = new UIView() { BackgroundColor = UIColor.Red };
        private UIView yellowView = new UIImageView() { Image = UIImage.FromBundle("DotNetBot"), BackgroundColor = UIColor.Yellow };
        private UIView greenView = new UIView() { BackgroundColor = UIColor.Green };
        private UIProgressView progessView = new UIProgressView();

        private UILabel textLabel1 = new UILabel()
        {
            Text = "Test",
            BackgroundColor = UIColor.DarkGray,
            TextAlignment = UITextAlignment.Center,
        };

        private UILabel textLabel2 = new UILabel()
        {
            Text = "Test Two",
            BackgroundColor = UIColor.DarkGray,
            TextAlignment = UITextAlignment.Center,
        };

        private class CustomGestures : UIGestureRecognizer
        {
            public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
            {
                base.PressesBegan(presses, evt);
            }

            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);
            }
        }

        public PlaygroundViewController()
        {
            var foo = new CustomGestures();
            this.View!.AddGestureRecognizer(foo);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var buttonStyle = UIButtonConfiguration.PlainButtonConfiguration;
            buttonStyle.CornerStyle = UIButtonConfigurationCornerStyle.Capsule;
            buttonStyle.ButtonSize = UIButtonConfigurationSize.Mini;

            this.View!.AddSubview(this.blueView);
            this.blueView.AddSubview(this.yellowView);
            this.blueView.AddSubview(this.greenView);
            this.View!.AddSubview(this.redView);

            this.redView.AddSubview(this.progessView);

            this.greenView.AddSubview(this.textLabel1);
            this.greenView.AddSubview(this.textLabel2);

            this.View!.SetNeedsUpdateConstraints();
        }

        public override void UpdateViewConstraints()
        {
            if (!this.didSetupConstraints)
            {
                // blueView = main view
                // redView = player controls
                this.blueView.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Bottom);
                this.redView.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Top);
                this.blueView.AutoPinEdge(ALEdge.Bottom, ALEdge.Top, this.redView);
                this.redView.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.blueView);
                this.redView.AutoSetDimension(ALDimension.Height, 175f);

                // yellowView = Album art.
                this.yellowView.AutoAlignAxisToSuperviewAxis(ALAxis.Vertical);
                this.yellowView.AutoAlignAxisToSuperviewAxis(ALAxis.Horizontal);
                this.yellowView.AutoSetDimensionsToSize(new CGSize(500f, 500f));

                // greenView = Artist/Track
                this.greenView.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.yellowView, 30f);
                this.greenView.AutoPinEdge(ALEdge.Bottom, ALEdge.Top, this.redView, -30f);
                this.greenView.AutoAlignAxisToSuperviewAxis(ALAxis.Vertical);
                this.greenView.AutoMatchDimensionWithMultiplier(ALDimension.Width, ALDimension.Width, this.yellowView, 1.5f);

                // Artist and Track Info.
                this.textLabel1.AutoPinEdgesToSuperviewEdgesExcludingEdge(new UIEdgeInsets(top: 10f, bottom: 0f, left: 0f, right: 0f), ALEdge.Bottom);
                this.textLabel2.AutoPinEdgesToSuperviewEdgesExcludingEdge(new UIEdgeInsets(top: 0f, bottom: 10f, left: 0f, right: 0f), ALEdge.Top);

                this.progessView.AutoPinEdge(ALEdge.Left, ALEdge.Left, this.redView, 5f);
                this.progessView.AutoPinEdge(ALEdge.Right, ALEdge.Right, this.redView, -5f);
                this.progessView.AutoAlignAxisToSuperviewAxis(ALAxis.Horizontal);
                this.progessView.AutoSetDimension(ALDimension.Height, 15f);

                this.didSetupConstraints = true;
            }

            base.UpdateViewConstraints();
        }
    }
}