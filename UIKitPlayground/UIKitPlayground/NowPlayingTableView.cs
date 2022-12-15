using System;

namespace UIKitPlayground
{
    public class NowPlayingTableViewController : UITableViewController
    {
        public NowPlayingTableViewController()
        {
            this.TableView = new NowPlayingTableView();
        }
    }

    public class NowPlayingTableView : UITableView
    {
        TrackInfo[] TableItems;

        public NowPlayingTableView()
        {
            var list = new List<TrackInfo>();

            for (var i = 0; i <= 100; i++)
            {
                list.Add(new TrackInfo());
            }

            TableItems = list.ToArray();
            this.Source = new TableSource(TableItems);
        }

        public class TableSource : UITableViewSource
        {

            TrackInfo[] TableItems;
            string CellIdentifier = TrackViewCell.ReuseIdentifier;

            public TableSource(TrackInfo[] items)
            {
                TableItems = items;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return TableItems.Length;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                TrackViewCell cell = tableView.DequeueReusableCell(CellIdentifier) as TrackViewCell;
                TrackInfo item = TableItems[indexPath.Row];

                if (cell == null)
                {
                    cell = new TrackViewCell(item);
                }
                else
                {
                    cell.SetupCell(item);
                }

                return cell;
            }
        }
    }

    public class TrackViewCell : UITableViewCell
    {
        private UILabel title = new UILabel();
        private UIImageView nowPlayingIndicator = new UIImageView();
        private UILabel artist = new UILabel();
        private UILabel duration = new UILabel();
        private UIStackView stackView = new UIStackView();
        private TrackInfo? trackInfo;

        public static string ReuseIdentifier => "trackCell";

        public TrackViewCell(TrackInfo info, UITableViewCellStyle style = UITableViewCellStyle.Default)
            : base(style, "trackCell")
        {
            this.SetupUI();
            this.SetupLayout();
            this.SetupCell(info);
        }

        public void SetupCell(TrackInfo info)
        {
            this.trackInfo = info;
        }

        private void SetupUI()
        {
            ClipsToBounds = true;
            Frame = new CGRect(x: 0, y: 320.33333396911621, width: 812, height: 48.5);
            PreservesSuperviewLayoutMargins = true;
#if !TVOS
            SeparatorInset = new UIEdgeInsets(top: 0, left: 8, bottom: 0, right: 8);
#endif

            ContentView.AddSubview(stackView);

            ContentView.ClipsToBounds = true;
            ContentView.ContentMode = UIViewContentMode.Center;
            ContentView.InsetsLayoutMarginsFromSafeArea = false;
#if !TVOS
            ContentView.MultipleTouchEnabled = true;
#endif
            ContentView.PreservesSuperviewLayoutMargins = true;

            stackView.AddArrangedSubview(title);
            stackView.AddArrangedSubview(nowPlayingIndicator);
            stackView.AddArrangedSubview(artist);
            stackView.AddArrangedSubview(duration);

            stackView.Spacing = 16;
            stackView.TranslatesAutoresizingMaskIntoConstraints = false;

            duration.ContentMode = UIViewContentMode.Left;
            duration.Font = UIFont.SystemFontOfSize(17);
            duration.SetContentCompressionResistancePriority((float)(UILayoutPriority.Required), UILayoutConstraintAxis.Horizontal);
            duration.SetContentHuggingPriority(251, UILayoutConstraintAxis.Vertical);
            duration.SetContentHuggingPriority(500, UILayoutConstraintAxis.Horizontal);
            duration.Text = "Duration";
            duration.TextAlignment = UITextAlignment.Natural;
            duration.TextColor = UIColor.SecondaryLabel;
            duration.TranslatesAutoresizingMaskIntoConstraints = false;

            artist.ContentMode = UIViewContentMode.Left;
            artist.Font = UIFont.SystemFontOfSize(17);
            artist.SetContentCompressionResistancePriority(500, UILayoutConstraintAxis.Horizontal);
            artist.SetContentHuggingPriority(251, UILayoutConstraintAxis.Horizontal);
            artist.Text = "Artist";
            artist.TextAlignment = UITextAlignment.Natural;
            artist.TextColor = UIColor.SecondaryLabel;
            artist.TranslatesAutoresizingMaskIntoConstraints = false;

            artist.Hidden = true;

            nowPlayingIndicator.ClipsToBounds = true;
            nowPlayingIndicator.ContentMode = UIViewContentMode.ScaleAspectFit;
            nowPlayingIndicator.Image = UIImage.GetSystemImage("waveform");
            nowPlayingIndicator.SetContentHuggingPriority(251, UILayoutConstraintAxis.Horizontal);
            nowPlayingIndicator.SetContentHuggingPriority(251, UILayoutConstraintAxis.Vertical);
            nowPlayingIndicator.TranslatesAutoresizingMaskIntoConstraints = false;

            nowPlayingIndicator.Hidden = true;

            title.ContentMode = UIViewContentMode.Left;
            title.Font = UIFont.SystemFontOfSize(17, UIFontWeight.Medium);
            title.SetContentCompressionResistancePriority(900, UILayoutConstraintAxis.Horizontal);
            title.SetContentHuggingPriority(249, UILayoutConstraintAxis.Horizontal);
            title.SetContentHuggingPriority(251, UILayoutConstraintAxis.Vertical);
            title.Text = "Track Title looooooooooooooong";
            title.TranslatesAutoresizingMaskIntoConstraints = false;
        }

        public void SetupLayout()
        {
            BottomAnchor.ConstraintEqualTo(stackView.BottomAnchor).Active = true;
            stackView.LeadingAnchor.ConstraintEqualTo(this.LeadingAnchor, 16).Active = true;
            TrailingAnchor.ConstraintEqualTo(stackView.TrailingAnchor, 16).Active = true;
            stackView.TopAnchor.ConstraintEqualTo(this.TopAnchor).Active = true;
            stackView.HeightAnchor.ConstraintEqualTo(48).Active = true;
            duration.WidthAnchor.ConstraintEqualTo(72).Active = true;
            artist.WidthAnchor.ConstraintEqualTo(256).Active = true;
            nowPlayingIndicator.WidthAnchor.ConstraintEqualTo(36).Active = true;
            title.WidthAnchor.ConstraintGreaterThanOrEqualTo(216).Active = true;
        }
    }

    public class TrackInfo
    {
        public string Title { get; set; }

        public string Duration { get; set; }

        public string Artist { get; set; }
    }
}