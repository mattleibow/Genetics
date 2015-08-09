using Android.Content;

using Genetics;
using Genetics.Attributes;

namespace GeneticsTests.TestCases
{
    public class Fields
    {
        [Splice(Resource.String.StringResource)]
        private string privateField;
        [Splice(Resource.String.StringResource)]
        public string publicField;
        [Splice(Resource.String.StringResource)]
        protected string protectedField;
    }

    public class Properties
    {
        [Splice(Resource.String.StringResource)]
        private string PrivateProperty { get; set; }
        [Splice(Resource.String.StringResource)]
        public string PublicProperty { get; set; }
        [Splice(Resource.String.StringResource)]
        protected string ProtectedProperty { get; set; }
    }

    public class PrivateReadonlyField
    {
        [Splice(Resource.String.StringResource)]
        private readonly string readonlyField;
    }

    public class PublicReadonlyField
    {
        [Splice(Resource.String.StringResource)]
        public readonly string readonlyField;
    }

    public class ProtectedReadonlyField
    {
        [Splice(Resource.String.StringResource)]
        protected readonly string readonlyField;
    }

    public class PrivateReadonlyProperty
    {
        [Splice(Resource.String.StringResource)]
        private readonly string readonlyProperty;
    }

    public class PublicReadonlyProperty
    {
        [Splice(Resource.String.StringResource)]
        public readonly string readonlyProperty;
    }

    public class ProtectedReadonlyProperty
    {
        [Splice(Resource.String.StringResource)]
        protected readonly string readonlyProperty;
    }

    public class IgnoredMembers
    {
        public string ignoredField;

        public string IgnoredProperty { get; set; }

    }
}
