using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

using Genetics;
using GeneticsTests.TestCases;

namespace GeneticsTests
{
    [TestFixture]
    public class TypeMappingTests
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
        public void GettingTheSameTypeMappingTwiceUsesTheCache()
        {
            var mapping1 = Geneticist.GetTypeMapping(typeof(Fields));
            var mapping2 = Geneticist.GetTypeMapping(typeof(Fields));

            Assert.AreSame(mapping1, mapping2);
        }

        [Test]
        public void FieldMappingsAreCreatedCorrectly()
        {
            var members = new[]
            {
                new object[] { "privateField", typeof(string) },
                new object[] { "publicField" , typeof(string) },
                new object[] { "protectedField", typeof(string) }
            };

            var mapping = Geneticist.GetTypeMapping(typeof(Fields));

            Assert.AreEqual(typeof(Fields), mapping.Type);
            Assert.AreEqual(3, mapping.Members.Count);

            int index = 0;
            foreach (var memberMapping in mapping.Members)
            {
                Assert.IsNotNull(memberMapping.Key as FieldInfo);
                Assert.IsNotNull(memberMapping.Value);

                Assert.AreEqual(typeof(Fields), memberMapping.Value.Type);
                Assert.IsNotNull(memberMapping.Value.Attribute);
                Assert.IsNotNull(memberMapping.Value.SetterMethod);
                Assert.IsNotNull(memberMapping.Value.GetterMethod);
                Assert.AreEqual(members[index][0], memberMapping.Value.Member.Name);
                Assert.AreEqual(members[index][1], memberMapping.Value.MemberType);

                index++;
            }
        }

        [Test]
        public void PropertyMappingsAreCreatedCorrectly()
        {
            var members = new[]
            {
                new object[] { "PrivateProperty", typeof(string) },
                new object[] { "PublicProperty" , typeof(string) },
                new object[] { "ProtectedProperty", typeof(string) }
            };

            var mapping = Geneticist.GetTypeMapping(typeof(Properties));

            Assert.AreEqual(typeof(Properties), mapping.Type);
            Assert.AreEqual(3, mapping.Members.Count);

            int index = 0;
            foreach (var memberMapping in mapping.Members)
            {
                Assert.IsNotNull(memberMapping.Key as PropertyInfo);
                Assert.IsNotNull(memberMapping.Value);

                Assert.AreEqual(typeof(Properties), memberMapping.Value.Type);
                Assert.IsNotNull(memberMapping.Value.Attribute);
                Assert.IsNotNull(memberMapping.Value.SetterMethod);
                Assert.IsNotNull(memberMapping.Value.GetterMethod);
                Assert.AreEqual(members[index][0], memberMapping.Value.Member.Name);
                Assert.AreEqual(members[index][1], memberMapping.Value.MemberType);

                index++;
            }
        }

        [Test]
        public void UnattributedMembersAreIgnored()
        {
            var mapping = Geneticist.GetTypeMapping(typeof(IgnoredMembers));

            Assert.AreEqual(typeof(IgnoredMembers), mapping.Type);
            Assert.AreEqual(0, mapping.Members.Count);
        }

        [Test]
        public void CannotSpliceReadonlyMembers()
        {
            var count = Geneticist.MappedTypes.Length;

            // fields
            Assert.Throws<SpliceException>(() =>
            {
                Geneticist.GetTypeMapping(typeof(PrivateReadonlyField));
            }, "Should not be able to splice private readonly fields.");
            Assert.IsFalse(Geneticist.MappedTypes.Contains(typeof(PrivateReadonlyField)));
            Assert.Throws<SpliceException>(() =>
            {
                Geneticist.GetTypeMapping(typeof(PublicReadonlyField));
            }, "Should not be able to splice public readonly fields.");
            Assert.IsFalse(Geneticist.MappedTypes.Contains(typeof(PublicReadonlyField)));
            Assert.Throws<SpliceException>(() =>
            {
                Geneticist.GetTypeMapping(typeof(ProtectedReadonlyField));
            }, "Should not be able to splice protected readonly fields.");
            Assert.IsFalse(Geneticist.MappedTypes.Contains(typeof(ProtectedReadonlyField)));

            // properties
            Assert.Throws<SpliceException>(() =>
            {
                Geneticist.GetTypeMapping(typeof(PrivateReadonlyProperty));
            }, "Should not be able to splice private readonly properties.");
            Assert.IsFalse(Geneticist.MappedTypes.Contains(typeof(PrivateReadonlyProperty)));
            Assert.Throws<SpliceException>(() =>
            {
                Geneticist.GetTypeMapping(typeof(PublicReadonlyProperty));
            }, "Should not be able to splice public readonly properties.");
            Assert.IsFalse(Geneticist.MappedTypes.Contains(typeof(PublicReadonlyProperty)));
            Assert.Throws<SpliceException>(() =>
            {
                Geneticist.GetTypeMapping(typeof(ProtectedReadonlyProperty));
            }, "Should not be able to splice protected readonly properties.");
            Assert.IsFalse(Geneticist.MappedTypes.Contains(typeof(ProtectedReadonlyProperty)));

            Assert.AreEqual(count, Geneticist.MappedTypes.Length);
        }
    }
}
