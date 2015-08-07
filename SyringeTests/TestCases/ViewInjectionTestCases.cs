using Android.Content;
using Android.Views;
using Android.Widget;

using Syringe;
using Syringe.Attributes;

namespace SyringeTests.TestCases
{
    public class SimpleViewsTargetObject
    {
        [Inject(Resource.Id.simpleButton)]
        public Button ButtonProperty { get; set; }

        [Inject(Resource.Id.simpleButton)]
        public TextView ButtonAsTextViewProperty { get; set; }

        [Inject(Resource.Id.simpleButton)]
        public View ButtonAsViewProperty { get; set; }

        [Inject(Resource.Id.simpleCheckBox)]
        public CheckBox CheckBoxProperty { get; set; }
    }

    public class InvalidViewTypeTargetObject
    {
        [Inject(Resource.Id.simpleButton)]
        public EditText EditTextProperty { get; set; }
    }

    //public class JavaCastViewsTargetObject
    //{
    //    [Inject(Resource.Id.javaCastNativeToolbar)]
    //    public Toolbar ToolbarNativeAndNativeProperty { get; set; }
    //
    //    [Inject(Resource.Id.javaCastNativeToolbar)]
    //    public Android.Support.V7.Widget.Toolbar ToolbarNativeButSupportProperty { get; set; }
    //
    //    [Inject(Resource.Id.javaCastSupportToolbar)]
    //    public Toolbar ToolbarSupportButNativeProperty { get; set; }
    //
    //    [Inject(Resource.Id.javaCastSupportToolbar)]
    //    public Android.Support.V7.Widget.Toolbar ToolbarSupportAndSupportProperty { get; set; }
    //}
}
