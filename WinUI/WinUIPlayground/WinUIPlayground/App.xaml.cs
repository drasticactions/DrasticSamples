// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.DependencyInjection;
using Drastic.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SharedPlayground.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIPlayground
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
            Ioc.Default.ConfigureServices(
             new ServiceCollection()
             .AddSingleton<IAppDispatcher>(new WindowsAppDispatche(dispatcherQueue))
             .AddSingleton<IErrorHandlerService, ErrorHandlerService>()
             .AddTransient<MastonetViewModel>()
             .BuildServiceProvider());
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new BaseWindow(new MastonetStatusPage());
            m_window.Activate();
        }

        private Window m_window;
    }
}
