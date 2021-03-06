﻿using System;
using Android.App;
using Android.Views;
using NUnit.Framework;

using Genetics;
using GeneticsTests.TestCases;
using System.Linq;

namespace GeneticsTests
{
    [TestFixture]
    public class EventSpliceTests : TestBase
    {
        [Test]
        public void TypeMappingCreatedForAllSupportedMembers()
		{
            var typeMapping = Geneticist.GetTypeMapping(typeof(ExactEventSpliceTargetObject));
            Assert.AreEqual(1, typeMapping.Methods.Values.Count);
            Assert.AreEqual("ExactParametersMethod", typeMapping.Methods.Values.Single().Single().Method.Name);

            typeMapping = Geneticist.GetTypeMapping(typeof(SimilarEventSpliceTargetObject));
            Assert.AreEqual(1, typeMapping.Methods.Values.Count);
            Assert.AreEqual("SimilarParametersMethod", typeMapping.Methods.Values.Single().Single().Method.Name);

            typeMapping = Geneticist.GetTypeMapping(typeof(MultipleEventSpliceTargetObject));
            Assert.AreEqual(1, typeMapping.Methods.Values.Count);
            Assert.AreEqual(2, typeMapping.Methods.Values.Single().Count);
            Assert.AreEqual("MultipleMethod", typeMapping.Methods.Values.Single()[0].Method.Name);
            Assert.AreEqual("MultipleMethod", typeMapping.Methods.Values.Single()[1].Method.Name);

            typeMapping = Geneticist.GetTypeMapping(typeof(EventNotFoundTargetObject));
            Assert.AreEqual(1, typeMapping.Methods.Values.Count);
            Assert.AreEqual("MissingMethod", typeMapping.Methods.Values.Single().Single().Method.Name);
        }

        [Test]
        public void InvalidEventThrows()
		{
            var typeMapping = Geneticist.GetTypeMapping(typeof(InvalidEventSpliceTargetObject));
            Assert.AreEqual(1, typeMapping.Methods.Values.Count);

            var view = CreateView(Resource.Layout.SimpleLayout);

            var target = new InvalidEventSpliceTargetObject();

            var ex = Assert.Throws<SpliceException>(() =>
            {
                Geneticist.Splice(target, view, Application.Context);
            });
            Assert.AreEqual("Unable to find event 'ItemLongClick' for method 'InvalidEvent'.", ex.Message);
        }

        [Test]
        public void ViewNotFoundIsHandledCorrectly()
        {
            var view = CreateView(Resource.Layout.SimpleLayout);

            var target = new EventNotFoundTargetObject();

            Assert.Throws<SpliceException>(() =>
            {
                Geneticist.Splice(target, view, Application.Context);
            });
        }

        [Test]
        public void OptionalViewNotFoundIsHandledCorrectly()
        {
            var view = CreateView(Resource.Layout.SimpleLayout);

            var target = new OptionalEventNotFoundTargetObject();

            Geneticist.Splice(target, view, Application.Context);
        }

        [Test]
        public void ExactMatchMembersAreBound()
        {
            var view = CreateView(Resource.Layout.SimpleLayout);

            var target = new ExactEventSpliceTargetObject();
            Geneticist.Splice(target, view, Application.Context);
            var button = view.FindViewById(Resource.Id.simpleButton);

            var ex = Assert.Throws<NotImplementedException>(() =>
            {
                button.CallOnClick();
            });
            Assert.AreEqual("ExactParametersMethod", ex.Message);
        }

        [Test]
        public void NullSourceDoesNotThrow()
        {
            var target = new ExactEventSpliceTargetObject();
            Geneticist.Splice(target, null, Application.Context);
        }

        [Test]
        public void SimilarMatchMembersAreBound()
        {
            var view = CreateView(Resource.Layout.SimpleLayout);

            var target = new SimilarEventSpliceTargetObject();
            Geneticist.Splice(target, view, Application.Context);
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
            var target = new DifferentEventSpliceTargetObject();

            var ex = Assert.Throws<SpliceException>(() =>
            {
                Geneticist.Splice(target, view, Application.Context);
            });
            Assert.AreEqual("Error creating delegate from 'SenderPerameterMethod' for event 'Click'.", ex.Message);
        }

        [Test]
        public void ParameterlessMembersThrow()
        {
            var view = CreateView(Resource.Layout.SimpleLayout);
            var target = new ParameterlessEventSpliceTargetObject();

            var ex = Assert.Throws<SpliceException>(() =>
            {
                Geneticist.Splice(target, view, Application.Context);
            });
            Assert.AreEqual("Error creating delegate from 'ParameterlessMethod' for event 'Click'.", ex.Message);
        }
        
        private static View CreateView(int layout)
        {
            var activity = GeneticsTestsApplication.CurrentActivity;
            var parent = (ViewGroup)activity.FindViewById(Android.Resource.Id.Content);

            var inflater = LayoutInflater.FromContext(Application.Context);
            var view = inflater.Inflate(layout, parent, false);

            return view;
        }

        [Test]
        public void MultipleMembersAreBound()
        {
            var view = CreateView(Resource.Layout.SimpleLayout);

            var target = new MultipleEventSpliceTargetObject();
            Geneticist.Splice(target, view, Application.Context);
            var button = view.FindViewById(Resource.Id.simpleButton);
            var checkbox = view.FindViewById(Resource.Id.simpleCheckBox);

            var ex = Assert.Throws<NotImplementedException>(() =>
            {
                button.CallOnClick();
            });
            Assert.AreEqual("Button", ex.Message);

            ex = Assert.Throws<NotImplementedException>(() =>
            {
                checkbox.CallOnClick();
            });
            Assert.AreEqual("CheckBox", ex.Message);
        }
    }
}
