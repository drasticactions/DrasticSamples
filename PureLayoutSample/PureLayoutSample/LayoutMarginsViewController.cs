using System;
using Drastic.PureLayout;

namespace PureLayoutSample
{
    public class LayoutMarginsViewController : UIViewController
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
            this.blueView.AddSubview(this.redView);
            this.redView.AddSubview(this.yellowView);
            this.yellowView.AddSubview(this.greenView);

            this.View!.SetNeedsUpdateConstraints();
        }

        public override void UpdateViewConstraints()
        {
            if (!this.didSetupConstraints)
            {
                // Before layout margins were introduced, this is a typical way of giving a subview some padding from its superview's edges
                this.blueView.AutoPinEdgesToSuperviewEdges(new UIEdgeInsets(top: 0, left: 10.0f, bottom: 10.0f, right: 10.0f));

                // Set the layoutMargins of the blueView, which will have an effect on subviews of the blueView that attach to
                // the blueView's margin attributes -- in this case, the redView
                this.blueView.LayoutMargins = new UIEdgeInsets(top: 10.0f, left: 20.0f, bottom: 80.0f, right: 20.0f);
                this.redView.AutoPinEdgesToSuperviewMargins();

                // Let the redView inherit the values we just set for the blueView's layoutMargins by setting the below property to YES.
                // Then, pin the yellowView's edges to the redView's margins, giving the yellowView the same insets from its superview as the redView.
                this.redView.PreservesSuperviewLayoutMargins = true;

                this.yellowView.AutoPinEdgeToSuperviewMargin(ALEdge.Left);
                this.yellowView.AutoPinEdgeToSuperviewMargin(ALEdge.Right);

                // By aligning the yellowView to its superview's horizontal margin axis, the yellowView will be positioned with its horizontal axis
                // in the middle of the redView's top and bottom margins (causing it to be slightly closer to the top of the redView, since the
                // redView has a much larger bottom margin than top margin).
                this.yellowView.AutoAlignAxisToSuperviewAxis(ALAxis.Horizontal);
                this.yellowView.AutoMatchDimensionWithMultiplier(ALDimension.Height, ALDimension.Height, this.redView, 0.5f);

                // Since yellowView.preservesSuperviewLayoutMargins is NO by default, it will not preserve (inherit) its superview's margins,
                // and instead will just have the default margins of: {8.0, 8.0, 8.0, 8.0} which will apply to its subviews (greenView)
                this.greenView.AutoPinEdgesToSuperviewMarginsExcludingEdge(ALEdge.Bottom);
                this.greenView.AutoSetDimension(ALDimension.Height, 50f);

                this.didSetupConstraints = true;
            }

            base.UpdateViewConstraints();
        }
    }
}