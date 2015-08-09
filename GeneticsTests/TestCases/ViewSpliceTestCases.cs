using Android.Content;
using Android.Views;
using Android.Widget;

using Genetics;
using Genetics.Attributes;

namespace GeneticsTests.TestCases
{
    public class SimpleViewsTargetObject
    {
        [Splice(Resource.Id.simpleButton)]
        public Button ButtonProperty { get; set; }

        [Splice(Resource.Id.simpleButton)]
        public TextView ButtonAsTextViewProperty { get; set; }

        [Splice(Resource.Id.simpleButton)]
        public View ButtonAsViewProperty { get; set; }

        [Splice(Resource.Id.simpleCheckBox)]
        public CheckBox CheckBoxProperty { get; set; }
    }

    public class InvalidViewTypeTargetObject
    {
        [Splice(Resource.Id.simpleButton)]
        public EditText EditTextProperty { get; set; }
    }

    //public class JavaCastViewsTargetObject
    //{
    //    [Splice(Resource.Id.javaCastNativeToolbar)]
    //    public Toolbar ToolbarNativeAndNativeProperty { get; set; }
    //
    //    [Splice(Resource.Id.javaCastNativeToolbar)]
    //    public Android.Support.V7.Widget.Toolbar ToolbarNativeButSupportProperty { get; set; }
    //
    //    [Splice(Resource.Id.javaCastSupportToolbar)]
    //    public Toolbar ToolbarSupportButNativeProperty { get; set; }
    //
    //    [Splice(Resource.Id.javaCastSupportToolbar)]
    //    public Android.Support.V7.Widget.Toolbar ToolbarSupportAndSupportProperty { get; set; }
    //}
}
