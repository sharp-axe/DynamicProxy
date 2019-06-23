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
    public class EventDetectorFactoryTests
    {
        #region IEventInterface

        [TestMethod]
        public void Create_IEventInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetEventDetectorType(typeof(IEvent)));
        }

        [TestMethod]
        public void CreateInstance_IEventInterface_DoesNotThrowException()
        {
            var detectorType = GetEventDetectorType(typeof(IEvent));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubscribeTheSameEventTwice_IEventInterface_ThrowAnExpectedException()
        {
            IEvent eventInstance;
            try
            {
                var detectorType = GetEventDetectorType(typeof(IEvent));
                eventInstance = (IEvent)Activator.CreateInstance(detectorType);
                eventInstance.EventEmptyArgs += (o, a) => { };
                eventInstance.EventEmptyArgs += (o, a) => { };
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(nameof(eventInstance.EventEmptyArgs)));
                throw;
            }
            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubscribeDiffrentEvents_IEventInterface_ThrowsAnExpectedException()
        {
            IEvent eventInstance;
            try
            {
                var detectorType = GetEventDetectorType(typeof(IEvent));
                eventInstance = (IEvent)Activator.CreateInstance(detectorType);
                eventInstance.EventEmptyArgs += (o, a) => { };
                eventInstance.EventIntArgs += (o, a) => { };
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(nameof(eventInstance.EventEmptyArgs)));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetDetectedEvent_NoEventHasBeenSubscribed_ThrowsAnExpectedException()
        {
            var detectorType = GetEventDetectorType(typeof(IEvent));
            var eventDetector = (IEventDetector)Activator.CreateInstance(detectorType);
            eventDetector.GetDetectedEvent();
        }

        [TestMethod]
        public void GetDetectedEvent_EmptyArgsHasBeenSubscribed_ReturnsTheExpectedEvent()
        {
            var detectorType = GetEventDetectorType(typeof(IEvent));
            var instance = Activator.CreateInstance(detectorType);
            var eventInstance = (IEvent)instance;
            var eventDetector = (IEventDetector)instance;
            eventInstance.EventEmptyArgs += (o, a) => { };
            Assert.AreEqual(typeof(IEvent).GetEvent(nameof(eventInstance.EventEmptyArgs)), eventDetector.GetDetectedEvent());
        }

        [TestMethod]
        public void GetDetectedEvent_IntArgsHasBeenSubscribed_ReturnsTheExpectedEvent()
        {
            var detectorType = GetEventDetectorType(typeof(IEvent));
            var instance = Activator.CreateInstance(detectorType);
            var eventInstance = (IEvent)instance;
            var eventDetector = (IEventDetector)instance;
            eventInstance.EventIntArgs += (o, a) => { };
            Assert.AreEqual(typeof(IEvent).GetEvent(nameof(eventInstance.EventIntArgs)), eventDetector.GetDetectedEvent());
        }
        [TestMethod]
        public void GetDetectedEvent_StringArgsHasBeenSubscribed_ReturnsTheExpectedEvent()
        {
            var detectorType = GetEventDetectorType(typeof(IEvent));
            var instance = Activator.CreateInstance(detectorType);
            var eventInstance = (IEvent)instance;
            var eventDetector = (IEventDetector)instance;
            eventInstance.EventStringArgs += (o, a) => { };
            Assert.AreEqual(typeof(IEvent).GetEvent(nameof(eventInstance.EventStringArgs)), eventDetector.GetDetectedEvent());
        }

        #endregion IEventInterface

        #region Other interfaces

        [TestMethod]
        public void Create_IPropertyGetterInterface_ReturnNotNull()
        {
            Assert.IsNotNull(GetEventDetectorType(typeof(IPropertyGetter)));
        }

        [TestMethod]
        public void Create_IPropetySetterInterface_ReturnNotNull()
        {
            Assert.IsNotNull(GetEventDetectorType(typeof(IPropertySetter)));
        }

        [TestMethod]
        public void Create_IMethodInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetEventDetectorType(typeof(IMethod)));
        }

        [TestMethod]
        public void Create_IInterface_ReturnsNotNull()
        {
            Assert.IsNotNull(GetEventDetectorType(typeof(IInterface)));
        }

        [TestMethod]
        public void CreateInstance_IPropertyGetterInterface_ThrowsNoException()
        {
            var detectorType = GetEventDetectorType(typeof(IPropertyGetter));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        public void CreateInstance_IPropertySetterInterface_ThrowsNoException()
        {
            var detectorType = GetEventDetectorType(typeof(IPropertySetter));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        public void CreateInstance_IMethodInterface_ThrowsNoException()
        {
            var detectorType = GetEventDetectorType(typeof(IMethod));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        public void CreateInstance_IInterface_ThrowsNoException()
        {
            var detectorType = GetEventDetectorType(typeof(IInterface));
            Activator.CreateInstance(detectorType);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetProperty_IPropertyGetterInterface_ThrowsNoException()
        {
            try
            {
                var detectorType = GetEventDetectorType(typeof(IPropertyGetter));
                var instance = (IPropertyGetter)Activator.CreateInstance(detectorType);
                var value = instance.Int;
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("event add method"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetProperty_IPropertyGetterInterface_ThrowsNoException()
        {
            try
            {
                var detectorType = GetEventDetectorType(typeof(IPropertySetter));
                var instance = (IPropertySetter)Activator.CreateInstance(detectorType);
                instance.Int = default(int);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("event add method"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetProperty_IMethodInterface_ThrowsNoException()
        {
            try
            {
                var detectorType = GetEventDetectorType(typeof(IMethod));
                var instance = (IMethod)Activator.CreateInstance(detectorType);
                instance.Action();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains("event add method"));
                throw;
            }
        }

        #endregion

        public Type GetEventDetectorType(Type targetType)
        {
            return targetTypeToEventDetectorTypeMap.GetOrAdd(targetType, t => new EventDetectorBuilder(targetType, Static.ModuleBinder.Value).CreateDetectorType());
        }

        private static ConcurrentDictionary<Type, Type> targetTypeToEventDetectorTypeMap = new ConcurrentDictionary<Type, Type>();
    }
}
