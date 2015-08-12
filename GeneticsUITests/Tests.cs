using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;

namespace GeneticsUITests
{
    [TestFixture]
    public class Tests
    {
        private AndroidApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            // TODO: If the Android app being tested is included in the solution then open
            // the Unit Tests window, right click Test Apps, select Add App Project
            // and select the app projects that should be tested.
            app = ConfigureApp
                .Android
				.ApkFile("../../../GeneticsTests/bin/Release/com.example.genetics.GeneticsTests.apk")
                .StartApp();
        }

        [Test]
        public void RunTests()
        {
            //app.Repl();

			// start the tests
			app.WaitForElement (e => e.Id ("RunTestsButton"));
			app.Tap (e => e.Id ("RunTestsButton"));

			// wait for results
			app.WaitForElement (e => e.Id ("ResultsResult"), timeout: TimeSpan.FromMinutes(5)); // to run all the tests
			var resultsText = app.Query (e => e.Id ("ResultsResult")).FirstOrDefault ();
			Assert.IsNotNull (resultsText);
			app.WaitFor (() => !string.IsNullOrEmpty (resultsText.Text));

			// take a screenshot to confirm
			app.Screenshot("Results");

			// make sure all passed
			Assert.AreEqual ("Passed", resultsText.Text);
        }
    }
}
