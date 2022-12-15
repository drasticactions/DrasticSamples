using System;
using Drastic.Nuke;
using ObjCRuntime;
using static System.Net.Mime.MediaTypeNames;

namespace UIKitPlayground
{
    public class ImageTableViewController : UITableViewController
    {
        public ImageTableViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var generator = new FakeCellDataItem();
            List<CellDataItem> items = new List<CellDataItem>();
            for (var i = 0; i <= 100; i++)
            {
                items.Add(generator.Generate());
            }

            this.TableView.Source = new TableSource(items.ToArray());
        }

        public class TableSource : UITableViewSource
        {

            ImagePipeline pipeline;
            CellDataItem[] TableItems;
            string CellIdentifier = "TableCell";

            public TableSource(CellDataItem[] items)
            {
                this.pipeline = ImagePipeline.Shared;
                TableItems = items;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return TableItems.Length;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                SizableImageCell cell = tableView.DequeueReusableCell(CellIdentifier) as SizableImageCell;
                CellDataItem item = TableItems[indexPath.Row];

                //if there are no cells to reuse, create a new one
                if (cell == null)
                {
                    cell = new SizableImageCell(45, UITableViewCellStyle.Subtitle, CellIdentifier);
                }

                cell.TextLabel.Text = item.Title;
                cell.DetailTextLabel.Lines = 2;
                cell.DetailTextLabel.Text = item.Description;
                this.pipeline.LoadImageWithUrl(new NSUrl(item.ImageUrl)!, UIImage.FromBundle("DotNetBot"), null, null, null, cell.ImageView);
                return cell;
            }
        }

        public class SizableImageCell : UITableViewCell
        {
            int desiredWidth;

            public SizableImageCell(int desiredWidth, UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
            {
                this.desiredWidth = desiredWidth;
            }

            public override void LayoutSubviews()
            {
                base.LayoutSubviews();

                var w = this.ImageView.Frame.Size.Width;
                var widthSub = w - desiredWidth;
                this.ImageView.Frame = new CGRect(this.ImageView.Frame.X, this.ImageView.Frame.Y, desiredWidth, this.ImageView.Frame.Size.Height);
                this.TextLabel.Frame = new CGRect(this.TextLabel.Frame.X - widthSub, this.TextLabel.Frame.Y, this.TextLabel.Frame.Size.Width + widthSub, this.TextLabel.Frame.Size.Height);
                this.DetailTextLabel.Frame = new CGRect(this.DetailTextLabel.Frame.X - widthSub, this.DetailTextLabel.Frame.Y, this.DetailTextLabel.Frame.Size.Width + widthSub, this.DetailTextLabel.Frame.Size.Height);
                this.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            }
        }

        public class FakeCellDataItem : Bogus.Faker<CellDataItem>
        {
            public FakeCellDataItem()
            {
                RuleFor(o => o.Title, f => f.Lorem.Sentence());
                RuleFor(o => o.Description, f => f.Lorem.Paragraph());
                RuleFor(o => o.ImageUrl, f => f.Image.LoremFlickrUrl());
            }

            static UIImage FromUrl(string uri)
            {
                using (var url = new NSUrl(uri))
                using (var data = NSData.FromUrl(url))
                    return UIImage.LoadFromData(data);
            }
        }

        public class CellDataItem
        {
            public string Title { get; set; }

            public string Description { get; set; }

            public string ImageUrl { get; set; }
        }
    }
}