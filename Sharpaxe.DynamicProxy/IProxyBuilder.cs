using System;

namespace Sharpaxe.DynamicProxy
{
    public interface IProxyBuilder<T>
        where T : class
    {
        T Build(T instance);

        void AddBeforePropertyGetterDecorator<TValue>(Func<T, TValue> pattern, Action decorator);
        void AddAfterPropertyGetterDecorator<TValue>(Func<T, TValue> pattern, Action<TValue> decorator);
        void SetPropertyGetterProxy<TValue>(Func<T, TValue> pattern, Func<T, TValue> proxy);

        void AddBeforePropertySetterDecorator<TValue>(Action<T, TValue> pattern, Action<TValue> decorator);
        void AddAfterPropertySetterDecorator<TValue>(Action<T, TValue> pattern, Action decorator);
        void SetPropertySetterProxy<TValue>(Action<T, TValue> pattern, Action<T, TValue> proxy);

        void AddBeforeIndexerGetterDecorator<TIndex, TValue>(Func<T, TIndex, TValue> pattern, Action<TIndex> decorator);
        void AddAfterIndexerGetterDecorator<TIndex, TValue>(Func<T, TIndex, TValue> pattern, Action<TIndex, TValue> decorator);
        void SetIndexerGetterProxy<TIndex, TValue>(Func<T, TIndex, TValue> pattern, Func<T, TIndex, TValue> proxy);

        void AddBeforeIndexerSetterDecorator<TIndex, TValue>(Action<T, TIndex, TValue> pattern, Action<TIndex, TValue> decorator);
        void AddAfterIndexerSetterDecorator<TIndex, TValue>(Action<T, TIndex, TValue> pattern, Action<TIndex> decorator);
        void SetIndexerSetterProxy<TIndex, TValue>(Action<T, TIndex, TValue> pattern, Action<T, TIndex, TValue> proxy);

        void AddBeforeEventDecorator<TArgs>(Action<T, Action<object, TArgs>> pattern, Action<object, TArgs> decorator) where TArgs : EventArgs;
        void AddAfterEventDecorator<TArgs>(Action<T, Action<object, TArgs>> pattern, Action<object, TArgs> decorator) where TArgs : EventArgs;
        void SetEventProxy<TArgs>(Action<T, Action<object, TArgs>> pattern, Action<Action<object, TArgs>, object, TArgs> decorator) where TArgs : EventArgs;

        void AddBeforeMethodDecorator(Func<T, Action> pattern, Action decorator);
        void AddBeforeMethodDecorator<TArg1>(Func<T, Action<TArg1>> pattern, Action<TArg1> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2>(Func<T, Action<TArg1, TArg2>> pattern, Action<TArg1, TArg2> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2, TArg3>(Func<T, Action<TArg1, TArg2, TArg3>> pattern, Action<TArg1, TArg2, TArg3> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4>(Func<T, Action<TArg1, TArg2, TArg3, TArg4>> pattern, Action<TArg1, TArg2, TArg3, TArg4> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> decorator);

        void AddBeforeMethodDecorator<TReturn>(Func<T, Func<TReturn>> pattern, Action decorator);
        void AddBeforeMethodDecorator<TArg1, TReturn>(Func<T, Func<TArg1, TReturn>> pattern, Action<TArg1> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2, TReturn>(Func<T, Func<TArg1, TArg2, TReturn>> pattern, Action<TArg1, TArg2> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TReturn>> pattern, Action<TArg1, TArg2, TArg3> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> decorator);
        void AddBeforeMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> decorator);

        void AddAfterMethodDecorator(Func<T, Action> pattern, Action decorator);
        void AddAfterMethodDecorator<TArg1>(Func<T, Action<TArg1>> pattern, Action<TArg1> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2>(Func<T, Action<TArg1, TArg2>> pattern, Action<TArg1, TArg2> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2, TArg3>(Func<T, Action<TArg1, TArg2, TArg3>> pattern, Action<TArg1, TArg2, TArg3> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4>(Func<T, Action<TArg1, TArg2, TArg3, TArg4>> pattern, Action<TArg1, TArg2, TArg3, TArg4> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> decorator);

        void AddAfterMethodDecorator<TReturn>(Func<T, Func<TReturn>> pattern, Action<TReturn> decorator);
        void AddAfterMethodDecorator<TArg1, TReturn>(Func<T, Func<TArg1, TReturn>> pattern, Action<TArg1, TReturn> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2, TReturn>(Func<T, Func<TArg1, TArg2, TReturn>> pattern, Action<TArg1, TArg2, TReturn> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TReturn> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TReturn> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn> decorator);
        void AddAfterMethodDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn> decorator);

        void SetMethodProxy(Func<T, Action> pattern, Action<T> proxy);
        void SetMethodProxy<Arg1>(Func<T, Action<Arg1>> pattern, Action<T, Arg1> proxy);
        void SetMethodProxy<Arg1, Arg2>(Func<T, Action<Arg1, Arg2>> pattern, Action<T, Arg1, Arg2> proxy);
        void SetMethodProxy<Arg1, Arg2, Arg3>(Func<T, Action<Arg1, Arg2, Arg3>> pattern, Action<T, Arg1, Arg2, Arg3> proxy);
        void SetMethodProxy<Arg1, Arg2, Arg3, Arg4>(Func<T, Action<Arg1, Arg2, Arg3, Arg4>> pattern, Action<T, Arg1, Arg2, Arg3, Arg4> proxy);
        void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5>(Func<T, Action<Arg1, Arg2, Arg3, Arg4, Arg5>> pattern, Action<T, Arg1, Arg2, Arg3, Arg4, Arg5> proxy);
        void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6>(Func<T, Action<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6>> pattern, Action<T, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6> proxy);
        void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7>(Func<T, Action<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7>> pattern, Action<T, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7> proxy);
        void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8>(Func<T, Action<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8>> pattern, Action<T, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8> proxy);

        void SetMethodProxy<TReturn>(Func<T, Func<TReturn>> pattern, Func<T, TReturn> proxy);
        void SetMethodProxy<Arg1, TReturn>(Func<T, Func<Arg1, TReturn>> pattern, Func<T, Arg1, TReturn> proxy);
        void SetMethodProxy<Arg1, Arg2, TReturn>(Func<T, Func<Arg1, Arg2, TReturn>> pattern, Func<T, Arg1, Arg2, TReturn> proxy);
        void SetMethodProxy<Arg1, Arg2, Arg3, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, TReturn>> pattern, Func<T, Arg1, Arg2, Arg3, TReturn> proxy);
        void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, TReturn>> pattern, Func<T, Arg1, Arg2, Arg3, Arg4, TReturn> proxy);
        void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, Arg5, TReturn>> pattern, Func<T, Arg1, Arg2, Arg3, Arg4, Arg5, TReturn> proxy);
        void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, TReturn>> pattern, Func<T, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, TReturn> proxy);
        void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, TReturn>> pattern, Func<T, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, TReturn> proxy);
        void SetMethodProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, TReturn>> pattern, Func<T, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, TReturn> proxy);
    }
}
