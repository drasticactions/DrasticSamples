using System;
using System.Reflection.Emit;

namespace UIKitPlayground
{
    public class TestCellTableViewController : UITableViewController
    {
        public TestCellTableViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            string[] tableItems = new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
            this.TableView.Source = new TableSource(tableItems);
        }

        public class TableSource : UITableViewSource
        {

            string[] TableItems;
            string CellIdentifier = "TableCell";

            public TableSource(string[] items)
            {
                TableItems = items;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return TableItems.Length;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
                string item = TableItems[indexPath.Row];

                //if there are no cells to reuse, create a new one
                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
                }

                cell.TextLabel.Text = item;

                return cell;
            }
        }
    }
}

