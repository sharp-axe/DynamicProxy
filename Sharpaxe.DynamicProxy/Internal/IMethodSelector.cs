using System.Reflection;

namespace Sharpaxe.DynamicProxy.Internal
{
    internal interface IMethodSelector
    {
        MethodInfo GetSelectedMethod(object methodToken);
    }
}
