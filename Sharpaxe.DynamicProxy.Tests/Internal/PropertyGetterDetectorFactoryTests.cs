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
            Assert.AreEqual(default(bool), propertyGetter.BoleanPropertyWithGetter);
        }

        [TestMethod]
        public void GetByteProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(byte), propertyGetter.BytePropertyWithGetter);
        }

        [TestMethod]
        public void GetSByteProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(sbyte), propertyGetter.SBytePropertyWithGetter);
        }

        [TestMethod]
        public void GetCharProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(char), propertyGetter.CharPropertyWithGetter);
        }

        [TestMethod]
        public void GetDecimalProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(decimal), propertyGetter.DecimalPropertyWithGetter);
        }

        [TestMethod]
        public void GetDoubleProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(double), propertyGetter.DoublePropertyWithGetter);
        }

        [TestMethod]
        public void GetFloatProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(float), propertyGetter.FloatPropertyWithGetter);
        }

        [TestMethod]
        public void GetIntProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(int), propertyGetter.IntPropertyWithGetter);
        }

        [TestMethod]
        public void GetUIntProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(uint), propertyGetter.UIntPropertyWithGetter);
        }

        [TestMethod]
        public void GetLongProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(long), propertyGetter.LongPropertyWithGetter);
        }

        [TestMethod]
        public void GetULongProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(ulong), propertyGetter.ULongPropertyWithGetter);
        }

        [TestMethod]
        public void GetShortProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(short), propertyGetter.ShortPropertyWithGetter);
        }

        [TestMethod]
        public void GetUShortProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(ushort), propertyGetter.UShortPropertyWithGetter);
        }

        [TestMethod]
        public void GetTestStructProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(TestStruct), propertyGetter.StructPropertyWithGetter);
        }

        [TestMethod]
        public void GetReferenceProperty_IPropertyGetterInterface_ReturnsDefaultValue()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
            Assert.AreEqual(default(string), propertyGetter.ReferencePropertyWithGetter);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetPropertyTwice_IPropertyGetterInterface_ThrowsAnExpectedException()
        {
            try
            {
                var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
                var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
                var _ = propertyGetter.IntPropertyWithGetter;
                _ = propertyGetter.IntPropertyWithGetter;
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("IntPropertyWithGetter"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetPropertyOneByOne_IPropertyGetterInterface_ThrowsAnExpectedException()
        {
            try
            {
                var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
                var propertyGetter = (IPropertyGetter)Activator.CreateInstance(detectorType);
                var _b = propertyGetter.BoleanPropertyWithGetter;
                var _i = propertyGetter.IntPropertyWithGetter;
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("BoleanPropertyWithGetter"));
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
            var _ = propertyGetter.BoleanPropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.BoleanPropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_BytePropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.BytePropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.BytePropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_SBytePropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.SBytePropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.SBytePropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_CharPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.CharPropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.CharPropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_DecimalPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.DecimalPropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.DecimalPropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_DoublePropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.DoublePropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.DoublePropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_FloatPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.FloatPropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.FloatPropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_IntPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.IntPropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.IntPropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_UIntPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.UIntPropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.UIntPropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_LongPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.LongPropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.LongPropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_ULongPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.ULongPropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.ULongPropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_ShortPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.ShortPropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.ShortPropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_UShortPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.UShortPropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.UShortPropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_StructPropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.StructPropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.StructPropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        [TestMethod]
        public void GetDetectedProperty_ReferencePropertyHasBeenDetected_ReturnTheExpectedProperty()
        {
            var detectorType = GetPropertyGetterDetectorType(typeof(IPropertyGetter));
            var instance = Activator.CreateInstance(detectorType);
            var propertyGetter = (IPropertyGetter)instance;
            var propertyDetector = (IPropertyGetterDetector)instance;
            var _ = propertyGetter.ReferencePropertyWithGetter;
            Assert.AreEqual(typeof(IPropertyGetter).GetProperty(nameof(propertyGetter.ReferencePropertyWithGetter)), propertyDetector.GetDetectedProperty());
        }

        public Type GetPropertyGetterDetectorType(Type targetType)
        {
            return targetTypeToPropertyGetterDetectorTypeMap.GetOrAdd(targetType, t => new PropertyGetterDetectorBuilder(targetType, Static.ModuleBinder.Value).CreateDetectorType());
        }

        private static ConcurrentDictionary<Type, Type> targetTypeToPropertyGetterDetectorTypeMap = new ConcurrentDictionary<Type, Type>();
    }
}
