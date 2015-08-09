using System;
using System.Xml;
using System.Xml.Linq;

using Android.Animation;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views.Animations;

using Genetics;
using Genetics.Attributes;

namespace GeneticsTests.TestCases
{
    public class SimpleTargetObject
    {
        [Splice(Resource.Boolean.BooleanResource)]
        public bool BooleanProperty { get; set; }

        [Splice(Resource.Color.ColorResource)]
        public Color ColorPropertyAndroid { get; set; }

        [Splice(Resource.Color.ColorResource)]
        public System.Drawing.Color ColorPropertySystem { get; set; }

        [Splice(Resource.Color.ColorStateListResource)]
        public ColorStateList ColorStateListProperty { get; set; }

        [Splice(Resource.Dimension.DimensionResource)]
        public float DimensionProperty { get; set; }

        [Splice(Resource.Integer.IntegerResource)]
        public int IntegerProperty { get; set; }

        [Splice(Resource.String.StringResource)]
        public string StringProperty { get; set; }
    }

    public class AnimationsTargetObject
    {
        [Splice(Resource.Animation.AnimationResource)]
        public Animation AnimationProperty { get; set; }

        [Splice(Resource.Animator.AnimatorResource)]
        public Animator AnimatorProperty { get; set; }
    }

    public class DrawableTargetObject
    {
        [Splice(Resource.Drawable.Icon, DisposeOnSever = true)]
        public Bitmap BitmapProperty { get; set; }

        [Splice(Resource.Drawable.Icon, DisposeOnSever = true)]
        public Drawable DrawableProperty { get; set; }

        [Splice(Resource.Drawable.Icon, DisposeOnSever = true)]
        public BitmapDrawable BitmapDrawableProperty { get; set; }
    }

    public class XmlTargetObject
    {
        [Splice(Resource.Xml.XmlResource)]
        public XmlReader XmlReaderResource { get; set; }

        [Splice(Resource.Xml.XmlResource)]
        public XmlDocument XmlDocumentResource { get; set; }

        [Splice(Resource.Xml.XmlResource)]
        public XDocument XDocumentResource { get; set; }

        [Splice(Resource.Xml.XmlResource)]
        public string XmlStringResource { get; set; }
    }

    public class GeneralTargetObject
    {
        [Splice(Resource.Boolean.BooleanResource)]
        public bool BooleanProperty { get; set; }

        [Splice(Resource.Color.ColorResource)]
        public Color ColorPropertyAndroid { get; set; }

        [Splice(Resource.Color.ColorResource)]
        public System.Drawing.Color ColorPropertySystem { get; set; }

        [Splice(Resource.Color.ColorStateListResource)]
        public ColorStateList ColorStateListProperty { get; set; }

        [Splice(Resource.Dimension.DimensionResource)]
        public float DimensionProperty { get; set; }

        [Splice(Resource.Integer.IntegerResource)]
        public int IntegerProperty { get; set; }

        [Splice(Resource.String.StringResource)]
        public string StringProperty { get; set; }

        [Splice(Resource.Array.IntegerArrayResource)]
        public int[] IntegerArrayProperty { get; set; }

        [Splice(Resource.Array.StringArrayResource)]
        public string[] StringArrayProperty { get; set; }

        [Splice(Resource.Drawable.Icon, DisposeOnSever = true)]
        public Bitmap BitmapProperty { get; set; }

        [Splice(Resource.Xml.XmlResource)]
        public string XmlStringResource { get; set; }

        [SpliceClick(Resource.Id.simpleButton)]
        public void ExactParametersMethod(object sender, EventArgs e)
        {
            throw new NotImplementedException("ExactParametersMethod");
        }
    }
}
