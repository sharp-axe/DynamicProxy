using System;

namespace Sharpaxe.DynamicProxy
{
    public interface IProxyBuilder<T>
        where T : class
    {
        T Build(T instance);

        void AddBeforePropertyGetterDecorator<TValue>(Func<T, TValue> pattern, Action decorator);
        void AddAfterPropertyGetterDecorator<TValue>(Func<T, TValue> pattern, Action<TValue> decorator);
        void AddPairPropertyGetterDecorators<TValue>(Func<T, TValue> pattern, Action beforeDecorator, Action<TValue> afterDecorator);
        void SetPropertyGetterProxy<TValue>(Func<T, TValue> pattern, Func<Func<TValue>, TValue> proxy);

        void AddBeforePropertySetterDecorator<TValue>(Action<T, TValue> pattern, Action<TValue> decorator);
        void AddAfterPropertySetterDecorator<TValue>(Action<T, TValue> pattern, Action<TValue> decorator);
        void AddPairPropertySetterDecorators<TValue>(Action<T, TValue> pattern, Action<TValue> beforeDecorator, Action<TValue> afterDecorator);
        void SetPropertySetterProxy<TValue>(Action<T, TValue> pattern, Action<Action<TValue>, TValue> proxy);

        void AddBeforeIndexerGetterDecorator<TIndex, TValue>(Func<T, TIndex, TValue> pattern, Action<TIndex> decorator);
        void AddAfterIndexerGetterDecorator<TIndex, TValue>(Func<T, TIndex, TValue> pattern, Action<TIndex, TValue> decorator);
        void AddPairIndexerGetterDecorators<TIndex, TValue>(Func<T, TIndex, TValue> pattern, Action<TIndex> beforeDecorator, Action<TIndex, TValue> afterDecorator);
        void SetIndexerGetterProxy<TIndex, TValue>(Func<T, TIndex, TValue> pattern, Func<Func<TIndex, TValue>, TIndex, TValue> proxy);

        void AddBeforeIndexerSetterDecorator<TIndex, TValue>(Action<T, TIndex, TValue> pattern, Action<TIndex, TValue> decorator);
        void AddAfterIndexerSetterDecorator<TIndex, TValue>(Action<T, TIndex, TValue> pattern, Action<TIndex, TValue> decorator);
        void AddPairIndexerSetterDecorators<TIndex, TValue>(Action<T, TIndex, TValue> pattern, Action<TIndex, TValue> beforeDecorator, Action<TIndex, TValue> afterDecorator);
        void SetIndexerSetterProxy<TIndex, TValue>(Action<T, TIndex, TValue> pattern, Action<Action<TIndex, TValue>, TIndex, TValue> proxy);

        void AddBeforeEventDecorator<TArgs>(Action<T, Action<object, TArgs>> pattern, Action<object, TArgs> decorator) where TArgs : EventArgs;
        void AddAfterEventDecorator<TArgs>(Action<T, Action<object, TArgs>> pattern, Action<object, TArgs> decorator) where TArgs : EventArgs;
        void AddPairEventDecorators<TArgs>(Action<T, Action<object, TArgs>> pattern, Action<object, TArgs> beforeDecorator, Action<object, TArgs> afterDecorator) where TArgs : EventArgs;
        void SetEventProxy<TArgs>(Action<T, Action<object, TArgs>> pattern, Action<Action<object, TArgs>, object, TArgs> decorator) where TArgs : EventArgs;

        void AddBeforeActionDecorator(Func<T, Action> pattern, Action decorator);
        void AddBeforeActionDecorator<TArg1>(Func<T, Action<TArg1>> pattern, Action<TArg1> decorator);
        void AddBeforeActionDecorator<TArg1, TArg2>(Func<T, Action<TArg1, TArg2>> pattern, Action<TArg1, TArg2> decorator);
        void AddBeforeActionDecorator<TArg1, TArg2, TArg3>(Func<T, Action<TArg1, TArg2, TArg3>> pattern, Action<TArg1, TArg2, TArg3> decorator);
        void AddBeforeActionDecorator<TArg1, TArg2, TArg3, TArg4>(Func<T, Action<TArg1, TArg2, TArg3, TArg4>> pattern, Action<TArg1, TArg2, TArg3, TArg4> decorator);
        void AddBeforeActionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5> decorator);
        void AddBeforeActionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> decorator);
        void AddBeforeActionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> decorator);
        void AddBeforeActionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> decorator);

        void AddBeforeFunctionDecorator<TReturn>(Func<T, Func<TReturn>> pattern, Action decorator);
        void AddBeforeFunctionDecorator<TArg1, TReturn>(Func<T, Func<TArg1, TReturn>> pattern, Action<TArg1> decorator);
        void AddBeforeFunctionDecorator<TArg1, TArg2, TReturn>(Func<T, Func<TArg1, TArg2, TReturn>> pattern, Action<TArg1, TArg2> decorator);
        void AddBeforeFunctionDecorator<TArg1, TArg2, TArg3, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TReturn>> pattern, Action<TArg1, TArg2, TArg3> decorator);
        void AddBeforeFunctionDecorator<TArg1, TArg2, TArg3, TArg4, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4> decorator);
        void AddBeforeFunctionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5> decorator);
        void AddBeforeFunctionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> decorator);
        void AddBeforeFunctionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> decorator);
        void AddBeforeFunctionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> decorator);

        void AddAfterActionDecorator(Func<T, Action> pattern, Action decorator);
        void AddAfterActionDecorator<TArg1>(Func<T, Action<TArg1>> pattern, Action<TArg1> decorator);
        void AddAfterActionDecorator<TArg1, TArg2>(Func<T, Action<TArg1, TArg2>> pattern, Action<TArg1, TArg2> decorator);
        void AddAfterActionDecorator<TArg1, TArg2, TArg3>(Func<T, Action<TArg1, TArg2, TArg3>> pattern, Action<TArg1, TArg2, TArg3> decorator);
        void AddAfterActionDecorator<TArg1, TArg2, TArg3, TArg4>(Func<T, Action<TArg1, TArg2, TArg3, TArg4>> pattern, Action<TArg1, TArg2, TArg3, TArg4> decorator);
        void AddAfterActionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5> decorator);
        void AddAfterActionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> decorator);
        void AddAfterActionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> decorator);
        void AddAfterActionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> decorator);

        void AddAfterFunctionDecorator<TReturn>(Func<T, Func<TReturn>> pattern, Action<TReturn> decorator);
        void AddAfterFunctionDecorator<TArg1, TReturn>(Func<T, Func<TArg1, TReturn>> pattern, Action<TArg1, TReturn> decorator);
        void AddAfterFunctionDecorator<TArg1, TArg2, TReturn>(Func<T, Func<TArg1, TArg2, TReturn>> pattern, Action<TArg1, TArg2, TReturn> decorator);
        void AddAfterFunctionDecorator<TArg1, TArg2, TArg3, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TReturn> decorator);
        void AddAfterFunctionDecorator<TArg1, TArg2, TArg3, TArg4, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TReturn> decorator);
        void AddAfterFunctionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn> decorator);
        void AddAfterFunctionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn> decorator);
        void AddAfterFunctionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn> decorator);
        void AddAfterFunctionDecorator<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn> decorator);

        void AddPairActionDecorators(Func<T, Action> pattern, Action beforeDecorator, Action afterDecorator);
        void AddPairActionDecorators<TArg1>(Func<T, Action<TArg1>> pattern, Action<TArg1> beforeDecorator, Action<TArg1> afterDecorator);
        void AddPairActionDecorators<TArg1, TArg2>(Func<T, Action<TArg1, TArg2>> pattern, Action<TArg1, TArg2> beforeDecorator, Action<TArg1, TArg2> afterDecorator);
        void AddPairActionDecorators<TArg1, TArg2, TArg3>(Func<T, Action<TArg1, TArg2, TArg3>> pattern, Action<TArg1, TArg2, TArg3> beforeDecorator, Action<TArg1, TArg2, TArg3> afterDecorator);
        void AddPairActionDecorators<TArg1, TArg2, TArg3, TArg4>(Func<T, Action<TArg1, TArg2, TArg3, TArg4>> pattern, Action<TArg1, TArg2, TArg3, TArg4> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4> afterDecorator);
        void AddPairActionDecorators<TArg1, TArg2, TArg3, TArg4, TArg5>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5> afterDecorator);
        void AddPairActionDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> afterDecorator);
        void AddPairActionDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> afterDecorator);
        void AddPairActionDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(Func<T, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> afterDecorator);

        void AddPairFunctionDecorators<TReturn>(Func<T, Func<TReturn>> pattern, Action beforeDecorator, Action<TReturn> afterDecorator);
        void AddPairFunctionDecorators<TArg1, TReturn>(Func<T, Func<TArg1, TReturn>> pattern, Action<TArg1> beforeDecorator, Action<TArg1, TReturn> afterDecorator);
        void AddPairFunctionDecorators<TArg1, TArg2, TReturn>(Func<T, Func<TArg1, TArg2, TReturn>> pattern, Action<TArg1, TArg2> beforeDecorator, Action<TArg1, TArg2, TReturn> afterDecorator);
        void AddPairFunctionDecorators<TArg1, TArg2, TArg3, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TReturn>> pattern, Action<TArg1, TArg2, TArg3> beforeDecorator, Action<TArg1, TArg2, TArg3, TReturn> afterDecorator);
        void AddPairFunctionDecorators<TArg1, TArg2, TArg3, TArg4, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TReturn> afterDecorator);
        void AddPairFunctionDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TReturn> afterDecorator);
        void AddPairFunctionDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TReturn> afterDecorator);
        void AddPairFunctionDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TReturn> afterDecorator);
        void AddPairFunctionDecorators<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>(Func<T, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn>> pattern, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> beforeDecorator, Action<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TReturn> afterDecorator);

        void SetActionProxy(Func<T, Action> pattern, Action<Action> proxy);
        void SetActionProxy<Arg1>(Func<T, Action<Arg1>> pattern, Action<Action<Arg1>, Arg1> proxy);
        void SetActionProxy<Arg1, Arg2>(Func<T, Action<Arg1, Arg2>> pattern, Action<Action<Arg1, Arg2>, Arg1, Arg2> proxy);
        void SetActionProxy<Arg1, Arg2, Arg3>(Func<T, Action<Arg1, Arg2, Arg3>> pattern, Action<Action<Arg1, Arg2, Arg3>, Arg1, Arg2, Arg3> proxy);
        void SetActionProxy<Arg1, Arg2, Arg3, Arg4>(Func<T, Action<Arg1, Arg2, Arg3, Arg4>> pattern, Action<Action<Arg1, Arg2, Arg3, Arg4>, Arg1, Arg2, Arg3, Arg4> proxy);
        void SetActionProxy<Arg1, Arg2, Arg3, Arg4, Arg5>(Func<T, Action<Arg1, Arg2, Arg3, Arg4, Arg5>> pattern, Action<Action<Arg1, Arg2, Arg3, Arg4, Arg5>, Arg1, Arg2, Arg3, Arg4, Arg5> proxy);
        void SetActionProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6>(Func<T, Action<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6>> pattern, Action<Action<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6>, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6> proxy);
        void SetActionProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7>(Func<T, Action<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7>> pattern, Action<Action<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7>, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7> proxy);
        void SetActionProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8>(Func<T, Action<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8>> pattern, Action<Action<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8>, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8> proxy);

        void SetFunctionProxy<TReturn>(Func<T, Func<TReturn>> pattern, Func<Func<TReturn>, TReturn> proxy);
        void SetFunctionProxy<Arg1, TReturn>(Func<T, Func<Arg1, TReturn>> pattern, Func<Func<Arg1, TReturn>, Arg1, TReturn> proxy);
        void SetFunctionProxy<Arg1, Arg2, TReturn>(Func<T, Func<Arg1, Arg2, TReturn>> pattern, Func<Func<Arg1, Arg2, TReturn>, Arg1, Arg2, TReturn> proxy);
        void SetFunctionProxy<Arg1, Arg2, Arg3, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, TReturn>> pattern, Func<Func<Arg1, Arg2, Arg3, TReturn>, Arg1, Arg2, Arg3, TReturn> proxy);
        void SetFunctionProxy<Arg1, Arg2, Arg3, Arg4, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, TReturn>> pattern, Func<Func<Arg1, Arg2, Arg3, Arg4, TReturn>, Arg1, Arg2, Arg3, Arg4, TReturn> proxy);
        void SetFunctionProxy<Arg1, Arg2, Arg3, Arg4, Arg5, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, Arg5, TReturn>> pattern, Func<Func<Arg1, Arg2, Arg3, Arg4, Arg5, TReturn>, Arg1, Arg2, Arg3, Arg4, Arg5, TReturn> proxy);
        void SetFunctionProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, TReturn>> pattern, Func<Func<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, TReturn>, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, TReturn> proxy);
        void SetFunctionProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, TReturn>> pattern, Func<Func<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, TReturn>, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, TReturn> proxy);
        void SetFunctionProxy<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, TReturn>(Func<T, Func<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, TReturn>> pattern, Func<Func<Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, TReturn>, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, TReturn> proxy);
    }
}
