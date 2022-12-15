using System;
using Drastic.PureLayout;
using Humanizer;
using ObjCRuntime;
using UIKit;

namespace UIKitPlayground
{
    public class PodcastTableViewController : UIViewController
    {
        PodcastTableView view;

        public PodcastTableViewController()
        {
            this.view = new PodcastTableView();
            this.ViewRespectsSystemMinimumLayoutMargins = false;
            this.view.PreservesSuperviewLayoutMargins = true;
            this.view.DirectionalLayoutMargins = NSDirectionalEdgeInsets.Zero;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View!.AddSubview(this.view);
            this.view.AutoPinEdgesToSuperviewEdges();
        }
    }

    public class PodcastHeaderView : UIView
    {
        private UIView podcastArtHolder = new UIView() { BackgroundColor = UIColor.Yellow };
        private UIView podcastInfoHolder = new UIView() { BackgroundColor = UIColor.Brown };

        public PodcastHeaderView()
        {
            this.SetupUI();
            this.SetupLayout();
        }

        private void SetupUI()
        {
            this.AddSubview(this.podcastInfoHolder);
            this.AddSubview(this.podcastArtHolder);
        }

        private void SetupLayout()
        {
            this.AutoSetDimension(ALDimension.Height, 250f);
            this.podcastInfoHolder.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Right);
            this.podcastArtHolder.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Left);

            this.podcastInfoHolder.AutoPinEdge(ALEdge.Left, ALEdge.Right, this.podcastArtHolder);
            this.podcastArtHolder.AutoPinEdge(ALEdge.Right, ALEdge.Left, this.podcastInfoHolder);
        }
    }

    public class PodcastTableView : UITableView
    {
        PodcastTrackInfo[] TableItems;
        PodcastHeaderView headerView;

        public PodcastTableView()
        {
            var list = new List<PodcastTrackInfo>();
            var generator = new FakePodcastTrackInfo();
            for (var i = 0; i <= 100; i++)
            {
                list.Add(generator.Generate());
            }

            TableItems = list.OrderByDescending(n => n.ReleaseDate).ToArray();
            this.Source = new TableSource(TableItems);
            this.AllowsSelection = true;
            
        }

        public PodcastTableView(NSCoder coder) : base(coder)
        {
        }

        public PodcastTableView(CGRect frame) : base(frame)
        {
        }

        public PodcastTableView(CGRect frame, UITableViewStyle style) : base(frame, style)
        {
        }

        protected PodcastTableView(NSObjectFlag t) : base(t)
        {
        }

        protected internal PodcastTableView(NativeHandle handle) : base(handle)
        {
        }

        public class TableSource : UITableViewSource
        {
            PodcastTrackInfo[] TableItems;
            string CellIdentifier = TrackViewCell.ReuseIdentifier;

            public TableSource(PodcastTrackInfo[] items)
            {
                TableItems = items;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return TableItems.Length;
            }

            public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
            {
                base.WillDisplayHeaderView(tableView, headerView, section);
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                PodcastItemViewCell cell = tableView.DequeueReusableCell(CellIdentifier) as PodcastItemViewCell;
                PodcastTrackInfo item = TableItems[indexPath.Row];

                if (cell == null)
                {
                    cell = new PodcastItemViewCell(item);
                }
                else
                {
                    cell.SetupCell(item);
                }

                return cell;
            }
        }

        public override UITableViewHeaderFooterView? GetHeaderView(nint section)
        {
            return base.GetHeaderView(section);
        }
    }

    public class PodcastItemViewCell : UITableViewCell
    {
        private UILabel releaseDate = new UILabel();
        private UILabel title = new UILabel();
        private UIImageView nowPlayingIndicator = new UIImageView();
        private UILabel description = new UILabel();
        private UILabel duration = new UILabel();
        private PodcastTrackInfo? trackInfo;

        private UIView releaseDateHolder = new UIView() { BackgroundColor = UIColor.Red };
        private UIView extraInfoHolder = new UIView() { BackgroundColor = UIColor.Red };

        private UIView podcastInfo = new UIView() { BackgroundColor = UIColor.Blue };
        private UIView durationHolder = new UIView() { BackgroundColor = UIColor.Green };
        private UIView optionsHolder = new UIView() { };

        private UIView mainContent = new UIView { };
#if !TVOS
        private UIView footer = new UIView() { BackgroundColor = UIColor.SystemFill };
#else
        private UIView footer = new UIView() { BackgroundColor = UIColor.Clear };
#endif

        public static string ReuseIdentifier => "podcastItemCell";

        public PodcastItemViewCell(PodcastTrackInfo info, UITableViewCellStyle style = UITableViewCellStyle.Default)
            : base(style, ReuseIdentifier)
        {
            this.SetupUI();
            this.SetupLayout();
            this.SetupCell(info);

#if !TVOS
            var poop = UIConfigurationColorTransformer.PreferredTint;
            var real = poop(UIColor.Clear);
            var test = new UIView() { BackgroundColor = real };
            test.Layer.CornerRadius = 5f;
            test.Layer.MasksToBounds = true;
            this.SelectedBackgroundView = test;
#endif
        }

        public void SetupCell(PodcastTrackInfo info)
        {
            this.trackInfo = info;
            this.title.Text = info.Title;
            this.description.Text = info.Description;
            this.duration.Text = info.Duration.Humanize();
            this.releaseDate.Text = info.ReleaseDate.Humanize();
        }

        private void SetupUI()
        {
            this.ContentView.AddSubview(this.mainContent);
            this.ContentView.AddSubview(this.footer);

            this.mainContent.AddSubview(this.podcastInfo);
            this.mainContent.AddSubview(this.durationHolder);
            this.mainContent.AddSubview(this.optionsHolder);

            this.podcastInfo.AddSubview(this.releaseDateHolder);
            this.podcastInfo.AddSubview(this.title);
            this.podcastInfo.AddSubview(this.description);
            this.podcastInfo.AddSubview(this.extraInfoHolder);

            this.durationHolder.AddSubview(this.duration);

            this.releaseDateHolder.AddSubview(this.releaseDate);
            this.releaseDate.TextAlignment = UITextAlignment.Left;
            this.title.TextAlignment = UITextAlignment.Left;
            this.description.TextAlignment = UITextAlignment.Left;

            this.duration.Lines = 1;
            this.releaseDate.Lines = 1;
            this.title.Lines = 2;
            this.description.Lines = 3;

            this.releaseDate.Font = UIFont.PreferredCaption1;
            this.title.Font = UIFont.PreferredTitle2;
            this.description.Font = UIFont.PreferredCaption1;
        }

        private void SetupLayout()
        {
            this.mainContent.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Bottom);
            this.footer.AutoPinEdgesToSuperviewEdgesExcludingEdge(new UIEdgeInsets(top: 0f, bottom: 0f, left: 25f, right: 25f), ALEdge.Top);

            this.mainContent.AutoPinEdge(ALEdge.Bottom, ALEdge.Top, this.footer);
            this.footer.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.mainContent);
            this.footer.AutoSetDimension(ALDimension.Height, 1f);

            // MARK: Holders.
            this.podcastInfo.AutoPinEdgesToSuperviewEdgesExcludingEdge(new UIEdgeInsets(top: 25f, bottom: 25f, left: 25f, right: 0f), ALEdge.Right);
            this.optionsHolder.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Left);

            this.durationHolder.AutoPinEdge(ALEdge.Top, ALEdge.Top, this.ContentView);
            this.durationHolder.AutoPinEdge(ALEdge.Bottom, ALEdge.Bottom, this.ContentView);
            this.durationHolder.AutoPinEdge(ALEdge.Left, ALEdge.Right, this.podcastInfo);
            this.durationHolder.AutoPinEdge(ALEdge.Right, ALEdge.Left, this.optionsHolder);
            this.durationHolder.AutoSetDimension(ALDimension.Width, 100f);

            this.podcastInfo.AutoPinEdge(ALEdge.Right, ALEdge.Left, this.durationHolder, -50f);
            this.optionsHolder.AutoPinEdge(ALEdge.Left, ALEdge.Right, this.durationHolder);

            this.optionsHolder.AutoSetDimension(ALDimension.Width, 100f);

            // MARK: Podcast Info
            this.releaseDateHolder.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Bottom);

            // FIXME: height.
            this.releaseDate.AutoPinEdgesToSuperviewEdges();

            this.title.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.releaseDateHolder);
            this.title.AutoPinEdge(ALEdge.Left, ALEdge.Left, this.podcastInfo);
            this.title.AutoPinEdge(ALEdge.Right, ALEdge.Right, this.podcastInfo);
            this.title.AutoPinEdge(ALEdge.Bottom, ALEdge.Top, this.description);

            this.description.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.title);
            this.description.AutoPinEdge(ALEdge.Left, ALEdge.Left, this.podcastInfo);
            this.description.AutoPinEdge(ALEdge.Right, ALEdge.Right, this.podcastInfo);
            this.description.AutoPinEdge(ALEdge.Bottom, ALEdge.Top, this.extraInfoHolder);

            this.extraInfoHolder.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Top);
            this.extraInfoHolder.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.description);

            this.duration.AutoCenterInSuperview();
        }
    }

    public class FakePodcastTrackInfo : Bogus.Faker<PodcastTrackInfo>
    {
        public FakePodcastTrackInfo()
        {
            RuleFor(o => o.Title, f => f.Lorem.Sentence());
            RuleFor(o => o.Description, f => f.Lorem.Paragraph());
            RuleFor(o => o.Duration, f => f.Date.Timespan(new TimeSpan(1, 5, 0)));
            RuleFor(o => o.ReleaseDate, f => f.Date.Past(1));
        }
    }

    public class PodcastTrackInfo
    {
        public string Title { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Description { get; set; }

        public bool IsDownloaded { get; set; }

        public bool IsBookmarked { get; set; }
    }
}