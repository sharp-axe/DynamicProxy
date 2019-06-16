using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpaxe.DynamicProxy.Internal.Detector;
using Sharpaxe.DynamicProxy.Internal.Detector.Builder;
using Sharpaxe.DynamicProxy.Tests.TestHelper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Sharpaxe.DynamicProxy.Tests.Internal.Detector.Builder
{
    [TestClass]
    public class PropertySetterDetectorFactoryTests
    {
        #region IPropertySetterInterface

        [TestMethod]
        public void Create_IPropertySetterInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetPropertySetterDetectorType(typeof(IPropertySetter)));
        }

        [TestMethod]
        public void CreateInstance_IPropertySetterInterface_DoesNotThrowException()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetPropertyTwice_IPropertySetterInterface_ThrowsAnExpectedException()
        {
            IPropertySetter propertySetter;
            try
            {
                var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
                propertySetter = (IPropertySetter)Activator.CreateInstance(detectorType);
                propertySetter.Int = default;
                propertySetter.Int = default;
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(nameof(propertySetter.Int)));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetPropertyOneByOne_IPropertySetterInterface_ThrowsAnExpectedException()
        {
            IPropertySetter propertySetter;
            try
            {
                var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
                propertySetter = (IPropertySetter)Activator.CreateInstance(detectorType);
                propertySetter.Boolean = default;
                propertySetter.Int = default;
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(nameof(propertySetter.Boolean)));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetDetectedProperty_NoPropertyHasBeenCalled_ThrowsAnExpectedException()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var propertyDetector = (IPropertySetterDetector)Activator.CreateInstance(detectorType);
            propertyDetector.GetDetectedProperty();
        }

        [TestMethod]
        public void GetDetectedProperty_BooleanPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.Boolean = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.Boolean)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_BytePropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.Byte = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.Byte)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_SBytePropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.SByte = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.SByte)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_CharPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.Char = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.Char)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_DecimalPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.Decimal = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.Decimal)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_DoublePropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.Double = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.Double)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_FloatPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.Float = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.Float)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_IntPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.Int = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.Int)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_UIntPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.UInt = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.UInt)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_LongPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.Long = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.Long)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_ULongPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.ULong = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.ULong)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_ShortPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.Short = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.Short)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_UShortPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.UShort = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.UShort)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_StructPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.Struct = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.Struct)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_ReferencePropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertySetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertySetter = (IPropertySetter)instance;
            var propertyDetector = (IPropertySetterDetector)instance;
            propertySetter.Class = default;
            Assert.AreEqual(typeof(IPropertySetter).GetProperty(nameof(propertySetter.Class)), propertyDetector.GetDetectedProperty());
        }

        #endregion IPropertySetterInterface

        #region Other interfaces

        [TestMethod]
        public void Create_IMethodInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetPropertySetterDetectorType(typeof(IMethod)));
        }

        [TestMethod]
        public void Create_IEventInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetPropertySetterDetectorType(typeof(IEvent)));
        }

        [TestMethod]
        public void Create_IPropertyGetterInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetPropertySetterDetectorType(typeof(IPropertyGetter)));
        }

        [TestMethod]
        public void Create_IInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetPropertySetterDetectorType(typeof(IInteface)));
        }

        [TestMethod]
        public void CreateInstance_IMethodInterface_ThrowsNoException()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IMethod));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        public void CreateInstance_IEventInterface_ThrowsNoException()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IEvent));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        public void CreateInstance_IPropertySetterInterface_ThrowsNoException()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IPropertyGetter));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        public void CreateInstance_IInterface_ThrowsNoException()
        {
            var detectorType = GetPropertySetterDetectorType(typeof(IInteface));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void InvokeMethod_IMethodDetector_ThrowsAnExpectedException()
        {
            try
            {
                var detectorType = GetPropertySetterDetectorType(typeof(IMethod));
                var instance = (IMethod)Activator.CreateInstance(detectorType);
                instance.Action();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("property setter"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void AddHandler_IEventDetector_ThrowsAnExpectedException()
        {
            try
            {
                var detectorType = GetPropertySetterDetectorType(typeof(IEvent));
                var instance = (IEvent)Activator.CreateInstance(detectorType);
                instance.EventEmptyArgs += (o, a) => { };
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("property setter"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RemoveHandler_IEventDetector_ThrowsAnExpectedException()
        {
            try
            {
                var detectorType = GetPropertySetterDetectorType(typeof(IEvent));
                var instance = (IEvent)Activator.CreateInstance(detectorType);
                instance.EventEmptyArgs -= (o, a) => { };
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("property setter"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetProperty_IPropertySetterDetector_ThrowsAnExpectedException()
        {
            try
            {
                var detectorType = GetPropertySetterDetectorType(typeof(IPropertyGetter));
                var instance = (IPropertyGetter)Activator.CreateInstance(detectorType);
                var _ = instance.Boolean;
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("property setter"));
                throw;
            }
        }

        #endregion

        public Type GetPropertySetterDetectorType(Type targetType)
        {
            return targetTypeToPropertySetterDetectorTypeMap.GetOrAdd(targetType, t => new PropertySetterDetectorBuilder(targetType, Static.ModuleBinder.Value).CreateDetectorType());
        }

        private static ConcurrentDictionary<Type, Type> targetTypeToPropertySetterDetectorTypeMap = new ConcurrentDictionary<Type, Type>();
    }
}
