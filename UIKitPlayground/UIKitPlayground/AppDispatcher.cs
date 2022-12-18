using System;
using Drastic.Services;

namespace UIKitPlayground
{
	public class AppDispatcher : NSObject, IAppDispatcher
    {
        public bool Dispatch(Action action)
        {
            this.InvokeOnMainThread(action);
            return true;
        }
    }
}