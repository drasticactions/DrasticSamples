using System;
using MonoTouch.Dialog;

namespace PureLayoutSample
{
    public class MainMenuViewController
    {
        private DialogViewController dv;
        private RootElement rootElement;
        private UINavigationController nav;

        public MainMenuViewController()
        {
            this.rootElement = new RootElement("Main Menu")
            {
                new Section("Layout Demos")
                {
                    new StringElement("Basic Auto Layout"),
                    new StringElement("Working with Arrays of Views"),
                    new StringElement("Distributing Views"),
                    new StringElement("Leading & Trailing Attributes"),
                    new StringElement("Cross-Attribute Constraints"),
                    new StringElement("Priorities & Inequalities"),
                    new StringElement("Animating Constraints"),
                    new StringElement("Constraint Identifiers"),
                    new StringElement("Layout Margins"),
                    new StringElement("Constraints Without Installing"),
                    new StringElement("Basic UIScrollView"),
                },
            };

            this.dv = new DialogViewController(this.rootElement);
            this.dv.OnSelection += Dv_OnSelection;
            this.nav = new UINavigationController(this.dv);
        }

        private void Dv_OnSelection(NSIndexPath obj)
        {
            var element = this.rootElement[obj.Section][obj.Row]!;

            // Root Section.
            if (obj.Section == 0)
            {
                switch (obj.Row)
                {
                    case 0:
                        this.RootViewController.PushViewController(new BasicLayoutViewController(), true);
                        break;
                    case 1:
                        this.RootViewController.PushViewController(new ArraysViewController(), true);
                        break;
                    case 10:
                        this.RootViewController.PushViewController(new ScrollViewController(), true);
                        break;
                }
            }
        }

        public UINavigationController RootViewController => this.nav;
    }
}