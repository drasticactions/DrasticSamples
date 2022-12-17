// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIPlayground
{
    /// <summary>
    /// Base Window.
    /// Pass in a page.
    /// </summary>
    public sealed partial class BaseWindow : Window
    {
        private Page page;

        public BaseWindow(Page page)
        {
            this.InitializeComponent();

            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(this.AppTitleBar);

            this.MainFrame.Content = this.page = page;
            page.DataContextChanged += this.Page_DataContextChanged;
            var manager = WindowManager.Get(this);
            manager.Backdrop = new MicaSystemBackdrop();
        }

        private void Page_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            this.MainGrid.DataContext = args.NewValue;
        }
    }
}
