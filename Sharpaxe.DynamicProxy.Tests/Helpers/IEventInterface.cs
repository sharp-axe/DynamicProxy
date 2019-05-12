using System;

namespace Sharpaxe.DynamicProxy.Tests.Helpers
{
#warning Should be an internal interface - change it after skip clr visibility check has been done
    public interface IEventInterface
    {
        event EventHandler Event;
    }
}
