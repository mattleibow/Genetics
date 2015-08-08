using System.Collections.Generic;
using Android.Content.Res;

using Genetics;
using Genetics.Attributes;

namespace GeneticsTests.TestCases
{
    public class ArraysTargetObject
    {
        [Splice(Resource.Array.IntegerArrayResource)]
        public int[] IntegerArrayProperty { get; set; }

        [Splice(Resource.Array.StringArrayResource)]
        public string[] StringArrayProperty { get; set; }

        [Splice(Resource.Array.TypedArrayResource)]
        public TypedArray TypedArrayProperty { get; set; }
    }

    public class ArrayTypeTargetObject
    {
        [Splice(Resource.Array.IntegerArrayResource)]
        public int[] IntegerArray { get; set; }

        [Splice(Resource.Array.IntegerArrayResource)]
        public List<int> IntegerList { get; set; }

        [Splice(Resource.Array.IntegerArrayResource)]
        public IList<int> IntegerInterfaceList { get; set; }

        [Splice(Resource.Array.IntegerArrayResource)]
        public IEnumerable<int> IntegerEnumerable { get; set; }

        [Splice(Resource.Array.StringArrayResource)]
        public IList<string> StringInterfaceList { get; set; }
    }

    public class InvalidArrayTypeTargetObject
    {
        [Splice(Resource.Array.StringArrayResource)]
        public int[] IntegerArrayProperty { get; set; }
    }

    public class InvalidTypeTargetObject
    {
        [Splice(Resource.Array.StringArrayResource)]
        public string StringArrayProperty { get; set; }
    }
}
