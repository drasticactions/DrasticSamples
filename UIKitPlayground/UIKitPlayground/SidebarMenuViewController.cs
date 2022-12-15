#if !TVOS
using System;
using System.Xml.Linq;

namespace UIKitPlayground
{
    public class SidebarMenuViewController : UIViewController, IUICollectionViewDelegate
    {
        private Guid firstRowIdentifier;
        private UICollectionViewDiffableDataSource<NSString, SidebarItem>? dataSource;
        private UICollectionView? collectionView;

        public SidebarMenuViewController()
        {
            this.firstRowIdentifier = Guid.NewGuid();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.ConfigureCollectionView();
            this.ConfigureDataSource();
            this.ApplyInitialSnapshot();
        }

        [Export("collectionView:didSelectItemAtIndexPath:")]
        protected void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var sidebarItem = this.dataSource?.GetItemIdentifier(indexPath);
        }

        private NSDiffableDataSourceSectionSnapshot<SidebarItem> ConfigureLibrarySnapshot()
        {
            var snapshot = new NSDiffableDataSourceSectionSnapshot<SidebarItem>();
            var header = SidebarItem.Header("Library");
            var items = new SidebarItem[]
            {
                SidebarItem.Row("Hoge", null, UIImage.FromBundle("DotNetBot")!.ScaleToSize(new CGSize(32, 32)), this.firstRowIdentifier),
            };

            snapshot.AppendItems(new[] { header });
            snapshot.ExpandItems(new[] { header });
            snapshot.AppendItems(items, header);
            return snapshot;
        }

        private NSDiffableDataSourceSectionSnapshot<SidebarItem> ConfigureCollectionsSnapshot()
        {
            var snapshot = new NSDiffableDataSourceSectionSnapshot<SidebarItem>();
            var header = SidebarItem.Header("Title");
            var items = new List<SidebarItem>();
            items.Add(SidebarItem.Row("Foobar", null, UIImage.FromBundle("DotNetBot")!.ScaleToSize(new CGSize(32, 32))));
            snapshot.AppendItems(new[] { header });
            snapshot.ExpandItems(new[] { header });
            snapshot.AppendItems(items.ToArray(), header);

            return snapshot;
        }

        private void ApplyInitialSnapshot()
        {
            this.dataSource!.ApplySnapshot(this.ConfigureLibrarySnapshot(), new NSString(SidebarSection.Library.ToString()), false);
            this.dataSource!.ApplySnapshot(this.ConfigureCollectionsSnapshot(), new NSString(SidebarSection.Collections.ToString()), false);
        }

        private void ConfigureCollectionView()
        {
            this.collectionView = new UICollectionView(this.View!.Bounds, this.CreateLayout());
            this.collectionView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            this.collectionView.BackgroundColor = UIColor.SystemBackground;
            this.collectionView.Delegate = this;
            this.View.AddSubview(this.collectionView);
        }

        private UICollectionViewLayout CreateLayout()
        {
            return new UICollectionViewCompositionalLayout((sectionIndex, layoutEnvironment) =>
            {
                var configuration = new UICollectionLayoutListConfiguration(UICollectionLayoutListAppearance.Sidebar)!;
                configuration.ShowsSeparators = false;
                configuration.HeaderMode = UICollectionLayoutListHeaderMode.FirstItemInSection;
                return NSCollectionLayoutSection.GetSection(configuration, layoutEnvironment);
            });
        }

        private void ConfigureDataSource()
        {
            var headerRegistration = UICollectionViewCellRegistration.GetRegistration(typeof(UICollectionViewListCell),
                 new UICollectionViewCellRegistrationConfigurationHandler((cell, indexpath, item) =>
                 {
                     var sidebarItem = (SidebarItem)item;
                     var contentConfiguration = UIListContentConfiguration.SidebarHeaderConfiguration;
                     contentConfiguration.Text = sidebarItem.Title;
                     contentConfiguration.TextProperties.Font = UIFont.PreferredSubheadline;
                     contentConfiguration.TextProperties.Color = UIColor.SecondaryLabel;
                     cell.ContentConfiguration = contentConfiguration;
                     ((UICollectionViewListCell)cell).Accessories = new[] { new UICellAccessoryOutlineDisclosure() };
                 }));

            var expandableRowRegistration = UICollectionViewCellRegistration.GetRegistration(typeof(UICollectionViewListCell),
                 new UICollectionViewCellRegistrationConfigurationHandler((cell, indexpath, item) =>
                 {
                     var sidebarItem = (SidebarItem)item;
                     var contentConfiguration = UIListContentConfiguration.SidebarSubtitleCellConfiguration;
                     contentConfiguration.Text = sidebarItem.Title;
                     contentConfiguration.SecondaryText = sidebarItem.Subtitle;
                     contentConfiguration.Image = sidebarItem.Image;
                     contentConfiguration.TextProperties.Font = UIFont.PreferredSubheadline;
                     contentConfiguration.TextProperties.Color = UIColor.SecondaryLabel;

                     cell.ContentConfiguration = contentConfiguration;
                     ((UICollectionViewListCell)cell).Accessories = new[] { new UICellAccessoryOutlineDisclosure() };
                 }));

            var rowRegistration = UICollectionViewCellRegistration.GetRegistration(typeof(UICollectionViewListCell),
                 new UICollectionViewCellRegistrationConfigurationHandler((cell, indexpath, item) =>
                 {
                     var sidebarItem = (SidebarItem)item;
                     var contentConfiguration = UIListContentConfiguration.SidebarSubtitleCellConfiguration;
                     contentConfiguration.Text = sidebarItem.Title;
                     contentConfiguration.SecondaryText = sidebarItem.Subtitle;
                     contentConfiguration.Image = sidebarItem.Image;
                     contentConfiguration.TextProperties.Font = UIFont.PreferredSubheadline;
                     contentConfiguration.TextProperties.Color = UIColor.SecondaryLabel;

                     cell.ContentConfiguration = contentConfiguration;
                 }));

            this.dataSource = new UICollectionViewDiffableDataSource<NSString, SidebarItem>(collectionView,
                new UICollectionViewDiffableDataSourceCellProvider((collectionView, indexPath, item) =>
                {
                    var sidebarItem = (SidebarItem)item;

                    switch (sidebarItem.Type)
                    {
                        case SidebarItemType.Header:
                            return collectionView.DequeueConfiguredReusableCell(headerRegistration, indexPath, item);
                        case SidebarItemType.ExpandableRow:
                            return collectionView.DequeueConfiguredReusableCell(expandableRowRegistration, indexPath, item);
                        default:
                            return collectionView.DequeueConfiguredReusableCell(rowRegistration, indexPath, item);
                    }
                })
            );
        }

        private class SidebarItem : NSObject
        {
            public Guid Id { get; }

            public SidebarItemType Type { get; }

            public string Title { get; }

            public string? Subtitle { get; }

            public UIImage? Image { get; }

            public SidebarItem(Guid id, SidebarItemType type, string title, string? subtitle = default, UIImage? image = default)
            {
                this.Id = id;
                this.Type = type;
                this.Title = title;
                this.Subtitle = subtitle;
                this.Image = image;
            }

            public static SidebarItem Header(string title, Guid? id = default)
                => new SidebarItem(id ?? Guid.NewGuid(), SidebarItemType.Header, title);

            public static SidebarItem ExpandableRow(string title, string? subtitle = default, UIImage? image = default, Guid? id = default)
                => new SidebarItem(id ?? Guid.NewGuid(), SidebarItemType.ExpandableRow, title, subtitle, image);

            public static SidebarItem Row(string title, string? subtitle = default, UIImage? image = default, Guid? id = default)
                => new SidebarItem(id ?? Guid.NewGuid(), SidebarItemType.Row, title, subtitle, image);
        }

        private enum SidebarItemType
        {
            Header,
            ExpandableRow,
            Row,
        }

        private enum SidebarSection
        {
            Library,
            Collections,
        }
    }
}

public static class UIImageExtensions
{
    public static UIImage ScaleToSize(this UIImage self, CGSize newSize)
    {
        CGRect scaledImageRect = CGRect.Empty;

        double aspectWidth = newSize.Width / self.Size.Width;
        double aspectHeight = newSize.Height / self.Size.Height;
        double aspectRatio = Math.Min(aspectWidth, aspectHeight);

        scaledImageRect.Size = new CGSize(self.Size.Width * aspectRatio, self.Size.Height * aspectRatio);
        scaledImageRect.X = (newSize.Width - scaledImageRect.Size.Width) / 2.0f;
        scaledImageRect.Y = (newSize.Height - scaledImageRect.Size.Height) / 2.0f;

        UIGraphics.BeginImageContextWithOptions(newSize, false, 0);
        self.Draw(scaledImageRect);

        UIImage scaledImage = UIGraphics.GetImageFromCurrentImageContext();
        UIGraphics.EndImageContext();
        return scaledImage;
    }
}
#endif