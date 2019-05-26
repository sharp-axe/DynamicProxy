using System;

namespace Sharpaxe.DynamicProxy.Tests.TestHelper
{
#warning Should be an internal interface - change it after skip clr visibility check has been done
    public interface IEvent
    {
        event EventHandler EventEmptyArgs;
        event EventHandler<EventArgs<int>> EventIntArgs;
        event EventHandler<EventArgs<string>> EventStringArgs;
    }
}
