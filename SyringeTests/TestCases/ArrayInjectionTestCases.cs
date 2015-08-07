using System.Collections.Generic;
using Android.Content.Res;

using Syringe;
using Syringe.Attributes;

namespace SyringeTests.TestCases
{
    public class ArraysTargetObject
    {
        [Inject(Resource.Array.IntegerArrayResource)]
        public int[] IntegerArrayProperty { get; set; }

        [Inject(Resource.Array.StringArrayResource)]
        public string[] StringArrayProperty { get; set; }

        [Inject(Resource.Array.TypedArrayResource)]
        public TypedArray TypedArrayProperty { get; set; }
    }

    public class ArrayTypeTargetObject
    {
        [Inject(Resource.Array.IntegerArrayResource)]
        public int[] IntegerArray { get; set; }

        [Inject(Resource.Array.IntegerArrayResource)]
        public List<int> IntegerList { get; set; }

        [Inject(Resource.Array.IntegerArrayResource)]
        public IList<int> IntegerInterfaceList { get; set; }

        [Inject(Resource.Array.IntegerArrayResource)]
        public IEnumerable<int> IntegerEnumerable { get; set; }

        [Inject(Resource.Array.StringArrayResource)]
        public IList<string> StringInterfaceList { get; set; }
    }

    public class InvalidArrayTypeTargetObject
    {
        [Inject(Resource.Array.StringArrayResource)]
        public int[] IntegerArrayProperty { get; set; }
    }

    public class InvalidTypeTargetObject
    {
        [Inject(Resource.Array.StringArrayResource)]
        public string StringArrayProperty { get; set; }
    }
}
