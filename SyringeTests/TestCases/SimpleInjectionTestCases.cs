using System.Xml;
using System.Xml.Linq;

using Android.Animation;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views.Animations;

using Syringe;

namespace SyringeTests.TestCases
{
    public class SimpleTargetObject
    {
        [Inject(Resource.Boolean.BooleanResource)]
        public bool BooleanProperty { get; set; }

        [Inject(Resource.Color.ColorResource)]
        public Color ColorPropertyAndroid { get; set; }

        [Inject(Resource.Color.ColorResource)]
        public System.Drawing.Color ColorPropertySystem { get; set; }

        [Inject(Resource.Color.ColorStateListResource)]
        public ColorStateList ColorStateListProperty { get; set; }

        [Inject(Resource.Dimension.DimensionResource)]
        public float DimensionProperty { get; set; }

        [Inject(Resource.Integer.IntegerResource)]
        public int IntegerProperty { get; set; }

        [Inject(Resource.String.StringResource)]
        public string StringProperty { get; set; }
    }

    public class AnimationsTargetObject
    {
        [Inject(Resource.Animation.AnimationResource)]
        public Animation AnimationProperty { get; set; }

        [Inject(Resource.Animator.AnimatorResource)]
        public Animator AnimatorProperty { get; set; }
    }

    public class DrawableTargetObject
    {
        [Inject(Resource.Drawable.Icon)]
        public Bitmap BitmapProperty { get; set; }

        [Inject(Resource.Drawable.Icon)]
        public Drawable DrawableProperty { get; set; }

        [Inject(Resource.Drawable.Icon)]
        public BitmapDrawable BitmapDrawableProperty { get; set; }
    }

    public class XmlTargetObject
    {
        [Inject(Resource.Xml.XmlResource)]
        public XmlReader XmlReaderResource { get; set; }

        [Inject(Resource.Xml.XmlResource)]
        public XmlDocument XmlDocumentResource { get; set; }

        [Inject(Resource.Xml.XmlResource)]
        public XDocument XDocumentResource { get; set; }

        [Inject(Resource.Xml.XmlResource)]
        public string XmlStringResource { get; set; }
    }
}
