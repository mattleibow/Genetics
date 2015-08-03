using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

using Syringe;
using SyringeTests.TestCases;

namespace SyringeTests
{
    [TestFixture]
    public class TypeMappingTests
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
        public void GettingTheSameTypeMappingTwiceUsesTheCache()
        {
            var mapping1 = Needle.GetTypeMapping(typeof(Fields));
            var mapping2 = Needle.GetTypeMapping(typeof(Fields));

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

            var mapping = Needle.GetTypeMapping(typeof(Fields));

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

            var mapping = Needle.GetTypeMapping(typeof(Properties));

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
            var mapping = Needle.GetTypeMapping(typeof(IgnoredMembers));

            Assert.AreEqual(typeof(IgnoredMembers), mapping.Type);
            Assert.AreEqual(0, mapping.Members.Count);
        }

        [Test]
        public void CannotInjectReadonlyMembers()
        {
            var count = Needle.MappedTypes.Length;

            // fields
            Assert.Throws<InjectionException>(() =>
            {
                Needle.GetTypeMapping(typeof(PrivateReadonlyField));
            }, "Should not be able to inject private readonly fields.");
            Assert.IsFalse(Needle.MappedTypes.Contains(typeof(PrivateReadonlyField)));
            Assert.Throws<InjectionException>(() =>
            {
                Needle.GetTypeMapping(typeof(PublicReadonlyField));
            }, "Should not be able to inject public readonly fields.");
            Assert.IsFalse(Needle.MappedTypes.Contains(typeof(PublicReadonlyField)));
            Assert.Throws<InjectionException>(() =>
            {
                Needle.GetTypeMapping(typeof(ProtectedReadonlyField));
            }, "Should not be able to inject protected readonly fields.");
            Assert.IsFalse(Needle.MappedTypes.Contains(typeof(ProtectedReadonlyField)));

            // properties
            Assert.Throws<InjectionException>(() =>
            {
                Needle.GetTypeMapping(typeof(PrivateReadonlyProperty));
            }, "Should not be able to inject private readonly properties.");
            Assert.IsFalse(Needle.MappedTypes.Contains(typeof(PrivateReadonlyProperty)));
            Assert.Throws<InjectionException>(() =>
            {
                Needle.GetTypeMapping(typeof(PublicReadonlyProperty));
            }, "Should not be able to inject public readonly properties.");
            Assert.IsFalse(Needle.MappedTypes.Contains(typeof(PublicReadonlyProperty)));
            Assert.Throws<InjectionException>(() =>
            {
                Needle.GetTypeMapping(typeof(ProtectedReadonlyProperty));
            }, "Should not be able to inject protected readonly properties.");
            Assert.IsFalse(Needle.MappedTypes.Contains(typeof(ProtectedReadonlyProperty)));

            Assert.AreEqual(count, Needle.MappedTypes.Length);
        }
    }
}
