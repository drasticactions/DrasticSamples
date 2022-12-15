using System;
using Drastic.Nuke;
using Drastic.PureLayout;

namespace UIKitPlayground
{
    public class RssFeedTableViewController : UIViewController
    {
        private RssItem[] items;

        private RssTableView view;

        public RssFeedTableViewController()
        {
            var fakeItems = new FakeRssItem();

            var list = new List<RssItem>();
            for (var i = 0; i <= 100; i++)
            {
                list.Add(fakeItems.Generate());
            }

            this.items = list.OrderByDescending(n => n.ReleaseDate).ToArray();

            this.view = new RssTableView(this.items);
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

        public class TableSource : UITableViewSource
        {
            RssItem[] TableItems;
            string CellIdentifier = TrackViewCell.ReuseIdentifier;

            public TableSource(RssItem[] items)
            {
                TableItems = items;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return TableItems.Length;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                RssItemViewCell cell = tableView.DequeueReusableCell(CellIdentifier) as RssItemViewCell;
                RssItem item = TableItems[indexPath.Row];

                if (cell == null)
                {
                    cell = new RssItemViewCell(item);
                }
                else
                {
                    cell.SetupCell(item);
                }

                return cell;
            }
        }

        public class RssTableView : UITableView
        {
            private RssItem[] items;

            public RssTableView(RssItem[] items)
            {
                this.items = items;
                this.Source = new TableSource(this.items);
            }
        }

        public class RssItemViewCell : UITableViewCell
        {
            public static string ReuseIdentifier => "rssItemCell";

            private ImagePipeline pipeline;
            private RssItem item;

            private UIView hasSeenHolder = new UIView() { BackgroundColor = UIColor.Red };
            private UIView iconHolder = new UIView() { BackgroundColor = UIColor.Blue };
            private UIView feedHolder = new UIView() { BackgroundColor = UIColor.Orange };

            private UIView content = new UIView() { BackgroundColor = UIColor.Green };
            private UIView footer = new UIView() { BackgroundColor = UIColor.Purple };

            private UIImageView icon = new UIImageView();
            private UILabel title = new UILabel();
            private UILabel description = new UILabel();
            private UILabel releaseDate = new UILabel();

            public RssItemViewCell(RssItem info, UITableViewCellStyle style = UITableViewCellStyle.Default)
          : base(style, ReuseIdentifier)
            {
                this.icon.Layer.CornerRadius = 5;
                this.icon.Layer.MasksToBounds = true;
                this.pipeline = ImagePipeline.Shared;
                this.SetupUI();
                this.SetupLayout();
                this.SetupCell(info);
            }

            public void SetupUI()
            {
                this.ContentView.AddSubview(this.content);
                this.ContentView.AddSubview(this.footer);

                this.content.AddSubview(this.hasSeenHolder);
                this.content.AddSubview(this.iconHolder);
                this.content.AddSubview(this.feedHolder);

                this.iconHolder.AddSubview(this.icon);
            }

            public void SetupLayout()
            {
                this.content.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Bottom);
                this.content.AutoPinEdge(ALEdge.Bottom, ALEdge.Top, this.footer);
                this.content.AutoSetDimension(ALDimension.Height, 60f);

                this.footer.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Top);
                this.footer.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.content);
                this.footer.AutoSetDimension(ALDimension.Height, 1f);

                this.hasSeenHolder.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Right);
                this.hasSeenHolder.AutoPinEdge(ALEdge.Right, ALEdge.Left, this.iconHolder);
                this.hasSeenHolder.AutoSetDimension(ALDimension.Width, 25f);

                this.iconHolder.AutoPinEdge(ALEdge.Left, ALEdge.Right, this.hasSeenHolder);
                this.iconHolder.AutoPinEdge(ALEdge.Right, ALEdge.Left, this.feedHolder);
                this.iconHolder.AutoPinEdge(ALEdge.Top, ALEdge.Top, this.content);
                this.iconHolder.AutoPinEdge(ALEdge.Bottom, ALEdge.Bottom, this.content);
                this.iconHolder.AutoSetDimension(ALDimension.Width, 40f);

                this.icon.AutoCenterInSuperview();
                this.icon.AutoSetDimensionsToSize(new CGSize(32, 32));

                this.feedHolder.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Left);
                this.feedHolder.AutoPinEdge(ALEdge.Left, ALEdge.Right, this.iconHolder);
            }

            public void SetupCell(RssItem item)
            {
                this.item = item;

                this.pipeline.LoadImageWithUrl(new NSUrl(item.ImageUrl)!, UIImage.FromBundle("DotNetBot"), null, null, null, this.icon);
            }
        }
    }

    public class FakeRssItem : Bogus.Faker<RssItem>
    {
        public FakeRssItem()
        {
            RuleFor(o => o.Title, f => f.Lorem.Sentence());
            RuleFor(o => o.Author, f => f.Name.FullName());
            RuleFor(o => o.Description, f => f.Lorem.Paragraph());
            RuleFor(o => o.ImageUrl, f => f.Image.LoremFlickrUrl());
            RuleFor(o => o.ReleaseDate, f => f.Date.Past(1));
        }
    }

    public class RssItem
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool HasSeen { get; set; } = false;

        public string ImageUrl { get; set; }
    }
}