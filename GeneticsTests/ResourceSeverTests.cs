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
    public class ResourceSeverTests
    {
        [SetUp]
        public void Setup()
        {
            Geneticist.Debug = true;
            Geneticist.DebugTextWriter = Console.Out;
            Geneticist.ThrowOnError = true;
        }

        [TearDown]
        public void Tear()
        {
        }

        [Test]
        public void SeverOnUnsplicedDoesNotThrow()
        {
            var view = ViewSpliceTests.CreateView(Resource.Layout.SimpleLayout);
            var simple = new GeneralTargetObject();
            Geneticist.Sever(simple, view, Application.Context);
        }

        [Test]
        public void MultipleSeverDoesNotThrow()
        {
            // just a simple twice on an empty object
            var target = new GeneralTargetObject();
            Geneticist.Sever(target, null, Application.Context);
            Geneticist.Sever(target, null, Application.Context);

            // now after splicing
            var view = ViewSpliceTests.CreateView(Resource.Layout.SimpleLayout);
            target = new GeneralTargetObject();
            Geneticist.Splice(target, view, Application.Context);
            Geneticist.Sever(target, view, Application.Context);
            Geneticist.Sever(target, view, Application.Context);
        }

        [Test]
        public void SeverWitoutViewDoesNotThrow()
        {
            var view = ViewSpliceTests.CreateView(Resource.Layout.SimpleLayout);
            var target = new GeneralTargetObject();
            Geneticist.Splice(target, view, Application.Context);
            Geneticist.Sever(target, null, Application.Context);
        }

        [Test]
        public void AllSupportedDrawableMembersAreUnbound()
        {
            var target = new DrawableTargetObject();
            Geneticist.Splice(target, null, Application.Context);

            var bitmap = target.BitmapProperty;
            var drawable = target.DrawableProperty;
            var bitmapDrawable = target.BitmapDrawableProperty;

            Assert.IsNotNull(bitmap);
            Assert.IsNotNull(drawable);
            Assert.IsNotNull(bitmapDrawable);

            Geneticist.Sever(target, null, Application.Context);

            Assert.IsNull(target.BitmapProperty);
            Assert.IsNull(target.DrawableProperty);
            Assert.IsNull(target.BitmapDrawableProperty);

            Assert.Throws<ArgumentException>(() => { bitmap.GetPixel(0, 0); });
            Assert.Throws<ArgumentException>(() => { drawable.GetState(); });
            Assert.Throws<ArgumentException>(() => { bitmapDrawable.GetState(); });
        }

        [Test]
        public void ExactMatchMembersAreSevered()
        {
            var view = ViewSpliceTests.CreateView(Resource.Layout.SimpleLayout);

            var target = new ExactEventSpliceTargetObject();
            Geneticist.Splice(target, view, Application.Context);
            var button = view.FindViewById(Resource.Id.simpleButton);

            var ex = Assert.Throws<NotImplementedException>(() =>
            {
                button.CallOnClick();
            });
            Assert.AreEqual("ExactParametersMethod", ex.Message);

            Geneticist.Sever(target, view, Application.Context);
            button.CallOnClick();
        }
    }
}
