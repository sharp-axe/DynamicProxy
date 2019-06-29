using System;

namespace Sharpaxe.DynamicProxy.Tests.TestHelper
{
    public class TestException : Exception
    {
        public TestException()
            : this(0)
        {
        }

        public TestException(int id)
            : base()
        {
            Id = id;
        }

        public int Id { get; }
    }
}
