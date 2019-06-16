namespace Sharpaxe.DynamicProxy.Tests.TestHelper
{
#warning Should be an internal interface - change it after skip clr visibility check has been done
    public interface IMethod
    {
        void Action();
        void ActionWithValueArgument(int arg);
        void ActionWithValueArgument(uint arg);
        void ActionWithReferenceArgument(object arg);
        void ActionWithReferenceArgument(string arg);
#warning Methods with ref and out paramerters are not supported
        //void ActionWithRefAndOutArguments(ref int arg1, ref object arg2, out int arg3, out object arg4);

        int FunctionWithValueReturnType();
        int FunctionWithValueArgumentAndValueReturnType(int argument);
        int FunctionWithValueArgumentAndValueReturnType(uint argument);
        int FunctionWithReferenceArgumentAndValueReturnType(object argument);
        int FunctionWithReferenceArgumentAndValueReturnType(string argument);
#warning Methods with ref and out paramerters are not supported
        //int FunctionWithRefAndOutArgumentsAndValueReturnType(ref int arg1, ref object arg2, out int arg3, out object arg4);

        object FunctionWithReferenceReturnType();
        object FunctionWithValueArgumentAndReferenceReturnType(int argument);
        object FunctionWithValueArgumentAndReferenceReturnType(uint argument);
        object FunctionWithReferenceArgumentAndReferenceReturnType(object argument);
        object FunctionWithReferenceArgumentAndReferenceReturnType(string argument);
#warning Methods with ref and out paramerters are not supported
        //object FunctionWithRefAndOutArgumentsAndReferenceReturnType(ref int arg1, ref object arg2, out int arg3, out object arg4);

        (int, object) FunctionWithTupleReturnType();
        (int, object) FunctionWithValueArgumentAndTupleReturnType(int argument);
        (int, object) FunctionWithValueArgumentAndTupleReturnType(uint argument);
        (int, object) FunctionWithReferenceArgumentAndTupleReturnType(object argument);
        (int, object) FunctionWithReferenceArgumentAndTupleReturnType(string argument);
#warning Methods with ref and out paramerters are not supported
        //(int, object) FunctionWithRefAndOutArgumentsAndTupleReturnType(ref int arg1, ref object arg2, out int arg3, out object arg4);
    }
}
