using System;
using System.Reflection;
using Android.App;
using Android.OS;
using Android.Runtime;
using Xamarin.Android.NUnitLite;

namespace GeneticsTests
{
    [Activity(Label = "GeneticsTests", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : TestSuiteActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            Console.WriteLine("activity created.");
            // tests can be inside the main assembly
            AddTest(Assembly.GetExecutingAssembly());
            // or in any reference assemblies
            // AddTest (typeof (Your.Library.TestClass).Assembly);

            // Once you called base.OnCreate(), you cannot add more assemblies.
            base.OnCreate(bundle);
        }
    }

    [Application]
    public class GeneticsTestsApplication : Application, Application.IActivityLifecycleCallbacks
    {
        protected GeneticsTestsApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            RegisterActivityLifecycleCallbacks(this);
        }

        public static Activity CurrentActivity { get; private set; }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
            CurrentActivity = null;
        }

        public void OnActivityResumed(Activity activity)
        {
            CurrentActivity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}
