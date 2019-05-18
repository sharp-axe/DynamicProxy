namespace Sharpaxe.DynamicProxy.Tests.TestHelper
{
#warning Should be an internal interface - change it after skip clr visibility check has been done
    public interface IPropertyGetter
    {
        bool       BoleanPropertyWithGetter    { get; }
        byte       BytePropertyWithGetter      { get; }
        sbyte      SBytePropertyWithGetter     { get; }
        char       CharPropertyWithGetter      { get; }
        decimal    DecimalPropertyWithGetter   { get; }
        double     DoublePropertyWithGetter    { get; }
        float      FloatPropertyWithGetter     { get; }
        int        IntPropertyWithGetter       { get; }
        uint       UIntPropertyWithGetter      { get; }
        long       LongPropertyWithGetter      { get; }
        ulong      ULongPropertyWithGetter     { get; }
        short      ShortPropertyWithGetter     { get; }
        ushort     UShortPropertyWithGetter    { get; }
        TestStruct StructPropertyWithGetter    { get; }
        string     ReferencePropertyWithGetter { get; } 
    }
}
