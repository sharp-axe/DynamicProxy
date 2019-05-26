using System;

namespace Sharpaxe.DynamicProxy.Tests.TestHelper
{
#warning Should be an internal class - change it after skip clr visibility check has been done
    public class EventArgs<T> : EventArgs 
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
