﻿using System;
using System.Text.Json.Serialization;
using ObjCRuntime;
using Drastic.PureLayout;
using System.Linq;
using static UIKit.NSCollectionLayoutDimension;
using Drastic.Nuke;
using DrasticTabBarItem = UIKitPlayground.TabBarItem;
using static System.Net.Mime.MediaTypeNames;
using UIKit;

namespace UIKitPlayground
{
    public enum TabBarItem
    {
        All,
        Favorites,
        Recents,
        Collections,
    }

    public static class TabBarItemExtensions
    {
        public static string Title(this TabBarItem item)
        {
            switch (item)
            {
                case TabBarItem.All:
                    return "All Recipies";
                case TabBarItem.Collections:
                    return "Collections";
                case TabBarItem.Favorites:
                    return "Favorites";
                case TabBarItem.Recents:
                    return "Recents";
            }

            return string.Empty;
        }

        public static UIImage? Image(this TabBarItem item)
        {
            switch (item)
            {
                case TabBarItem.All:
                    return UIImage.GetSystemImage("tray");
                case TabBarItem.Favorites:
                    return UIImage.GetSystemImage("heart.circle");
                case TabBarItem.Collections:
                    return UIImage.GetSystemImage("folder");
                case TabBarItem.Recents:
                    return UIImage.GetSystemImage("clock");
            }

            return null;
        }
    }

    public class RecipeListViewController : UIViewController, IUICollectionViewDelegate
    {
        private UICollectionView? collectionView;
        private UICollectionViewFlowLayout collectionViewLayout;
        private UICollectionViewDiffableDataSource<NSString, Recipe>? dataSource;
        private Recipe? selectedRecipe;
        private DrasticTabBarItem selectedDataType = DrasticTabBarItem.All;
        private NSString? recipeCollectionName;
        private List<Recipe> recipies = new List<Recipe>();

        public RecipeListViewController()
        {
            this.collectionViewLayout = new UICollectionViewFlowLayout();
            var faker = new FakeRecipie();
            for (var i = 0; i <= 100; i++)
            {
                this.recipies.Add(faker.Generate());
            }
        }

        public void ShowRecipes(string title)
        {
            this.selectedDataType = DrasticTabBarItem.Collections;
            this.recipeCollectionName = new NSString(title);

            this.Apply(this.recipies, true);

            // HACK
            this.View!.LayoutIfNeeded();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.collectionView = new UICollectionView(this.View!.Bounds!, this.collectionViewLayout);
            this.collectionView.ClipsToBounds = true;
            //this.collectionView.MultipleTouchEnabled = true;
            this.collectionView.TranslatesAutoresizingMaskIntoConstraints = false;

            this.View!.AddSubview(this.collectionView);

            this.ConfigureCollectionView();

            this.collectionViewLayout.EstimatedItemSize = UICollectionViewFlowLayout.AutomaticSize;
            this.collectionViewLayout.FooterReferenceSize = new CGSize(0, 0);
            this.collectionViewLayout.HeaderReferenceSize = new CGSize(0, 0);
            this.collectionViewLayout.ItemSize = new CGSize(128, 128);
            this.collectionViewLayout.MinimumInteritemSpacing = 10;
            this.collectionViewLayout.MinimumLineSpacing = 10;
            this.collectionViewLayout.SectionInset = new UIEdgeInsets(top: 0, left: 0, bottom: 0, right: 0);

            this.collectionView.AutoPinEdgesToSuperviewSafeArea();

            this.ConfigureDataSource();

            this.ShowRecipes("Hoge");
        }

        [Export("collectionView:didSelectItemAtIndexPath:")]
        protected void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var value = this.dataSource?.GetItemIdentifier(indexPath);
            if (value is null)
                return;

            this.selectedRecipe = value;

            var cell = (RecipeListCell)this.dataSource?.GetCell(collectionView, indexPath)!;
            this.InvokeOnMainThread(() => cell!.imageView.Blur());
            //this.recipeDetailViewController.Recipe = value;
        }

        private void ConfigureDataSource()
        {
            this.collectionView!.RegisterClassForCell(typeof(RecipeListCell), RecipeListCell.ReuseIdentifier);

            this.dataSource = new UICollectionViewDiffableDataSource<NSString, Recipe>(collectionView,
                new UICollectionViewDiffableDataSourceCellProvider((collectionView, indexPath, item) =>
                {

                    var cell = collectionView.DequeueReusableCell(RecipeListCell.ReuseIdentifier, indexPath);
                    if (cell is RecipeListCell recipeListCell)
                    {
                        if (item is Recipe recipe)
                        {
                            recipeListCell.Configure(recipe);
                        }
                    }

                    return (UICollectionViewCell)cell;
                })
            );
        }

        private void Apply(List<Recipe> recipies, bool animated = false)
        {
            // Determine what recipes to append to the snapshot.
            List<Recipe> recipiesToAppend = new List<Recipe>();
            switch (this.selectedDataType)
            {
                case DrasticTabBarItem.All:
                default:
                    recipiesToAppend = recipies;
                    break;
            }

            var snapshot = new NSDiffableDataSourceSnapshot<NSString, Recipe>();
            var title = new NSString(Section.Main.ToString());
            snapshot.AppendSections(new[] { title });
            snapshot.AppendItems(recipiesToAppend.ToArray(), title);

            NSIndexPath? selectedRecipeIndexPath = null;

            this.dataSource?.ApplySnapshot(snapshot, animated, () => {

            });
        }

        private void ConfigureCollectionView()
        {
            this.collectionView!.Delegate = this;
            this.collectionView.AlwaysBounceVertical = true;
            this.collectionView.CollectionViewLayout = this.CreateLayout();
        }

        private UICollectionViewCompositionalLayout CreateLayout()
        {
            var layout = new UICollectionViewCompositionalLayout((sectionIndex, layoutEnvironment) =>
            {
                var recipeItemSize = NSCollectionLayoutSize.Create(CreateFractionalWidth(1.0f), CreateFractionalHeight(1.0f));

                var recipeItem = NSCollectionLayoutItem.Create(recipeItemSize);

                recipeItem.ContentInsets = new NSDirectionalEdgeInsets(top: 5, leading: 10, bottom: 5, trailing: 10);

                var groupSize = NSCollectionLayoutSize.Create(CreateFractionalWidth(1.0f), CreateFractionalWidth(0.375f));

                var group = NSCollectionLayoutGroup.CreateHorizontal(groupSize, recipeItem, 2);

                var section = NSCollectionLayoutSection.Create(group);

                return section;
            });

            return layout;
        }

        private enum Section
        {
            Main,
        }
    }

    public class FakeRecipie : Bogus.Faker<Recipe>
    {
        public FakeRecipie()
        {
            RuleFor(o => o.Title, f => string.Join(" ", f.Lorem.Words()));
            RuleFor(o => o.LargeImage, f => f.Image.PicsumUrl());
            RuleFor(o => o.SmallImage, f => f.Image.PicsumUrl());
        }
    }

    public class RecipeListCell : UICollectionViewCell
    {
        public UIImageView imageView = new UIImageView();
        public UIImageView favoriteImageView = new UIImageView();
        public UILabel titleLabel = new UILabel();
        public UIView view2 = new UIView();
        public static string ReuseIdentifier = nameof(RecipeListCell);

#if TVOS
        private UIColor borderColor = UIColor.Gray.ColorWithAlpha(2.0f);
#else
        private UIColor borderColor = UIColor.SystemGray2.ColorWithAlpha(2.0f);
#endif
        private UIColor selectedBorderColor = UIColorExtensions.GetSystemTint();
        private ImagePipeline pipeline;

        protected internal RecipeListCell(NativeHandle handle) : base(handle)
        {
            this.pipeline = ImagePipeline.Shared;
            this.SetupUI();
            this.SetupLayout();
        }

        private void SetupUI()
        {
            this.AutoresizingMask = UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleBottomMargin;
            this.ClipsToBounds = true;
            this.ContentMode = UIViewContentMode.Center;
            //this.Frame = new CGRect(0, 0, 250, 200);
            //this.MultipleTouchEnabled = true;

            this.ContentView.AddSubview(this.imageView);
            this.ContentView.AddSubview(this.view2);

            this.ContentView.ClipsToBounds = true;
            this.ContentView.ContentMode = UIViewContentMode.Center;
            //this.ContentView.Frame = new CGRect(0, 0, 250, 200);
            this.ContentView.InsetsLayoutMarginsFromSafeArea = false;
            //this.ContentView.MultipleTouchEnabled = true;

            this.view2.AddSubview(this.titleLabel);

            this.view2.BackgroundColor = UIColor.Black.ColorWithAlpha(.5f);
            this.view2.TranslatesAutoresizingMaskIntoConstraints = false;

            this.imageView.ClipsToBounds = true;
            this.imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            this.imageView.SetContentHuggingPriority(251, UILayoutConstraintAxis.Horizontal);
            this.imageView.SetContentHuggingPriority(251, UILayoutConstraintAxis.Vertical);
            this.imageView.TranslatesAutoresizingMaskIntoConstraints = false;

#if TVOS
            this.imageView.AdjustsImageWhenAncestorFocused = true;
#endif
            this.titleLabel.AdjustsFontForContentSizeCategory = true;
            this.titleLabel.ContentMode = UIViewContentMode.Left;
            this.titleLabel.Font = UIFont.PreferredHeadline;
            this.titleLabel.MinimumScaleFactor = 0.75f;
            this.titleLabel.SetContentHuggingPriority(251, UILayoutConstraintAxis.Horizontal);
            this.titleLabel.SetContentHuggingPriority(251, UILayoutConstraintAxis.Vertical);
            this.titleLabel.Text = "Recipe Title";
            this.titleLabel.TextAlignment = UITextAlignment.Center;
            this.titleLabel.TranslatesAutoresizingMaskIntoConstraints = false;
        }

        private void SetupLayout()
        {
            this.imageView.AutoPinEdgesToSuperviewEdges();

            this.view2.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Top);
            this.view2.AutoMatchDimensionWithMultiplier(ALDimension.Height, ALDimension.Height, this, .25f);
            //this.view2.AutoSetDimension(ALDimension.Height, 50);

            this.titleLabel.AutoPinEdgesToSuperviewEdges();

            this.Layer.BorderColor = this.borderColor.CGColor;
            this.Layer.BorderWidth = 1;
            this.Layer.CornerRadius = 8;
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
            this.titleLabel.Text = null;
            this.imageView.Image = null;
            this.favoriteImageView.Alpha = 0;
        }

        public void Configure(Recipe recipe)
        {
            this.titleLabel.Text = recipe.Title;
            this.pipeline.LoadImageWithUrl(new NSUrl(recipe.SmallImage!), UIImage.FromBundle("DotNetBot"), null, null, null, this.imageView);
            this.favoriteImageView.Alpha = recipe.IsFavorite ? 1 : 0;
        }

        public override bool Selected
        {
            get => base.Selected; set
            {
                base.Selected = value;

                this.Layer.BorderColor = value ? this.selectedBorderColor.CGColor : this.borderColor.CGColor;
                this.Layer.BorderWidth = value ? 2 : 1;
            }
        }
#if TVOS

        public override void DidUpdateFocus(UIFocusUpdateContext context, UIFocusAnimationCoordinator coordinator)
        {
            //context.NextFocusedView!.Layer.BackgroundColor = UIColor.White.CGColor;

            //if (context.PreviouslyFocusedView is RecipeListCell)
            //{
            //    context.PreviouslyFocusedView.Layer.BackgroundColor = this.BackgroundColor!.CGColor;
            //}
        }
#endif
    }

    public class Recipe : NSObject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("prepTime")]
        public int PrepTime { get; set; }

        [JsonPropertyName("cookTime")]
        public int CookTime { get; set; }

        [JsonPropertyName("servings")]
        public string Servings { get; set; }

        [JsonPropertyName("ingredients")]
        public string Ingredients { get; set; }

        [JsonPropertyName("directions")]
        public string Directions { get; set; }

        [JsonPropertyName("isFavorite")]
        public bool IsFavorite { get; set; }

        [JsonPropertyName("addedOn")]
        public DateTime AddedOn { get; set; }

        [JsonPropertyName("collections")]
        public List<string> Collections { get; set; }

        [JsonPropertyName("imageNames")]
        public List<string> ImageNames { get; set; }

        public string? SmallImage { get; set; }

        public string? LargeImage { get; set; }
    }
}
