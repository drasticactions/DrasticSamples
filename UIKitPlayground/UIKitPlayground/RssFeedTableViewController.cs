using System;
using Drastic.Nuke;
using Drastic.PureLayout;
using Humanizer;

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

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var item = this.TableItems[indexPath.Row];
                item.HasSeen = true;
                var cell = (RssItemViewCell)tableView.CellAt(indexPath)!;
                cell.UpdateHasSeen(item.HasSeen);
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

            private UIView hasSeenHolder = new UIView();
            private UIView iconHolder = new UIView();
            private UIView feedHolder = new UIView();

            private UIView content = new UIView();
            private UIView footer = new UIView();

            private UIImageView hasSeenIcon = new UIImageView();
            private UIImageView icon = new UIImageView();
            private UILabel title = new UILabel() { Lines = 2, Font = UIFont.PreferredHeadline, TextAlignment = UITextAlignment.Left };
            private UILabel description = new UILabel();
            private UILabel releaseDate = new UILabel() { Lines = 1, Font = UIFont.PreferredFootnote, TextAlignment = UITextAlignment.Right };
            private UILabel author = new UILabel() { Lines = 1, Font = UIFont.PreferredFootnote, TextAlignment = UITextAlignment.Left };
            private bool showIcon;

            public RssItemViewCell(RssItem info, bool showIcon = false, UITableViewCellStyle style = UITableViewCellStyle.Default)
          : base(style, ReuseIdentifier)
            {
#if TVOS
                this.footer.BackgroundColor = UIColor.Clear;
#else
                this.footer.BackgroundColor = UIColor.SystemFill;
#endif
                this.showIcon = showIcon;
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

                this.hasSeenHolder.AddSubview(this.hasSeenIcon);

                this.iconHolder.AddSubview(this.icon);

                this.feedHolder.AddSubview(this.title);
                this.feedHolder.AddSubview(this.author);
                this.feedHolder.AddSubview(this.releaseDate);

                this.hasSeenIcon.Image = UIImage.GetSystemImage("circle.fill");
            }

            public void SetupLayout()
            {
                this.content.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Bottom);
                this.content.AutoPinEdge(ALEdge.Bottom, ALEdge.Top, this.footer);
                this.content.AutoSetDimension(ALDimension.Height, 70f);

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

                this.hasSeenIcon.AutoCenterInSuperview();
                this.hasSeenIcon.AutoSetDimensionsToSize(new CGSize(12, 12));

                this.icon.AutoCenterInSuperview();
                this.icon.AutoSetDimensionsToSize(new CGSize(32, 32));

                this.feedHolder.AutoPinEdgesToSuperviewEdgesExcludingEdge(new UIEdgeInsets(top: 0f, left: 0f, bottom: 0f, right: 0f), ALEdge.Left);
                this.feedHolder.AutoPinEdge(ALEdge.Left, ALEdge.Right, this.iconHolder);

                this.title.AutoPinEdge(ALEdge.Top, ALEdge.Top, this.feedHolder, 5f);
                //this.title.AutoPinEdge(ALEdge.Bottom, ALEdge.Bottom, this.author, 5f);
                this.title.AutoPinEdge(ALEdge.Right, ALEdge.Right, this.feedHolder, -15f);
                this.title.AutoPinEdge(ALEdge.Left, ALEdge.Left, this.feedHolder, 15f);

                this.author.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.icon);
                this.author.AutoPinEdge(ALEdge.Left, ALEdge.Left, this.title);
                this.author.AutoPinEdge(ALEdge.Right, ALEdge.Left, this.releaseDate);

                this.releaseDate.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.icon);
                this.releaseDate.AutoPinEdge(ALEdge.Right, ALEdge.Right, this.title);
                this.releaseDate.AutoPinEdge(ALEdge.Left, ALEdge.Right, this.author);
            }

            public void SetupCell(RssItem item)
            {
                this.item = item;

                this.pipeline.LoadImageWithUrl(new NSUrl(item.ImageUrl)!, UIImage.FromBundle("DotNetBot"), null, null, null, this.icon);
                this.title.Text = item.Title;
                this.author.Text = item.Author;
                this.releaseDate.Text = item.ReleaseDate.Humanize();

                if (!this.showIcon)
                {
                    this.icon.Hidden = true;
                    this.iconHolder.AutoSetDimension(ALDimension.Width, 0f);
                }

                this.hasSeenIcon.Hidden = item.HasSeen;
            }

            public void UpdateHasSeen(bool hasSeen)
                => this.hasSeenIcon.SetHidden(hasSeen, true);
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