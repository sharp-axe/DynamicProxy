namespace Sharpaxe.DynamicProxy.Tests.TestHelper
{
#warning Should be an internal interface - change it after skip clr visibility check has been done
    public interface IPropertyGetter
    {
        bool       Boolean { get; }
        byte       Byte    { get; }
        sbyte      SByte   { get; }
        char       Char    { get; }
        decimal    Decimal { get; }
        double     Double  { get; }
        float      Float   { get; }
        int        Int     { get; }
        uint       UInt    { get; }
        long       Long    { get; }
        ulong      ULong   { get; }
        short      Short   { get; }
        ushort     UShort  { get; }
        TestStruct Struct  { get; }
        string     Class   { get; } 
    }
}
