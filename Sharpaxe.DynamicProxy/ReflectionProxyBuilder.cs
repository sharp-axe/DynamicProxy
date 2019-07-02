using Sharpaxe.DynamicProxy.Internal;
using Sharpaxe.DynamicProxy.Internal.Detector;
using Sharpaxe.DynamicProxy.Internal.Proxy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Sharpaxe.DynamicProxy
{
    public class ReflectionProxyBuilder<T> : IProxyBuilder<T>
        where T : class
    {
        private readonly Type type;
        private readonly ITypeRepository typeRepository;
        private readonly Lazy<TypeConfiguration> configuration;

        public ReflectionProxyBuilder()
        {
            type = ThrowIfTypeIsNotAnInterfaceElseReturn(typeof(T));
            configuration = new Lazy<TypeConfiguration>(() => InitializeTypeWrapSettings(type), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public T Build(T instance)
        {
            (object proxy, IProxyConfigurator proxyConfigurator) = typeRepository.CreateConfigurableProxy(type, instance);

            ConfigurePropertiesGetter(proxyConfigurator);
            ConfigurePropertiesSetter(proxyConfigurator);
            ConfigureEvents(proxyConfigurator);
            ConfigureMethods(proxyConfigurator);

            return (T)proxy;
        }

        private void ConfigurePropertiesGetter(IProxyConfigurator proxyConfigurator)
        {
            foreach (var pgc in configuration.Value.PropertyToGetterConfigurationMap)
            {
                if (!pgc.Value.Decorators.IsEmpty())
                {
                    proxyConfigurator.SetPropertyGetterDecorators(pgc.Key, pgc.Value.Decorators);
                }

                if (pgc.Value.Proxy != null)
                {
                    proxyConfigurator.SetPropertyGetterProxy(pgc.Key, pgc.Value.Proxy);
                }
            }
        }

        private void ConfigurePropertiesSetter(IProxyConfigurator proxyConfigurator)
        {
            foreach (var psc in configuration.Value.PropertyToSetterConfigurationMap)
            {
                if (!psc.Value.Decorators.IsEmpty())
                {
                    proxyConfigurator.SetPropertySetterDecorators(psc.Key, psc.Value.Decorators);
                }

                if (psc.Value.Proxy != null)
                {
                    proxyConfigurator.SetPropertySetterProxy(psc.Key, psc.Value.Proxy);
                }
            }
        }

        private void ConfigureEvents(IProxyConfigurator proxyConfigurator)
        {
            foreach (var es in configuration.Value.EventToConfigurationMap)
            {
                if (!es.Value.Decorators.IsEmpty())
                {
                    proxyConfigurator.SetEventDecorators(es.Key, es.Value.Decorators);
                }

                if (es.Value.Proxy != null)
                {
                    proxyConfigurator.SetEventProxy(es.Key, es.Value.Proxy);
                }
            }
        }

        private void ConfigureMethods(IProxyConfigurator proxyConfigurator)
        {
            foreach (var mc in configuration.Value.MethodToConfigurationMap)
            {
                if (!mc.Value.Decorators.IsEmpty())
                {
                    proxyConfigurator.SetMethodDecorators(mc.Key, mc.Value.Decorators);
                }

                if (mc.Value.Proxy != null)
                {
                    proxyConfigurator.SetMethodProxy(mc.Key, mc.Value.Proxy);
                }
            }
        }


        public void AddBeforePropertyGetterDecorator<TValue>(Func<T, TValue> pattern, Action decorator)
        {
            AddBeforePropertyGetterDecorator(ResolvePropertyGetterPattern(pattern), decorator);
        }

        public void AddAfterPropertyGetterDecorator<TValue>(Func<T, TValue> pattern, Action<TValue> decorator)
        {
            AddAfterPropertyGetterDecorator(ResolvePropertyGetterPattern(pattern), decorator);
        }

        public void AddPairPropertyGetterDecorators<TValue>(Func<T, TValue> pattern, Action beforeDecorator, Action<TValue> afterDecorator)
        {
            AddPairPropertyGetterDecorators(ResolvePropertyGetterPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void SetPropertyGetterProxy<TValue>(Func<T, TValue> pattern, Func<T, TValue> proxy)
        {
            SetPropertyGetterProxy(ResolvePropertyGetterPattern(pattern), proxy);
        }

        public void AddBeforePropertySetterDecorator<TValue>(Action<T, TValue> pattern, Action<TValue> decorator)
        {
            AddBeforePropertySetterDecorator(ResolvePropertySetterPattern(pattern), decorator);
        }

        public void AddAfterPropertySetterDecorator<TValue>(Action<T, TValue> pattern, Action<TValue> decorator)
        {
            AddAfterPropertySetterDecorator(ResolvePropertySetterPattern(pattern), decorator);
        }

        public void AddPairPropertySetterDecorators<TValue>(Action<T, TValue> pattern, Action<TValue> beforeDecorator, Action<TValue> afterDecorator)
        {
            AddPairPropertySetterDecorator(ResolvePropertySetterPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void SetPropertySetterProxy<TValue>(Action<T, TValue> pattern, Action<T, TValue> proxy)
        {
            SetPropertySetterProxy(ResolvePropertySetterPattern(pattern), proxy);
        }

        public void AddBeforeIndexerGetterDecorator<TIndex, TValue>(Func<T, TIndex, TValue> pattern, Action<TIndex> decorator)
        {
            AddBeforePropertyGetterDecorator(ResolveIndexerGetterPattern(pattern), decorator);
        }

        public void AddAfterIndexerGetterDecorator<TIndex, TValue>(Func<T, TIndex, TValue> pattern, Action<TIndex, TValue> decorator)
        {
            AddAfterPropertyGetterDecorator(ResolveIndexerGetterPattern(pattern), decorator);
        }

        public void AddPairIndexerGetterDecorators<TIndex, TValue>(Func<T, TIndex, TValue> pattern, Action<TIndex> beforeDecorator, Action<TIndex, TValue> afterDecorator)
        {
            AddPairPropertyGetterDecorators(ResolveIndexerGetterPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void SetIndexerGetterProxy<TIndex, TValue>(Func<T, TIndex, TValue> pattern, Func<T, TIndex, TValue> proxy)
        {
            SetPropertyGetterProxy(ResolveIndexerGetterPattern(pattern), proxy);
        }

        public void AddBeforeIndexerSetterDecorator<TIndex, TValue>(Action<T, TIndex, TValue> pattern, Action<TIndex, TValue> decorator)
        {
            AddBeforePropertySetterDecorator(ResolveIndexerSetterPattern(pattern), decorator);
        }

        public void AddAfterIndexerSetterDecorator<TIndex, TValue>(Action<T, TIndex, TValue> pattern, Action<TIndex, TValue> decorator)
        {
            AddAfterPropertySetterDecorator(ResolveIndexerSetterPattern(pattern), decorator);
        }

        public void AddPairIndexerSetterDecorators<TIndex, TValue>(Action<T, TIndex, TValue> pattern, Action<TIndex, TValue> beforeDecorator, Action<TIndex, TValue> afterDecorator)
        {
            AddPairPropertySetterDecorator(ResolveIndexerSetterPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void SetIndexerSetterProxy<TIndex, TValue>(Action<T, TIndex, TValue> pattern, Action<T, TIndex, TValue> proxy)
        {
            SetPropertySetterProxy(ResolveIndexerSetterPattern(pattern), proxy);
        }

        public void AddBeforeEventDecorator<TArgs>(Action<T, Action<object, TArgs>> pattern, Action<object, TArgs> decorator) where TArgs : EventArgs
        {
            AddBeforeEventDecorator(ResolveEventPattern(pattern), decorator);
        }

        public void AddAfterEventDecorator<TArgs>(Action<T, Action<object, TArgs>> pattern, Action<object, TArgs> decorator) where TArgs : EventArgs
        {
            AddAfterEventDecorator(ResolveEventPattern(pattern), decorator);
        }

        public void AddPairEventDecorators<TArgs>(Action<T, Action<object, TArgs>> pattern, Action<object, TArgs> beforeDecorator, Action<object, TArgs> afterDecorator) where TArgs : EventArgs
        {
            AddPairEventDecorators(ResolveEventPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void SetEventProxy<TArgs>(Action<T, Action<object, TArgs>> pattern, Action<Action<object, TArgs>, object, TArgs> decorator) where TArgs : EventArgs
        {
            SetEventProxy(ResolveEventPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator(Func<T, Action> pattern, Action decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1>(Func<T, Action<TArg1>> pattern, Action<TArg1> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2>(Func<T, Action<TArg1, TArg2>> pattern, Action<TArg1, TArg2> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2, TArg3>(Func<T, Action<TArg1, TArg2, TArg3>> pattern, Action<TArg1, TArg2, TArg3> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4>(Func<T, Action<TArg1, TArg2, TArg3, TArg4>> pattern, Action<TArg1, TArg2, TArg3, TArg4> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TReturn>(Func<T, Func<TReturn>> pattern, Action<TReturn> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TReturn>(Func<T, Func<TArg1, TReturn>> pattern, Action<TArg1, TReturn> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2, TReturn>(Func<T, Func<TArg1, TArg2, TReturn>> pattern, Action<TArg1, TArg2, TReturn> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TReturn> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TReturn> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn> decorator)
        {
            AddAfterMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator(Func<T, Action> pattern, Action decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1>(Func<T, Action<TArg1>> pattern, Action<TArg1> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2>(Func<T, Action<TArg1, TArg2>> pattern, Action<TArg1, TArg2> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2, TArg3>(Func<T, Action<TArg1, TArg2, TArg3>> pattern, Action<TArg1, TArg2, TArg3> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4>(Func<T, Action<TArg1, TArg2, TArg3, TArg4>> pattern, Action<TArg1, TArg2, TArg3, TArg4> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TReturn>(Func<T, Func<TReturn>> pattern, Action decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TReturn>(Func<T, Func<TArg1, TReturn>> pattern, Action<TArg1> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2, TReturn>(Func<T, Func<TArg1, TArg2, TReturn>> pattern, Action<TArg1, TArg2> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TReturn>> pattern, Action<TArg1, TArg2, TArg3> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> decorator)
        {
            AddBeforeMethodDecorator(ResolveMethodPattern(pattern), decorator);
        }

        public void AddPairMethodDecorators(Func<T, Action> pattern, Action beforeDecorator, Action afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1>(Func<T, Action<TArg1>> pattern, Action<TArg1> beforeDecorator, Action<TArg1> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2>(Func<T, Action<TArg1, TArg2>> pattern, Action<TArg1, TArg2> beforeDecorator, Action<TArg1, TArg2> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2, TArg3>(Func<T, Action<TArg1, TArg2, TArg3>> pattern, Action<TArg1, TArg2, TArg3> beforeDecorator, Action<TArg1, TArg2, TArg3> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2, TArg3, TArg4>(Func<T, Action<TArg1, TArg2, TArg3, TArg4>> pattern, Action<TArg1, TArg2, TArg3, TArg4> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2, TArg3, TArg4, TArg5>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TReturn>(Func<T, Func<TReturn>> pattern, Action beforeDecorator, Action<TReturn> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TReturn>(Func<T, Func<TArg1, TReturn>> pattern, Action<TArg1> beforeDecorator, Action<TArg1, TReturn> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2, TReturn>(Func<T, Func<TArg1, TArg2, TReturn>> pattern, Action<TArg1, TArg2> beforeDecorator, Action<TArg1, TArg2, TReturn> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2, TArg3, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TReturn>> pattern, Action<TArg1, TArg2, TArg3> beforeDecorator, Action<TArg1, TArg2, TArg3, TReturn> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2, TArg3, TArg4, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TReturn> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void AddPairMethodDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn> afterDecorator)
        {
            AddPairMethodDecorators(ResolveMethodPattern(pattern), beforeDecorator, afterDecorator);
        }

        public void SetMethodProxy(Func<T, Action> pattern, Action<T> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1>(Func<T, Action<Arg1>> pattern, Action<T, Arg1> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2>(Func<T, Action<Arg1, Arg2>> pattern, Action<T, Arg1, Arg2> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2, Arg3>(Func<T, Action<Arg1, Arg2, Arg3>> pattern, Action<T, Arg1, Arg2, Arg3> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2, Arg3, Arg4>(Func<T, Action<Arg1, Arg2, Arg3, Arg4>> pattern, Action<T, Arg1, Arg2, Arg3, Arg4> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5>(Func<T, Action<Arg1, Arg2, Arg3, Arg4, Arg5>> pattern, Action<T, Arg1, Arg2, Arg3, Arg4, Arg5> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6>(Func<T, Action<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6>> pattern, Action<T, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7>(Func<T, Action<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7>> pattern, Action<T, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8>(Func<T, Action<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8>> pattern, Action<T, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<TReturn>(Func<T, Func<TReturn>> pattern, Func<T, TReturn> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, TReturn>(Func<T, Func<Arg1, TReturn>> pattern, Func<T, Arg1, TReturn> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2, TReturn>(Func<T, Func<Arg1, Arg2, TReturn>> pattern, Func<T, Arg1, Arg2, TReturn> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2, Arg3, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, TReturn>> pattern, Func<T, Arg1, Arg2, Arg3, TReturn> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, TReturn>> pattern, Func<T, Arg1, Arg2, Arg3, Arg4, TReturn> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, Arg5, TReturn>> pattern, Func<T, Arg1, Arg2, Arg3, Arg4, Arg5, TReturn> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, TReturn>> pattern, Func<T, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, TReturn> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, TReturn>> pattern, Func<T, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, TReturn> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }

        public void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, TReturn>> pattern, Func<T, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, TReturn> proxy)
        {
            SetMethodProxy(ResolveMethodPattern(pattern), proxy);
        }
        
        private PropertyInfo ResolvePropertyGetterPattern<TValue>(Func<T, TValue> pattern)
        {
            (object instance, IPropertyGetterDetector detector) = typeRepository.CreatePropertyGetterDetector(type);
            pattern.Invoke((T)instance);
            return detector.GetDetectedProperty();
        }

        private PropertyInfo ResolvePropertySetterPattern<TValue>(Action<T, TValue> pattern)
        {
            (object instance, IPropertySetterDetector detector) = typeRepository.CreatePropertySetterDetector(type);
            pattern.Invoke((T)instance, default);
            return detector.GetDetectedProperty();
        }

        private PropertyInfo ResolveIndexerGetterPattern<TIndex, TValue>(Func<T, TIndex, TValue> pattern)
        {
            (object instance, IPropertyGetterDetector detector) = typeRepository.CreatePropertyGetterDetector(type);
            pattern.Invoke((T)instance, default);
            return detector.GetDetectedProperty();
        }

        private PropertyInfo ResolveIndexerSetterPattern<TIndex, TValue>(Action<T, TIndex, TValue> pattern)
        {
            (object instance, IPropertySetterDetector detector) = typeRepository.CreatePropertySetterDetector(type);
            pattern.Invoke((T)instance, default, default);
            return detector.GetDetectedProperty();
        }

        private EventInfo ResolveEventPattern<TArgs>(Action<T, Action<object, TArgs>> pattern) where TArgs : EventArgs
        {
            (object instance, IEventDetector detector) = typeRepository.CreateEventDetector(type);
            pattern.Invoke((T)instance, (o, a) => { });
            return detector.GetDetectedEvent();
        }

        private MethodInfo ResolveMethodPattern(Func<T, Delegate> pattern)
        {
            (object instance, IMethodDetector detector) = typeRepository.CreateMethodDetector(type);
            var token = pattern.Invoke((T)instance);
            return detector.GetDetectedMethod(token);
        }

        private void AddBeforePropertyGetterDecorator(PropertyInfo propertyInfo, object decorator)
        {
            configuration.Value.PropertyToGetterConfigurationMap[propertyInfo].Decorators.Add(new ValueTuple<object, object>(decorator, null));
        }

        private void AddAfterPropertyGetterDecorator(PropertyInfo propertyInfo, object decorator)
        {
            configuration.Value.PropertyToGetterConfigurationMap[propertyInfo].Decorators.Add(new ValueTuple<object, object>(null, decorator));
        }

        private void AddPairPropertyGetterDecorators(PropertyInfo propertyInfo, object beforeDecorator, object afterDecorator)
        {
            configuration.Value.PropertyToGetterConfigurationMap[propertyInfo].Decorators.Add(new ValueTuple<object, object>(beforeDecorator, afterDecorator));
        }

        private void SetPropertyGetterProxy(PropertyInfo propertyInfo, object proxy)
        {
            configuration.Value.PropertyToGetterConfigurationMap[propertyInfo].Proxy = proxy;
        }

        private void AddBeforePropertySetterDecorator(PropertyInfo propertyInfo, object decorator)
        {
            configuration.Value.PropertyToSetterConfigurationMap[propertyInfo].Decorators.Add(new ValueTuple<object, object>(decorator, null));
        }

        private void AddAfterPropertySetterDecorator(PropertyInfo propertyInfo, object decorator)
        {
            configuration.Value.PropertyToSetterConfigurationMap[propertyInfo].Decorators.Add(new ValueTuple<object, object>(null, decorator));
        }

        private void AddPairPropertySetterDecorator(PropertyInfo propertyInfo, object beforeDecorator, object afterDecorator)
        {
            configuration.Value.PropertyToSetterConfigurationMap[propertyInfo].Decorators.Add(new ValueTuple<object, object>(beforeDecorator, afterDecorator));
        }

        private void SetPropertySetterProxy(PropertyInfo propertyInfo, object proxy)
        {
            configuration.Value.PropertyToSetterConfigurationMap[propertyInfo].Proxy = proxy;
        }

        private void AddBeforeEventDecorator(EventInfo eventInfo, object decorator)
        {
            configuration.Value.EventToConfigurationMap[eventInfo].Decorators.Add(new ValueTuple<object, object>(decorator, null));
        }

        private void AddAfterEventDecorator(EventInfo eventInfo, object decorator)
        {
            configuration.Value.EventToConfigurationMap[eventInfo].Decorators.Add(new ValueTuple<object, object>(null, decorator));
        }

        private void AddPairEventDecorators(EventInfo eventInfo, object beforeDecorator, object afterDecorator)
        {
            configuration.Value.EventToConfigurationMap[eventInfo].Decorators.Add(new ValueTuple<object, object>(beforeDecorator, afterDecorator));
        }

        private void SetEventProxy(EventInfo eventInfo, object proxy)
        {
            configuration.Value.EventToConfigurationMap[eventInfo].Proxy = proxy;
        }

        private void AddBeforeMethodDecorator(MethodInfo methodInfo, object decorator)
        {
            configuration.Value.MethodToConfigurationMap[methodInfo].Decorators.Add(new ValueTuple<object, object>(decorator, null));
        }

        private void AddAfterMethodDecorator(MethodInfo methodInfo, object decorator)
        {
            configuration.Value.MethodToConfigurationMap[methodInfo].Decorators.Add(new ValueTuple<object, object>(null, decorator));
        }

        private void AddPairMethodDecorators(MethodInfo methodInfo, object beforeDecorator, object afterDecorator)
        {
            configuration.Value.MethodToConfigurationMap[methodInfo].Decorators.Add(new ValueTuple<object, object>(beforeDecorator, afterDecorator));
        }

        private void SetMethodProxy(MethodInfo methodInfo, object proxy)
        {
            configuration.Value.MethodToConfigurationMap[methodInfo].Proxy = proxy;
        }

        private static TypeConfiguration InitializeTypeWrapSettings(Type type)
        {
            return new TypeConfiguration(
                eventToConfigurationMap: type.GetEvents(BindingFlags.Instance | BindingFlags.Public).ToReadOnlyDictionary(e => e, e => new MemberConfiguration()),
                methodToConfigurationMap: type.GetMethods(BindingFlags.Instance | BindingFlags.Public).ToReadOnlyDictionary(m => m, m => new MemberConfiguration()),
                propertyToGetterConfigurationMap: type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty).ToReadOnlyDictionary(p => p, p => new MemberConfiguration()),
                propertyToSetterConfigurationMap: type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty).ToReadOnlyDictionary(p => p, p => new MemberConfiguration()));
        }

        private static Type ThrowIfTypeIsNotAnInterfaceElseReturn(Type type)
        {
            if (!type.IsInterface)
            {
                throw new InvalidOperationException($"Following type must be an interface: {type.FullName}");
            }
            return type;
        }

        private class TypeConfiguration
        {
            public TypeConfiguration(
                ReadOnlyDictionary<EventInfo, MemberConfiguration> eventToConfigurationMap,
                ReadOnlyDictionary<MethodInfo, MemberConfiguration> methodToConfigurationMap,
                ReadOnlyDictionary<PropertyInfo, MemberConfiguration> propertyToGetterConfigurationMap,
                ReadOnlyDictionary<PropertyInfo, MemberConfiguration> propertyToSetterConfigurationMap)
            {
                EventToConfigurationMap = eventToConfigurationMap;
                MethodToConfigurationMap = methodToConfigurationMap;
                PropertyToGetterConfigurationMap = propertyToGetterConfigurationMap;
                PropertyToSetterConfigurationMap = propertyToSetterConfigurationMap;
            }

            public ReadOnlyDictionary<EventInfo, MemberConfiguration> EventToConfigurationMap { get; }
            public ReadOnlyDictionary<MethodInfo, MemberConfiguration> MethodToConfigurationMap { get; }
            public ReadOnlyDictionary<PropertyInfo, MemberConfiguration> PropertyToGetterConfigurationMap { get; }
            public ReadOnlyDictionary<PropertyInfo, MemberConfiguration> PropertyToSetterConfigurationMap { get; }
        }

        private class MemberConfiguration
        {
            public MemberConfiguration()
            {
                Decorators = new List<(object, object)>();
            }

            public object Proxy { get; set; }
            public List<ValueTuple<object, object>> Decorators { get; }
        }
    }
}
