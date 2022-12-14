namespace PureLayoutSample.iOS;

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
        var vc = new MainMenuViewController();
        Window.RootViewController = vc.RootViewController;

        // make the window visible
        Window.MakeKeyAndVisible();

        return true;
    }
}
