using System;
using NUnit.Framework;

using Genetics;

namespace GeneticsTests
{
    public class TestBase
    {
        [SetUp]
        public virtual void Setup()
        {
            Geneticist.Debug = true;
            Geneticist.DebugTextWriter = Console.Out;
            Geneticist.ThrowOnError = true;
        }

        [TearDown]
        public virtual void Tear()
        {
        }
    }
}
