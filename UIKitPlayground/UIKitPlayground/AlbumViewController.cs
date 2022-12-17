using System;
using Drastic.PureLayout;

namespace UIKitPlayground
{
    public class AlbumViewController : UIViewController
    {
        private AlbumHolderView albumHolderView = new AlbumHolderView();
        private TracksHolderView tracksHolderView = new TracksHolderView();

        public AlbumViewController()
        {
            this.SetupUI();
            this.SetupLayout();
        }

        private void SetupUI()
        {
            this.View!.AddSubview(this.albumHolderView);
            this.View!.AddSubview(this.tracksHolderView);
        }

        private void SetupLayout()
        {
            this.albumHolderView.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Bottom);
            this.albumHolderView.AutoPinEdge(ALEdge.Bottom, ALEdge.Top, this.tracksHolderView);

            this.tracksHolderView.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Top);
            this.tracksHolderView.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.albumHolderView);

            this.albumHolderView.AutoSetDimension(ALDimension.Height, 300f);
        }
    }

    public class AlbumHolderView : UIView
    {
        private AlbumArtHolder albumArtHolder = new AlbumArtHolder();
        private AlbumInfoHolder albumInfoHolder = new AlbumInfoHolder();

        public AlbumHolderView()
        {
            this.BackgroundColor = UIColor.Red;

            this.SetupUI();
            this.SetupLayout();
        }

        public void SetupUI()
        {
            this.AddSubview(this.albumArtHolder);
            this.AddSubview(this.albumInfoHolder);
        }

        public void SetupLayout()
        {
            this.albumArtHolder.AutoPinEdgesToSuperviewEdgesExcludingEdge(new UIEdgeInsets(0, 0, 0, 0), ALEdge.Right);
            this.albumArtHolder.AutoPinEdge(ALEdge.Right, ALEdge.Left, this.albumInfoHolder, -65f);
            this.albumArtHolder.AutoSetDimension(ALDimension.Width, 225f);

            this.albumInfoHolder.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Left);
            this.albumInfoHolder.AutoPinEdge(ALEdge.Left, ALEdge.Right, this.albumArtHolder, -65f);
        }

        public class AlbumArtHolder : UIView
        {
            private UIImageView albumArt = new UIImageView() { BackgroundColor = UIColor.Brown };

            public AlbumArtHolder()
            {
                this.BackgroundColor = UIColor.Green;
                this.albumArt.Image = UIImage.FromBundle("DotNetBot");
                this.SetupUI();
                this.SetupLayout();
            }

            public void SetupUI()
            {
                this.AddSubview(this.albumArt);
            }

            public void SetupLayout()
            {
                this.albumArt.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Top);
                this.albumArt.AutoSetDimensionsToSize(new CGSize(225f, 225f));
            }
        }

        public class AlbumInfoHolder : UIView
        {
            private InfoHolder infoHolder = new InfoHolder();
            private ButtonHolder buttonHolder = new ButtonHolder();

            public AlbumInfoHolder()
            {
                this.BackgroundColor = UIColor.Orange;

                this.SetupUI();
                this.SetupLayout();
            }

            public void SetupUI()
            {
                this.AddSubview(this.infoHolder);
                this.AddSubview(this.buttonHolder);
            }

            public void SetupLayout()
            {
                this.buttonHolder.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Top);
                this.buttonHolder.AutoSetDimension(ALDimension.Height, 100f);

                this.infoHolder.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Bottom);
                this.infoHolder.AutoPinEdge(ALEdge.Bottom, ALEdge.Top, this.buttonHolder);
            }

            public class InfoHolder : UIView
            {
                private UILabel albumName = new UILabel() { Text = "The Bible 2", Font = UIFont.PreferredHeadline.WithSize(25), TextAlignment = UITextAlignment.Left };
                private UILabel artistName = new UILabel() { Text = "AJJ", Font = UIFont.PreferredSubheadline.WithSize(25), TextColor = UIColorExtensions.GetSystemTint(), TextAlignment = UITextAlignment.Left };
                private UILabel info = new UILabel() { Text = "PUNK ・ 2016", Font = UIFont.PreferredFootnote.WithSize(12), TextAlignment = UITextAlignment.Left };

                public InfoHolder()
                {
                    this.BackgroundColor = UIColor.Black;
                    this.SetupUI();
                    this.SetupLayout();
                }

                public void SetupUI()
                {
                    this.AddSubview(this.albumName);
                    this.AddSubview(this.artistName);
                    this.AddSubview(this.info);
                }

                public void SetupLayout()
                {
                    this.info.AutoPinEdge(ALEdge.Bottom, ALEdge.Bottom, this);
                    this.info.AutoPinEdge(ALEdge.Left, ALEdge.Left, this);

                    this.artistName.AutoPinEdge(ALEdge.Bottom, ALEdge.Top, this.info);
                    this.artistName.AutoPinEdge(ALEdge.Left, ALEdge.Left, this);

                    this.albumName.AutoPinEdge(ALEdge.Bottom, ALEdge.Top, this.artistName);
                    this.albumName.AutoPinEdge(ALEdge.Left, ALEdge.Left, this);
                }
            }

            public class ButtonHolder : UIView
            {
                public UIButton playAllButton = new UIButton(UIButtonType.RoundedRect) { };
                public UIButton shuffleButton = new UIButton(UIButtonType.RoundedRect) { };

                public ButtonHolder()
                {
                    this.playAllButton.SetTitle("再生", UIControlState.Normal);
                    this.playAllButton.SetImage(UIImage.GetSystemImage("play.fill"), UIControlState.Normal);

                    this.shuffleButton.SetTitle("シャッフル", UIControlState.Normal);
                    this.shuffleButton.SetImage(UIImage.GetSystemImage("shuffle"), UIControlState.Normal);

                    //#if !TVOS
                    //                    this.playAllButton.PreferredBehavioralStyle = UIBehavioralStyle.Pad;
                    //#endif
                    //this.playAllButton.ImageView.Image = UIImage.GetSystemImage("play.fill");
                    //this.playAllButton.TitleLabel.Text = "再生";

                    this.BackgroundColor = UIColor.DarkGray;
                    this.SetupUI();
                    this.SetupLayout();
                }

                public void SetupUI()
                {
                    this.AddSubview(this.playAllButton);
                    this.AddSubview(this.shuffleButton);
                }

                public void SetupLayout()
                {
                    //this.playAllButton.AutoCenterInSuperview();
                    this.playAllButton.AutoPinEdge(ALEdge.Left, ALEdge.Left, this);
                    this.playAllButton.AutoPinEdge(ALEdge.Bottom, ALEdge.Bottom, this);

                    this.shuffleButton.AutoPinEdge(ALEdge.Left, ALEdge.Right, this.playAllButton);
                    this.shuffleButton.AutoPinEdge(ALEdge.Bottom, ALEdge.Bottom, this);
                }
            }
        }
    }

    public class TracksHolderView : UIView
    {
        public TracksHolderView()
        {
            this.BackgroundColor = UIColor.Blue;
        }
    }
}