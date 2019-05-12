using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpaxe.DynamicProxy.Internal;
using Sharpaxe.DynamicProxy.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpaxe.DynamicProxy.Tests.Internal
{
    [TestClass]
    public class TypeFactoryTests
    {
        [TestMethod]
        public void CreatePropertyDetectoryType_PropertyGetterInterface_ReturnNotNull()
        {
            Assert.IsNotNull(GetPropertyDetector(typeof(IPropertyGetterInterface)));
        }

        [TestMethod]
        public void CreatePropertyDetectorType_PropertySetterInterface_ReturnNotNull()
        {
            Assert.IsNotNull(GetPropertyDetector(typeof(IPropertySetterInterface)));
        }

        [TestMethod]
        public void CreateEventDetectorType_EventInterface_ReturnNotNull()
        {
            Assert.IsNotNull(GetEventDetector(typeof(IEventDetector)));
        }

        [TestMethod]
        public void CreateMethodDetector_MethodInterface_ReturnNotNull()
        {
            Assert.IsNotNull(GetMethodDetector(typeof(IMethodDetector)));
        }

        private IPropertyDetector GetPropertyDetector(Type type)
        {
            return (IPropertyDetector)Activator.CreateInstance(GetDetectorType(type));
        }

        private IEventDetector GetEventDetector(Type type)
        {
            return (IEventDetector)Activator.CreateInstance(GetDetectorType(type));
        }

        private IMethodDetector GetMethodDetector(Type type)
        {
            return (IMethodDetector)Activator.CreateInstance(GetDetectorType(type));
        }

        private Type GetDetectorType(Type type)
        {
            if (!targetTypeToDetectorTypeMap.TryGetValue(type, out Type detectorType))
            {
                detectorType = TypeFactory.CreateDetectorType(type, Static.ModuleBinder.Value);
                targetTypeToDetectorTypeMap.Add(type, detectorType);
            }
            return detectorType;
        }

        private static Dictionary<Type, Type> targetTypeToDetectorTypeMap = new Dictionary<Type, Type>();
    }
}
