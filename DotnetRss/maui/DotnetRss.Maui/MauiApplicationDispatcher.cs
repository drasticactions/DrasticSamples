// <copyright file="MauiApplicationDispatcher.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.Services;

namespace DotnetRss.Maui
{
    public class MauiApplicationDispatcher : IAppDispatcher
    {

        public bool Dispatch(Action action)
            => App.Current!.Dispatcher.Dispatch(action);
    }
}