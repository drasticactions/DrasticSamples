namespace PureLayoutSample.tvOS;

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
        this.Window = new UIWindow();

        var dv = new MainMenuViewController();

        this.Window.RootViewController = dv.RootViewController;
        this.Window!.MakeKeyAndVisible();

        return true;
    }
}
