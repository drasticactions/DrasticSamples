using UIKitPlayground.MaciOS;

namespace UIKitPlayground.MacCatalyst;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override UIWindow? Window
    {
        get;
        set;
    }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // create a new window instance based on the screen size
        Window = new UIWindow(UIScreen.MainScreen.Bounds);

        // create a UIViewController with a single UILabel
        var vc = GenerateMainViewController();
        //var vc = new DemoGalleryViewController();
        Window.RootViewController = vc;

        // make the window visible
        Window.MakeKeyAndVisible();

        return true;
    }

    private static UISplitViewController GenerateMainViewController()
    {
        var sidebarViewController = new SidebarMenuViewController();
        var splitViewController = new UISplitViewController(style: UISplitViewControllerStyle.TripleColumn);
        splitViewController.PrimaryBackgroundStyle = UISplitViewControllerBackgroundStyle.Sidebar;
        splitViewController.PreferredDisplayMode = UISplitViewControllerDisplayMode.TwoBesideSecondary;

        splitViewController.SetViewController(sidebarViewController, UISplitViewControllerColumn.Primary);
        splitViewController.SetViewController(new RssFeedTableViewController(), UISplitViewControllerColumn.Supplementary);
        splitViewController.SetViewController(new PodcastTableViewController(), UISplitViewControllerColumn.Secondary);

        splitViewController.PreferredPrimaryColumnWidth = 150f;
        splitViewController.PreferredSupplementaryColumnWidth = 250f;
        return splitViewController;
    }
}
