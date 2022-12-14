using System;
using AVFoundation;

namespace TvSample
{
    public class PlaybackViewController
    {
        AVPlayer player;

        NSTimer timer;
        Song currentSong;
        private UIImageView BackgroundImage { get; } = new UIImageView();
        private UICollectionViewFlowLayout SongsViewLayout { get; } = new UICollectionViewFlowLayout();
        //private SongCollectionView SongsView { get; } = new SongCollectionView(new CGRect(), SongsViewLayout);
        private UILabel SongNameLabel { get; } = new UILabel();
        private UIProgressView SongProgressView { get; } = new UIProgressView();
        private UIButton PlayButton { get; } = new UIButton(UIButtonType.RoundedRect);
        private UILabel PartyCodeLabel { get; } = new UILabel();
        private UILabel ArtistName { get; } = new UILabel();
        private UIButton NextButton { get; } = new UIButton(UIButtonType.RoundedRect);
        private UIButton PreviousButton { get; } = new UIButton(UIButtonType.RoundedRect);

        public PlaybackViewController()
        {
            // timer = NSTimer.CreateRepeatingScheduledTimer(1, OnTimer);
        }
    }
}

