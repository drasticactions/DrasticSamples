using System;
namespace UIKitPlayground
{
    public static class UIViewExtensions
    {
        public static void Blur(this UIView view)
        {
            if (!UIAccessibility.IsReduceTransparencyEnabled)
            {
                view.BackgroundColor = UIColor.Clear;

                var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Dark);
                var blurEffectView = new UIVisualEffectView(blurEffect);

                blurEffectView.Frame = view.Bounds;
                blurEffectView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

                view.InsertSubview(blurEffectView, 0);
            }
            else
            {
                view.BackgroundColor = UIColor.Black;
            }
        }

        public static void SetHidden(this UIView view, bool isHidden, bool animated, Action? completion = null)
        {
            if (isHidden && view.Hidden)
            {
                return;
            }

            if (animated)
            {
                var startAlpha = isHidden ? 1 : 0;
                var animatingAlpha = isHidden ? 0 : 1;
                view.Alpha = startAlpha;
                UIView.Animate(0.25, 0.1, UIViewAnimationOptions.CurveEaseOut, () => view.Alpha = animatingAlpha, () =>
                {
                    view.Hidden = isHidden;
                    view.Alpha = 1.0f;
                    completion?.Invoke();
                });
            }
            else
            {
                view.Hidden = isHidden;
                completion?.Invoke();
            }
        }
    }
}