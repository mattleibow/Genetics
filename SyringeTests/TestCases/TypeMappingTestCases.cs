using Android.Content;

using Syringe;

namespace SyringeTests.TestCases
{
    public class Fields
    {
        [Inject(Resource.String.StringResource)]
        private string privateField;
        [Inject(Resource.String.StringResource)]
        public string publicField;
        [Inject(Resource.String.StringResource)]
        protected string protectedField;
    }

    public class Properties
    {
        [Inject(Resource.String.StringResource)]
        private string PrivateProperty { get; set; }
        [Inject(Resource.String.StringResource)]
        public string PublicProperty { get; set; }
        [Inject(Resource.String.StringResource)]
        protected string ProtectedProperty { get; set; }
    }

    public class PrivateReadonlyField
    {
        [Inject(Resource.String.StringResource)]
        private readonly string readonlyField;
    }

    public class PublicReadonlyField
    {
        [Inject(Resource.String.StringResource)]
        public readonly string readonlyField;
    }

    public class ProtectedReadonlyField
    {
        [Inject(Resource.String.StringResource)]
        protected readonly string readonlyField;
    }

    public class PrivateReadonlyProperty
    {
        [Inject(Resource.String.StringResource)]
        private readonly string readonlyProperty;
    }

    public class PublicReadonlyProperty
    {
        [Inject(Resource.String.StringResource)]
        public readonly string readonlyProperty;
    }

    public class ProtectedReadonlyProperty
    {
        [Inject(Resource.String.StringResource)]
        protected readonly string readonlyProperty;
    }

    public class IgnoredMembers
    {
        public string ignoredField;

        public string IgnoredProperty { get; set; }

    }
}
