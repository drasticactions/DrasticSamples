using System;
using System.Linq;
using Drastic.PureLayout;

namespace PureLayoutSample
{
	public class ArraysViewController : UIViewController
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
                // Apply a fixed height of 50 pt to two views at once, and a fixed height of 70 pt to another two views
                NSArray.FromObjects(new[] { this.redView, this.yellowView }).AutoSetViewsDimension(ALDimension.Height, 50f);
                NSArray.FromObjects(new[] { this.blueView, this.greenView }).AutoSetViewsDimension(ALDimension.Height, 70f);
                var views = NSArray.FromObjects(new[] { this.redView, this.blueView, this.yellowView, this.greenView });
                views.AutoMatchViewsDimension(ALDimension.Width);

                this.redView.AutoPinEdgeToSuperviewMargin(ALEdge.Top, 20f);

                ((UIView)views.First()).AutoPinEdgeToSuperviewEdge(ALEdge.Left);
                UIView? previousView = null;
                foreach (var testView in views)
                {
                    var view = (UIView)testView;
                    if (view == previousView)
                    {
                        view.AutoPinEdge(ALEdge.Left, ALEdge.Right, previousView);
                        view.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, previousView);
                    }

                    previousView = view;
                }

                ((UIView)views.Last()).AutoPinEdgeToSuperviewEdge(ALEdge.Right);

                this.didSetupConstraints = true;
            }

            base.UpdateViewConstraints();
        }
    }
}