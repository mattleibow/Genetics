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
        [Splice(Resource.Drawable.Icon)]
        public Bitmap BitmapProperty { get; set; }

        [Splice(Resource.Drawable.Icon)]
        public Drawable DrawableProperty { get; set; }

        [Splice(Resource.Drawable.Icon)]
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
}
