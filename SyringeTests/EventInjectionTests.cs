using System;
using Android.App;
using Android.Views;
using NUnit.Framework;

using Syringe;
using SyringeTests.TestCases;
using System.Linq;

namespace SyringeTests
{
    [TestFixture]
    public class EventInjectionTests
    {
        [SetUp]
        public void Setup()
        {
            Needle.Debug = true;
            Needle.DebugTextWriter = Console.Out;
            Needle.ThrowOnError = true;
        }

        [TearDown]
        public void Tear()
        {
        }

        [Test]
        public void TypeMappingCreatedForAllSupportedMembers()
		{
            var typeMapping = Needle.GetTypeMapping(typeof(ExactEventInjectionTargetObject));
            Assert.AreEqual(1, typeMapping.Methods.Values.Count);
            Assert.AreEqual("ExactParametersMethod", typeMapping.Methods.Values.Single().Method.Name);

            typeMapping = Needle.GetTypeMapping(typeof(SimilarEventInjectionTargetObject));
            Assert.AreEqual(1, typeMapping.Methods.Values.Count);
            Assert.AreEqual("SimilarParametersMethod", typeMapping.Methods.Values.Single().Method.Name);
        }

        [Test]
        public void InvalidEventThrows()
		{
            var typeMapping = Needle.GetTypeMapping(typeof(InvalidEventInjectionTargetObject));
            Assert.AreEqual(1, typeMapping.Methods.Values.Count);

            var view = CreateView(Resource.Layout.SimpleLayout);

            var target = new InvalidEventInjectionTargetObject();

            var ex = Assert.Throws<InjectionException>(() =>
            {
                Needle.Inject(target, view, Application.Context);
            });
            Assert.AreEqual("Unable to find event 'ItemLongClick' for method 'InvalidEvent'.", ex.Message);
        }

        [Test]
        public void ExactMatchMembersAreBound()
        {
            var view = CreateView(Resource.Layout.SimpleLayout);

            var target = new ExactEventInjectionTargetObject();
            Needle.Inject(target, view, Application.Context);
            var button = view.FindViewById(Resource.Id.simpleButton);

            var ex = Assert.Throws<NotImplementedException>(() =>
            {
                button.CallOnClick();
            });
            Assert.AreEqual("ExactParametersMethod", ex.Message);
        }

        [Test]
        public void SimilarMatchMembersAreBound()
        {
            var view = CreateView(Resource.Layout.SimpleLayout);

            var target = new SimilarEventInjectionTargetObject();
            Needle.Inject(target, view, Application.Context);
            var button = view.FindViewById(Resource.Id.simpleButton);

            var ex = Assert.Throws<NotImplementedException>(() =>
            {
                button.CallOnClick();
            });
            Assert.AreEqual("SimilarParametersMethod", ex.Message);
        }

        [Test]
        public void DifferentMembersThrow()
        {
            var view = CreateView(Resource.Layout.SimpleLayout);
            var target = new DifferentEventInjectionTargetObject();

            var ex = Assert.Throws<InjectionException>(() =>
            {
                Needle.Inject(target, view, Application.Context);
            });
            Assert.AreEqual("Error creating delegate from 'SenderPerameterMethod' for event 'Click'.", ex.Message);
        }

        [Test]
        public void ParameterlessMembersThrow()
        {
            var view = CreateView(Resource.Layout.SimpleLayout);
            var target = new ParameterlessEventInjectionTargetObject();

            var ex = Assert.Throws<InjectionException>(() =>
            {
                Needle.Inject(target, view, Application.Context);
            });
            Assert.AreEqual("Error creating delegate from 'ParameterlessMethod' for event 'Click'.", ex.Message);
        }
        
        private static View CreateView(int layout)
        {
            var activity = SyringeTestsApplication.CurrentActivity;
            var parent = (ViewGroup)activity.FindViewById(Android.Resource.Id.Content);

            var inflater = LayoutInflater.FromContext(Application.Context);
            var view = inflater.Inflate(layout, parent, false);

            return view;
        }
    }
}
