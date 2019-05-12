using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Sharpaxe.DynamicProxy.Tests.Helpers
{
    public static class Static
    {
        public static Lazy<ModuleBuilder> ModuleBinder = new Lazy<ModuleBuilder>(CreateModuleBuilder, LazyThreadSafetyMode.ExecutionAndPublication);

        private static ModuleBuilder CreateModuleBuilder()
        {
            var dynamicAssemblyName = String.Format(DynamicAssemblyFormat, Assembly.GetExecutingAssembly().GetName().Name);
            var dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(dynamicAssemblyName), AssemblyBuilderAccess.Run);
            return dynamicAssembly.DefineDynamicModule(DynamicModuleName);
        }

        private const string DynamicAssemblyFormat = "{0}__Sharpaxe.Dynamic";
        private const string DynamicModuleName = "DynamicModule";
    }
}
