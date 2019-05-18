using System.Reflection;

namespace Sharpaxe.DynamicProxy.Internal.Detector
{
#warning Should be an internal interface - change it after skip clr visibility check has been done
    internal interface IPropertySetterDetector
    {
        PropertyInfo GetDetectedProperty();
    }
}
