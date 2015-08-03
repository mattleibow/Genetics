using System;
using System.Linq;
using Android.App;
using Android.Graphics;
using NUnit.Framework;

using Syringe;
using SyringeTests.TestCases;

namespace SyringeTests
{
    [TestFixture]
    public class ArrayResourceInjectionTests
    {
        [SetUp]
        public void Setup()
        {
            Needle.Debug = true;
            Needle.DebugTextWriter = Console.Out;
            Needle.ThrowOnError = true;
        }

        [TearDown]
        public void Tear()
        {
        }

        [Test]
        public void TypeMappingCreatedForAllSupportedMembers()
        {
            var mapping = Needle.GetTypeMapping(typeof(ArraysTargetObject));
            Assert.AreEqual(3, mapping.Members.Values.Count);

            mapping = Needle.GetTypeMapping(typeof(ArrayTypeTargetObject));
            Assert.AreEqual(5, mapping.Members.Values.Count);

            mapping = Needle.GetTypeMapping(typeof(InvalidArrayTypeTargetObject));
            Assert.AreEqual(1, mapping.Members.Values.Count);

            mapping = Needle.GetTypeMapping(typeof(InvalidTypeTargetObject));
            Assert.AreEqual(1, mapping.Members.Values.Count);
        }

        [Test]
        public void SimpleArrayInjected()
        {
            var target = new ArraysTargetObject();
            Needle.Inject(target, null, Application.Context);

            var intArray = target.IntegerArrayProperty;
            Assert.IsNotNull(intArray);
            Assert.AreEqual(1, intArray[0]);
            Assert.AreEqual(2, intArray[1]);
            Assert.AreEqual(3, intArray[2]);

            var stringArray = target.StringArrayProperty;
            Assert.IsNotNull(stringArray);
            Assert.AreEqual("StringArray-1", stringArray[0]);
            Assert.AreEqual("StringArray-2", stringArray[1]);
            Assert.AreEqual("StringArray-3", stringArray[2]);
        }

        [Test]
        public void VariousCollectionTypesInjected()
        {
            var target = new ArrayTypeTargetObject();
            Needle.Inject(target, null, Application.Context);

            var integerArrays = new[] 
            {
                target.IntegerArray,
                target.IntegerEnumerable.ToArray(),
                target.IntegerInterfaceList.ToArray(),
                target.IntegerList.ToArray(),
            };
            foreach (var intArray in integerArrays)
            {
                Assert.AreEqual(1, intArray[0]);
                Assert.AreEqual(2, intArray[1]);
                Assert.AreEqual(3, intArray[2]);
            }

            var strings = target.StringInterfaceList;
            Assert.IsNotNull(strings);
            Assert.AreEqual("StringArray-1", strings[0]);
            Assert.AreEqual("StringArray-2", strings[1]);
            Assert.AreEqual("StringArray-3", strings[2]);
        }

        [Test]
        public void TypedArrayInjected()
        {
            var target = new ArraysTargetObject();
            Needle.Inject(target, null, Application.Context);

            var typedArray = target.TypedArrayProperty;
            Assert.IsNotNull(typedArray);
            Assert.AreEqual(Color.ParseColor("#FF0000").ToArgb(), typedArray.GetColor(0, 0));
            Assert.AreEqual(Color.ParseColor("#00FF00").ToArgb(), typedArray.GetColor(1, 0));
            Assert.AreEqual(Color.ParseColor("#0000FF").ToArgb(), typedArray.GetColor(2, 0));
        }

        [Test]
        public void InvalidArrayTypeTargetIsInvalid()
        {
            var target = new InvalidArrayTypeTargetObject();
            Needle.Inject(target, null, Application.Context);

            // the length is right, but the items are not
            var intArray = target.IntegerArrayProperty;
            Assert.IsNotNull(intArray);
            Assert.AreEqual(0, intArray[0]);
            Assert.AreEqual(0, intArray[1]);
            Assert.AreEqual(0, intArray[2]);
        }

        [Test]
        public void InvalidTypeTargetThrows()
        {
            var target = new InvalidTypeTargetObject();
            Assert.Throws<InjectionException>(() =>
            {
                Needle.Inject(target, null, Application.Context);
            });
        }
    }
}
