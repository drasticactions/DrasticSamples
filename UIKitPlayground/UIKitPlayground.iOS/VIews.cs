//using System;
//using UIKit;
//using CoreGraphics;
//using CoreAnimation;
//using System.Runtime.InteropServices;

//namespace UIKitPlayground.iOS
//{
//    public class ProgressButton : UIButton
//    {
//        public UIColor MainTintColor
//        {
//            get => MainTintColor;
//            set
//            {
//                MainTintColor = value;
//                CircleLayer.StrokeColor = MainTintColor;
//                CubeView.BackgroundColor = MainTintColor;
//            }
//        }

//        public float Progress
//        {
//            get => Progress;
//            set
//            {
//                Progress = value;
//                AnimateProgress(CircleLayer.CircleLayer.StrokeEnd, (float)Progress);
//            }
//        }

//        public WaitingView CircleLayer
//        {
//            get
//            {
//                var layer = new WaitingView();
//                layer.RotationStartingAngle = -(float)Math.PI / 2;
//                layer.RotationEndingAngle = layer.RotationStartingAngle + 2 * (float)Math.PI;
//                layer.ShouldSpin = false;
//                layer.IsClockwise = true;
//                return layer;
//            }
//        }

//        public UIView CubeView
//        {
//            get
//            {
//                var view = new UIView();
//                view.Layer.CornerRadius = 3;
//                return view;
//            }
//        }

//        public ProgressButton(CGRect frame) : base(frame)
//        {
//            CommonInit();
//        }

//        public ProgressButton(NSCoder coder) : base(coder)
//        {
//            CommonInit();
//        }

//        private void CommonInit()
//        {
//            BackgroundColor = UIColor.Clear;
//            CircleLayer.StrokeColor = MainTintColor;
//            AddSubview(CircleLayer);
//            CircleLayer.PinToSuperview();

//            CubeView.BackgroundColor = MainTintColor;
//            AddSubview(CubeView);
//            CubeView.TranslatesAutoresizingMaskIntoConstraints = false;
//            CubeView.HeightAnchor.ConstraintEqualTo(HeightAnchor, 0.3f).Active = true;
//            CubeView.WidthAnchor.ConstraintEqualTo(CubeView.HeightAnchor, 1).Active = true;
//            CubeView.CenterXAnchor.ConstraintEqualTo(CenterXAnchor).Active = true;
//            CubeView.CenterYAnchor.ConstraintEqualTo(CenterYAnchor).Active = true;
//        }

//        private void AnimateProgress(nfloat startValue = 0f, nfloat endValue = 0)
//        {
//            CircleLayer.CircleLayer.StrokeEnd = endValue;
//            var animation = new CABasicAnimation("strokeEnd");
//            animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);
//            animation.From = startValue;
//            animation.Duration = 0.3f;
//            CircleLayer.CircleLayer.AddAnimation(animation, null);
//        }
//    }

//    public class RoundButton : UIButton
//    {
//        public UIImage Image
//        {
//            get => Image;
//            set
//            {
//                Image = value;
//                CommonInit();
//            }
//        }

//        public RoundButton(CGRect frame) : base(frame)
//        {
//            CommonInit();
//        }

//        public RoundButton(NSCoder coder) : base(coder)
//        {
//            CommonInit();
//        }

//        private void CommonInit()
//        {
//            SetBackgroundImage(Image, UIControlState.Normal);
//        }

//        public override void LayoutSubviews()
//        {
//            base.LayoutSubviews();
//            Layer.CornerRadius = Frame.Height / 2;
//        }
//    }

//    public class WaitingView : UIView
//    {
//        // MARK: - Properties
//        public nfloat RotationStartingAngle = (nfloat)Math.PI;
//        public nfloat RotationEndingAngle = 3 * (nfloat)Math.PI / 2;
//        public bool ShouldSpin = true;
//        public bool IsClockwise = false;
//        public nfloat StrokeWidth = 3;
//        public UIColor StrokeColor = UIColor.SystemBlue;

//        public CAShapeLayer CircleLayer = new CAShapeLayer
//        {
//            FillColor = UIColor.Clear.CGColor,
//            LineCap = CAShapeLayer.CapRound
//        };

//        public WaitingView()
//        {
//            CommonInit();
//        }

//        // MARK: Initializers
//        public WaitingView(CGRect frame) : base(frame)
//        {
//            CommonInit();
//        }

//        public WaitingView(NSCoder coder) : base(coder)
//        {
//            CommonInit();
//        }

//        private void CommonInit()
//        {
//            BackgroundColor = UIColor.Clear;
//            CircleLayer.StrokeColor = StrokeColor.CGColor;
//            CircleLayer.LineWidth = StrokeWidth;
//            Layer.AddSublayer(CircleLayer);
//        }

//        public override void MovedToWindow()
//        {
//            base.MovedToWindow();

//            if (ShouldSpin)
//            {
//                StartSpinning();
//            }
//        }

//        public override void LayoutSubviews()
//        {
//            base.LayoutSubviews();

//            CGPoint center = new CGPoint(Frame.Width / 2, Frame.Height / 2);
//            var radius = Math.Min(Frame.Width / 2, Frame.Height / 2) - StrokeWidth / 2;
//            CircleLayer.Path = UIBezierPath.FromArc(center, (NFloat)radius, RotationStartingAngle, RotationEndingAngle, IsClockwise).CGPath;
//        }

//        public void StartSpinning()
//        {
//            string animationKey = "rotation";
//            Layer.RemoveAnimation(animationKey);
//            CABasicAnimation rotationAnimation = CABasicAnimation.FromKeyPath("transform.rotation");
//            rotationAnimation.From = NSNumber.FromFloat(0.0f);
//            rotationAnimation.To = NSNumber.FromFloat((float)Math.PI * 2);
//            rotationAnimation.Duration = 2;
//            rotationAnimation.RepeatCount = float.PositiveInfinity;
//            Layer.AddAnimation(rotationAnimation, animationKey);
//        }
//    }
//}