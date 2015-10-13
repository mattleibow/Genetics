using System;
using Android.App;
using Android.Views;
using NUnit.Framework;

using Genetics;
using GeneticsTests.TestCases;

namespace GeneticsTests
{
    [TestFixture]
    public class ViewSpliceTests : TestBase
    {
        [Test]
        public void TypeMappingCreatedForAllSupportedMembers()
		{
			var mapping = Geneticist.GetTypeMapping(typeof(SimpleViewsTargetObject));
			Assert.AreEqual(4, mapping.Members.Values.Count);

			mapping = Geneticist.GetTypeMapping(typeof(InvalidViewTypeTargetObject));
			Assert.AreEqual(1, mapping.Members.Values.Count);

			mapping = Geneticist.GetTypeMapping(typeof(ViewNotFoundTargetObject));
			Assert.AreEqual(2, mapping.Members.Values.Count);

            //mapping = Geneticist.GetTypeMapping(typeof(JavaCastViewsTargetObject));
            //Assert.AreEqual(4, mapping.Members.Values.Count);
        }

        [Test]
        public void AllSimpleMembersAreBound()
        {
            var view = CreateView(Resource.Layout.SimpleLayout);

            var target = new SimpleViewsTargetObject();
            Geneticist.Splice(target, view, Application.Context);

            Assert.IsNotNull(target.ButtonProperty);
            Assert.IsNotNull(target.ButtonAsViewProperty);
            Assert.IsNotNull(target.ButtonAsTextViewProperty);
            Assert.IsNotNull(target.CheckBoxProperty);

            Assert.AreSame(target.ButtonProperty, target.ButtonAsViewProperty);
            Assert.AreSame(target.ButtonProperty, target.ButtonAsTextViewProperty);
        }

        [Test]
        public void ViewNotFoundIsHandledCorrectly()
        {
            var view = CreateView(Resource.Layout.SimpleLayout);

            var target = new ViewNotFoundTargetObject();
            Geneticist.Splice(target, view, Application.Context);

            Assert.IsNotNull(target.ButtonProperty);
            Assert.IsNull(target.MissingViewProperty);
        }

        [Test]
        public void InvalidTypeMembersThrow()
        {
            var view = CreateView(Resource.Layout.SimpleLayout);

            var target = new InvalidViewTypeTargetObject();
            Assert.Throws<InvalidCastException>(() =>
            {
                Geneticist.Splice(target, view, Application.Context);
            });
		}

        //[Test]
        //public void AllJavaCastMembersAreBound()
        //{
        //    Assert.Ignore("Unable tyo find a test case");
        //
        //    var view = CreateView(Resource.Layout.JavaCastRequiredLayout);
        //
        //    var target = new JavaCastViewsTargetObject();
        //    Geneticist.Splice(target, view, Application.Context);
        //
        //    Assert.IsNotNull(target.ToolbarNativeAndNativeProperty);
        //    Assert.IsNotNull(target.ToolbarSupportButNativeProperty);
        //    Assert.IsNotNull(target.ToolbarNativeButSupportProperty);
        //    Assert.IsNotNull(target.ToolbarSupportAndSupportProperty);
        //
        //    Assert.AreSame(target.ToolbarNativeAndNativeProperty, target.ToolbarSupportButNativeProperty);
        //    Assert.AreSame(target.ToolbarNativeButSupportProperty, target.ToolbarSupportAndSupportProperty);
        //}

        public static View CreateView(int layout)
        {
            var activity = GeneticsTestsApplication.CurrentActivity;
            var parent = (ViewGroup)activity.FindViewById(Android.Resource.Id.Content);

            var inflater = LayoutInflater.FromContext(Application.Context);
            var view = inflater.Inflate(layout, parent, false);

            return view;
        }
    }
}
