using System.Reflection;

namespace Sharpaxe.DynamicProxy.Internal
{
    internal interface IEventDetector
    {
        EventInfo GetDetectedEvent();
    }
}
