using System;
namespace TvSample
{
    public class SongCollectionViewCellNew : UICollectionViewCell
    {
        public UIImageView AlbumArtView;
        public UILabel SongName;

        public SongCollectionViewCellNew(IntPtr handle)
            : base(handle)
        {
            SetupUI();
        }

        public SongCollectionViewCellNew()
        {
            SetupUI();
        }

        private Song _song;

        public Song Song
        {
            get { return _song; }
            set
            {
                _song = value;
                if (_song == null) return;

                AlbumArtView.Image = FromUrl(value.AlbumArtMedium);
                SongName.Text = value.Title;
            }
        }

        private UIImage FromUrl(string uri)
        {
            using (var url = new NSUrl(uri))
            using (var data = NSData.FromUrl(url))
                return UIImage.LoadFromData(data);
        }

        private void SetupUI()
        {
            AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleBottomMargin;
            ContentMode = UIViewContentMode.Center;
            Frame = new CGRect(0, 265, 550, 550);

            ContentView.BackgroundColor = UIColor.FromWhiteAlpha(0, 0);
            ContentView.ClipsToBounds = true;
            ContentView.ContentMode = UIViewContentMode.Center;
            ContentView.Frame = new CGRect(0, 265, 550, 550);

            AlbumArtView = new UIImageView(new CGRect(35, 0, 480, 480));
            AlbumArtView.AdjustsImageWhenAncestorFocused = true;
            AddSubview(AlbumArtView);

            SongName = new UILabel(new CGRect(35, 490, 480, 50))
            {
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.White,
                Alpha = 0f
            };

            AddSubview(SongName);
        }
    }
}