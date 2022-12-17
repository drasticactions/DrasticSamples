namespace UIKitPlayground.tvOS;

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

        this.Window.RootViewController = new RecipeListViewController();

        this.Window.MakeKeyAndVisible();

        return true;
    }
}
