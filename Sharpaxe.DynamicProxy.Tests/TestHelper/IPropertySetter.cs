namespace Sharpaxe.DynamicProxy.Tests.TestHelper
{
#warning Should be an internal interface - change it after skip clr visibility check has been done
    public interface IPropertySetter
    {
        bool       Boolean { set; }
        byte       Byte    { set; }
        sbyte      SByte   { set; }
        char       Char    { set; }
        decimal    Decimal { set; }
        double     Double  { set; }
        float      Float   { set; }
        int        Int     { set; }
        uint       UInt    { set; }
        long       Long    { set; }
        ulong      ULong   { set; }
        short      Short   { set; }
        ushort     UShort  { set; }
        TestStruct Struct  { set; }
        string     Class   { set; }    
    }
}