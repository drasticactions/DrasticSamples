using System;
using Drastic.DragAndDrop;
using Drastic.PureLayout;
using ObjCRuntime;
using static CoreFoundation.DispatchSource;

namespace UIKitDragAndDrop
{
    public class MainWindow : UIWindow
    {
        public MainWindow(UIWindowScene windowScene)
            : base(windowScene)
        {
            this.RootViewController = new AppViewController();
        }

        public class AppViewController : UIViewController, IUIDragInteractionDelegate, IUIDropInteractionDelegate
        {
            private UIImageView imageView = new UIImageView();
            private UILabel label = new UILabel() { Text = $"Drag and drop an image on the window. Drag that image off of the window...", Lines = 0 };
            private UIDragInteraction imageDrag;
            private UIDropInteraction imageDrop;

            public AppViewController()
            {
                this.imageDrag = new UIDragInteraction(this);
                this.imageDrop = new UIDropInteraction(this);
                this.SetupUI();
                this.SetupLayout();
            }

            private void DragAndDropView_Drop(object? sender, Drastic.DragAndDrop.DragAndDropOverlayTappedEventArgs e)
            {
                try
                {
                    var data = File.ReadAllBytes(e.Paths.First());
                    this.imageView.Image = UIImage.LoadFromData(NSData.FromArray(data));
                }
                catch (Exception ex)
                {
                    // Ignore...
                }
            }

            private void SetupUI()
            {
                this.imageView.Layer.BackgroundColor = CGColor.CreateCmyk(255, 255, 255, 225, 1)!;
                this.imageView.AddInteraction(this.imageDrag);
                this.imageView.AddInteraction(this.imageDrop);
                this.imageView.UserInteractionEnabled = true;
                this.View!.AddSubview(this.imageView);
                this.View.AddSubview(this.label);
            }

            private void SetupLayout()
            {
                this.imageView.AutoCenterInSuperview();
                this.imageView.AutoSetDimensionsToSize(new CGSize(400, 400));
                this.label.AutoPinEdge(ALEdge.Left, ALEdge.Left, this.imageView);
                this.label.AutoPinEdge(ALEdge.Right, ALEdge.Right, this.imageView);
                this.label.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.imageView, 10f);
            }

            public UIDragItem[] GetItemsForBeginningSession(UIDragInteraction interaction, IUIDragSession session)
            {
                var provider = new NSItemProvider(this.imageView.Image!);
                var item = new UIDragItem(provider);
                item.LocalObject = this.imageView.Image!;

                return new UIDragItem[1] { item };
            }

            [Export("dropInteraction:canHandleSession:")]
            public bool CanHandleSession(UIDropInteraction interaction, IUIDropSession session)
            {
                return true;
            }

            [Export("dropInteraction:sessionDidUpdate:")]
            public UIDropProposal SessionDidUpdate(UIDropInteraction interaction, IUIDropSession session)
            {
                if (session.LocalDragSession == null)
                {
                    return new UIDropProposal(UIDropOperation.Copy);
                }

                return new UIDropProposal(UIDropOperation.Cancel);
            }

            [Export("dropInteraction:performDrop:")]
            public async void PerformDrop(UIDropInteraction interaction, IUIDropSession session)
            {
                session.ProgressIndicatorStyle = UIDropSessionProgressIndicatorStyle.None;

                var file = session.Items.FirstOrDefault();
                if (file is null)
                {
                    return;
                }

                var result = await this.LoadItemAsync(file.ItemProvider, file.ItemProvider.RegisteredTypeIdentifiers.ToList());

                try
                {
                    this.imageView.Image = UIImage.LoadFromData(NSData.FromFile(result!.FileUrl.Path!.ToString()));
                }
                catch (Exception ex)
                {

                }
            }

            private async Task<LoadInPlaceResult?> LoadItemAsync(NSItemProvider itemProvider, List<string> typeIdentifiers)
            {
                if (typeIdentifiers is null || !typeIdentifiers.Any())
                {
                    return null;
                }

                var typeIdent = typeIdentifiers.First();

                if (itemProvider.HasItemConformingTo(typeIdent))
                {
                    return await itemProvider.LoadInPlaceFileRepresentationAsync(typeIdent);
                }

                typeIdentifiers.Remove(typeIdent);

                return await this.LoadItemAsync(itemProvider, typeIdentifiers);
            }
        }
    }
}