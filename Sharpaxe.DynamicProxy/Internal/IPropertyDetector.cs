using System.Reflection;

namespace Sharpaxe.DynamicProxy.Internal
{
    internal interface IPropertyDetector
    {
        PropertyInfo GetDetectedProperty();
    }
}
