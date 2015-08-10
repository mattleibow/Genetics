using System;
using Android.App;
using Android.Graphics;
using Android.Views.Animations;
using Android.Animation;
using NUnit.Framework;

using Genetics;
using GeneticsTests.TestCases;

namespace GeneticsTests
{
    [TestFixture]
    public class ResourceSpliceTests : TestBase
    {
        [Test]
        public void TypeMappingCreatedForAllSupportedMembers()
        {
            var mapping = Geneticist.GetTypeMapping(typeof(SimpleTargetObject));
            Assert.AreEqual(7, mapping.Members.Values.Count);

            mapping = Geneticist.GetTypeMapping(typeof(AnimationsTargetObject));
            Assert.AreEqual(2, mapping.Members.Values.Count);

            mapping = Geneticist.GetTypeMapping(typeof(DrawableTargetObject));
            Assert.AreEqual(3, mapping.Members.Values.Count);

            mapping = Geneticist.GetTypeMapping(typeof(XmlTargetObject));
            Assert.AreEqual(4, mapping.Members.Values.Count);
        }

        [Test]
        public void AllSupportedMembersAreBound()
        {
            var target = new SimpleTargetObject();
            Geneticist.Splice(target, null, Application.Context);

            Assert.IsTrue(target.BooleanProperty);

            Assert.AreEqual(Color.Argb(255, 255, 0, 0), target.ColorPropertyAndroid);

            Assert.AreEqual(System.Drawing.Color.FromArgb(255, 255, 0, 0), target.ColorPropertySystem);

            var colorStateList = target.ColorStateListProperty;
            Assert.AreEqual(System.Drawing.Color.Black.ToArgb(), colorStateList.DefaultColor);
            Assert.AreEqual(System.Drawing.Color.Blue.ToArgb(), colorStateList.GetColorForState(new[] { Android.Resource.Attribute.StateFocused }, Color.Fuchsia));
            Assert.AreEqual(System.Drawing.Color.Red.ToArgb(), colorStateList.GetColorForState(new[] { Android.Resource.Attribute.StatePressed }, Color.Fuchsia));

            var density = Application.Context.Resources.DisplayMetrics.Density;
            Assert.AreEqual(25.0f * density, target.DimensionProperty);

            Assert.AreEqual(123, target.IntegerProperty);

            Assert.AreEqual("This is a String resource.", target.StringProperty);
        }

        [Test]
        public void AllSupportedAnimationMembersAreBound()
        {
            var target = new AnimationsTargetObject();
            Geneticist.Splice(target, null, Application.Context);

            var animation = target.AnimationProperty;
            Assert.IsTrue(animation is AnimationSet);
            var animationSet = (AnimationSet)animation;
            Assert.IsTrue(animationSet.Animations[0] is ScaleAnimation);
            Assert.IsTrue(animationSet.Animations[1] is AnimationSet);
            animationSet = (AnimationSet)animationSet.Animations[1];
            Assert.IsTrue(animationSet.Animations[0] is ScaleAnimation);
            Assert.IsTrue(animationSet.Animations[1] is RotateAnimation);

            var animator = target.AnimatorProperty;
            Assert.IsTrue(animator is AnimatorSet);
            var animatorSet = (AnimatorSet)animator;
            Assert.IsTrue(animatorSet.ChildAnimations[0] is AnimatorSet);
            Assert.IsTrue(animatorSet.ChildAnimations[1] is ObjectAnimator);
            animatorSet = (AnimatorSet)animatorSet.ChildAnimations[0];
            Assert.IsTrue(animatorSet.ChildAnimations[0] is ObjectAnimator);
            Assert.IsTrue(animatorSet.ChildAnimations[1] is ObjectAnimator);
        }

        [Test]
        public void AllSupportedDrawableMembersAreBound()
        {
            var density = Application.Context.Resources.DisplayMetrics.Density;
            var target = new DrawableTargetObject();
            Geneticist.Splice(target, null, Application.Context);

            var bitmap = target.BitmapProperty;
            Assert.IsNotNull(bitmap);
            Assert.AreEqual(72.0f * density, bitmap.Width);
            Assert.AreEqual(72.0f * density, bitmap.Height);

            var drawable = target.DrawableProperty;
            Assert.IsNotNull(drawable);
            Assert.AreEqual(72.0f * density, drawable.IntrinsicWidth);
            Assert.AreEqual(72.0f * density, drawable.IntrinsicHeight);

            var bitmapDrawable = target.BitmapDrawableProperty;
            Assert.IsNotNull(bitmapDrawable);
            Assert.AreEqual(72.0f * density, bitmapDrawable.Bitmap.Width);
            Assert.AreEqual(72.0f * density, bitmapDrawable.Bitmap.Height);
        }

        [Test]
        public void AllSupportedXmlMembersAreBound()
        {
            var target = new XmlTargetObject();
            Geneticist.Splice(target, null, Application.Context);

            var xmlReader = target.XmlReaderResource;
            Assert.IsNotNull(xmlReader);
            xmlReader.Read();
            Assert.AreEqual("RootElement", xmlReader.LocalName);
            xmlReader.MoveToFirstAttribute();
            Assert.AreEqual("attribute", xmlReader.Name);
            Assert.AreEqual("attribute value", xmlReader.Value);

            var xmlDocument = target.XmlDocumentResource;
            Assert.IsNotNull(xmlDocument);
            var documentElement = xmlDocument.DocumentElement;
            Assert.AreEqual("RootElement", documentElement.LocalName);
            Assert.AreEqual("attribute value", documentElement.Attributes["attribute"].Value);

            var xDocument = target.XDocumentResource;
            Assert.IsNotNull(xDocument);
            var xElement = xDocument.Root;
            Assert.AreEqual("RootElement", xElement.Name.LocalName);
            Assert.AreEqual("attribute value", xElement.Attribute("attribute").Value);

            var xmlString = target.XmlStringResource;
            Assert.IsNotNull(xmlString);
            Assert.IsTrue(xmlString.Contains("</RootElement>"));
            Assert.IsTrue(xmlString.Contains("attribute=\"attribute value\""));
        }
    }
}
