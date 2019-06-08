using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sharpaxe.DynamicProxy.Internal.Detector;
using Sharpaxe.DynamicProxy.Internal.DetectorBuilder;
using Sharpaxe.DynamicProxy.Tests.TestHelper;
using System;
using System.Collections.Concurrent;

namespace Sharpaxe.DynamicProxy.Tests.Internal
{
    [TestClass]
    public class PropertyGetterDetectorFactoryTests
    {
        #region IPropertyGetterInterface

        [TestMethod]
        public void Create_IPropertyGetterInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetPropertyGetterDetectorType(typeof(IPropertyGetter)));
        }

        [TestMethod]
        public void CreateInstance_IPropertyGetterInterface_DoesNotThrowException()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        public void GetBooleanProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(bool), propertyGetter.Boolean);
        }

        [TestMethod]
        public void GetByteProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(byte), propertyGetter.Byte);
        }

        [TestMethod]
        public void GetSByteProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(sbyte), propertyGetter.SByte);
        }

        [TestMethod]
        public void GetCharProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(char), propertyGetter.Char);
        }

        [TestMethod]
        public void GetDecimalProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(decimal), propertyGetter.Decimal);
        }

        [TestMethod]
        public void GetDoubleProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(double), propertyGetter.Double);
        }

        [TestMethod]
        public void GetFloatProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(float), propertyGetter.Float);
        }

        [TestMethod]
        public void GetIntProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(int), propertyGetter.Int);
        }

        [TestMethod]
        public void GetUIntProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(uint), propertyGetter.UInt);
        }

        [TestMethod]
        public void GetLongProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(long), propertyGetter.Long);
        }

        [TestMethod]
        public void GetULongProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(ulong), propertyGetter.ULong);
        }

        [TestMethod]
        public void GetShortProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(short), propertyGetter.Short);
        }

        [TestMethod]
        public void GetUShortProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(ushort), propertyGetter.UShort);
        }

        [TestMethod]
        public void GetTestStructProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(TestStruct), propertyGetter.Struct);
        }

        [TestMethod]
        public void GetReferenceProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(string), propertyGetter.Class);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetPropertyTwice_IPropertyGetterInterface_ThrowsAnExpectedException()
        {
            IPropertyGetter propertyGetter;
            try
            {
                var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
                propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
                var _ = propertyGetter.Int;
                _ = propertyGetter.Int;
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(nameof(propertyGetter.Int)));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetPropertyOneByOne_IPropertyGetterInterface_ThrowsAnExpectedException()
        {
            IPropertyGetter propertyGetter;
            try
            {
                var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
                propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
                var _b = propertyGetter.Boolean;
                var _i = propertyGetter.Int;
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(nameof(propertyGetter.Boolean)));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetDetectedProperty_NoPropertyHasBeenCalled_ThrowsAnExpectedException()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyDetector = (IPropertyGetterDetector)Activator.CreateInstance(detectorType);
            propertyDetector.GetDetectedProperty();
        }

        [TestMethod]
        public void GetDetectedProperty_BooleanPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.Boolean;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.Boolean)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_BytePropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.Byte;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.Byte)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_SBytePropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.SByte;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.SByte)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_CharPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.Char;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.Char)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_DecimalPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.Decimal;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.Decimal)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_DoublePropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.Double;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.Double)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_FloatPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.Float;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.Float)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_IntPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.Int;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.Int)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_UIntPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.UInt;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.UInt)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_LongPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.Long;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.Long)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_ULongPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.ULong;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.ULong)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_ShortPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.Short;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.Short)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_UShortPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.UShort;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.UShort)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_StructPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.Struct;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.Struct)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_ReferencePropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.Class;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.Class)), propertyDetector.GetDetectedProperty());
        }

        #endregion IPropertyGetterInterface

        #region Other interfaces

        [TestMethod]
        public void Create_IMethodInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetPropertyGetterDetectorType(typeof(IMethod)));
        }

        [TestMethod]
        public void Create_IEventInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetPropertyGetterDetectorType(typeof(IEvent)));
        }

        [TestMethod]
        public void Create_IPropertySetterInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetPropertyGetterDetectorType(typeof(IPropertySetter)));
        }

        [TestMethod]
        public void Create_IInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetPropertyGetterDetectorType(typeof(IInteface)));
        }

        [TestMethod]
        public void CreateInstance_IMethodInterface_ThrowsNoException()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IMethod));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        public void CreateInstance_IEventInterface_ThrowsNoException()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IEvent));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        public void CreateInstance_IPropertySetterInterface_ThrowsNoException()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertySetter));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        public void CreateInstance_IInterface_ThrowsNoException()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IInteface));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void InvokeMethod_IMethodDetector_ThrowsAnExpectedException()
        {
            try
            {
                var detectorType = GetPropertyGetterDetectorType(typeof(IMethod));
                var instance = (IMethod)Activator.CreateInstance(detectorType);
                instance.Action();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("property getter"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void AddHandler_IEventDetector_ThrowsAnExpectedException()
        {
            try
            {
                var detectorType = GetPropertyGetterDetectorType(typeof(IEvent));
                var instance = (IEvent)Activator.CreateInstance(detectorType);
                instance.EventEmptyArgs += (o, a) => { };
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("property getter"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RemoveHandler_IEventDetector_ThrowsAnExpectedException()
        {
            try
            {
                var detectorType = GetPropertyGetterDetectorType(typeof(IEvent));
                var instance = (IEvent)Activator.CreateInstance(detectorType);
                instance.EventEmptyArgs -= (o, a) => { };
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("property getter"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetProperty_IPropertySetterDetector_ThrowsAnExpectedException()
        {
            try
            {
                var detectorType = GetPropertyGetterDetectorType(typeof(IPropertySetter));
                var instance = (IPropertySetter)Activator.CreateInstance(detectorType);
                instance.Boolean = default(bool);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("property getter"));
                throw;
            }
        }

        #endregion

        public Type GetPropertyGetterDetectorType(Type targetType)
        {
            return targetTypeToPropertyGetterDetectorTypeMap.GetOrAdd(targetType, t => new PropertyGetterDetectorBuilder(targetType, Static.ModuleBinder.Value).CreateDetectorType());
        }

        private static ConcurrentDictionary<Type, Type> targetTypeToPropertyGetterDetectorTypeMap = new ConcurrentDictionary<Type, Type>();
    }
}
