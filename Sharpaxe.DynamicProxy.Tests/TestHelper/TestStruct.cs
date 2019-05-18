using System.Threading;

namespace Sharpaxe.DynamicProxy.Tests.TestHelper
{
#warning Should be an internal interface - change it after skip clr visibility check has been done
    public struct TestStruct
    {
        public bool     BoolenField;
        public byte     ByteField;
        public sbyte    SByteField;
        public char     CharField;
        public decimal  DecimalField;
        public double   DoubleField;
        public float    FloatField;
        public int      IntField;
        public uint     UIntField;
        public long     LongField;
        public short    ShortField;
        public ushort   UShortField;
        public string   ReferenceField;

        public CancellationToken StructField;
    }
}
