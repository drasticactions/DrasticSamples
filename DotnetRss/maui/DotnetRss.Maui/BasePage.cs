// <copyright file="BasePage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.ViewModels;

namespace DotnetRss.Maui
{
    public class BasePage : ContentPage
    {
        internal BaseViewModel? vm;

        public BasePage(IServiceProvider provider)
        {
            this.Provider = provider;
        }

        internal BaseViewModel? Vm
        {
            get { return this.vm; }
            set {
                this.vm = value;
                this.BindingContext = this.vm;
            }
        }

        internal IServiceProvider Provider { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (this.Vm is not null)
            {
                this.Vm.OnLoad();
            }
        }
    }
}