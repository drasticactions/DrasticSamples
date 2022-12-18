using CommunityToolkit.Mvvm.DependencyInjection;
using Drastic.Services;
using Microsoft.Extensions.DependencyInjection;
using SharedPlayground.ViewModels;

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
        Ioc.Default.ConfigureServices(
     new ServiceCollection()
     .AddSingleton<IAppDispatcher, UIKitPlayground.AppDispatcher>()
     .AddSingleton<IErrorHandlerService, UIKitPlayground.ErrorHandlerService>()
     .AddTransient<MastonetViewModel>()
     .AddTransient<RecipeListViewModel>()
     .BuildServiceProvider());

        this.Window = new UIWindow();

        this.Window.RootViewController = new RecipeListViewModelViewController();

        this.Window.MakeKeyAndVisible();

        return true;
    }
}
