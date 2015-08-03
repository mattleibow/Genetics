using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

using Syringe;
using SyringeTests.TestCases;

namespace SyringeTests
{
    [TestFixture]
    public class EnumerableMappingTests
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
        public void GetEnumerableElementTypeTest()
        {
            var validTypes = new[]
            {
                typeof(int[]),
                typeof(IEnumerable<int>),
                typeof(IList<int>),
                typeof(List<int>),
                typeof(IReadOnlyList<int>),
                typeof(ICollection<int>),
                typeof(IReadOnlyCollection<int>),
                typeof(CustomEnumerable),
                typeof(CustomAbstractList),
                typeof(CustomList),
            };
            foreach (var type in validTypes)
            {
                Assert.AreEqual(typeof(int), type.GetEnumerableElementType());
            }

            var invalidTypes = new[]
            {
                typeof(Array),
                typeof(IEnumerable),
                typeof(ArrayList),
                typeof(CustomType),
            };
            foreach (var type in invalidTypes)
            {
                Assert.IsNull(type.GetEnumerableElementType());
            }
        }

        [Test]
        public void CreateEnumerableTest()
        {
            var array = new int[] { 1, 2, 3 };
            var list = new List<int> { 1, 2, 3 };

            var validTypes = new Dictionary<Type, Type>
            {
                { typeof(int[]), typeof(int[]) },
                { typeof(IEnumerable<int>), typeof(List<int>) },
                { typeof(List<int>), typeof(List<int>) },
                { typeof(IList<int>), typeof(List<int>) },
                { typeof(IReadOnlyList<int>), typeof(List<int>) },
                { typeof(ICollection<int>), typeof(List<int>) },
                { typeof(IReadOnlyCollection<int>), typeof(List<int>) },
                { typeof(CustomList), typeof(CustomList) },
            };
            foreach (var pair in validTypes)
            {
                var result = array.CreateEnumerable(pair.Key);
                Assert.AreEqual(pair.Value, result.GetType());

                result = list.CreateEnumerable(pair.Key);
                Assert.AreEqual(pair.Value, result.GetType());
            }

            var invalidTypes = new[]
            {
                typeof(Array),
                typeof(IEnumerable),
                typeof(ArrayList),
                typeof(CustomEnumerable),
                typeof(CustomType),
            };
            foreach (var type in invalidTypes)
            {
                var result = array.CreateEnumerable(type);
                Assert.IsNull(result);

                result = list.CreateEnumerable(type);
                Assert.IsNull(result);
            }
        }
    }
}
