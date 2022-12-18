using CommunityToolkit.Mvvm.DependencyInjection;
using CoreFoundation;
using Drastic.Services;
using Microsoft.Extensions.DependencyInjection;
using SharedPlayground.ViewModels;

namespace UIKitPlayground.iOS;

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
        Ioc.Default.ConfigureServices(
             new ServiceCollection()
             .AddSingleton<IAppDispatcher, UIKitPlayground.AppDispatcher>()
             .AddSingleton<IErrorHandlerService, UIKitPlayground.ErrorHandlerService>()
             .AddTransient<MastonetViewModel>()
             .AddTransient<RecipeListViewModel>()
             .BuildServiceProvider());

        // create a new window instance based on the screen size
        Window = new UIWindow(UIScreen.MainScreen.Bounds);

        // create a UIViewController with a single UILabel
        var vc = new RssFeedTableViewController();
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
        splitViewController.SetViewController(new ImageTableViewController(), UISplitViewControllerColumn.Supplementary);
        splitViewController.SetViewController(new NowPlayingTableViewController(), UISplitViewControllerColumn.Secondary);

        splitViewController.PreferredPrimaryColumnWidth = 150f;
        splitViewController.PreferredSupplementaryColumnWidth = 200f;
        return splitViewController;
    }
}
