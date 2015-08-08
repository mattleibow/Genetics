using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

using Genetics;
using GeneticsTests.TestCases;

namespace GeneticsTests
{
    [TestFixture]
    public class EnumerableMappingTests
    {
        [SetUp]
        public void Setup()
        {
            Geneticist.Debug = true;
            Geneticist.DebugTextWriter = Console.Out;
            Geneticist.ThrowOnError = true;
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
                var result = pair.Key.CreateEnumerable(array);
                Assert.AreEqual(pair.Value, result.GetType());

                result = pair.Key.CreateEnumerable(list);
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
                var result = type.CreateEnumerable(array);
                Assert.IsNull(result);

                result = type.CreateEnumerable(list);
                Assert.IsNull(result);
            }
        }
    }
}
