using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Views.Animations;

using Syringe;

namespace SyringeSample
{
    [Activity(Label = "SyringeSample", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.DeviceDefault.NoActionBar")]
    public class MainActivity : Activity
    {
		// view injection fields
		[Inject(Resource.Id.title)] private TextView title;
		[Inject(Resource.Id.subtitle)] private TextView subtitle;
		[Inject(Resource.Id.hello)]  private Button hello;
		[Inject(Resource.Id.listview)]  private ListView listview;
		[Inject(Resource.Id.footer)]  private TextView footer;
        [Inject(Resource.Id.title, Resource.Id.subtitle, Resource.Id.hello)] private List<View> headerViews;

        // resource injection fields
        [Inject(Resource.String.titleText)] private string titleText;
        [Inject(Resource.String.subtitleText)] private string subtitleText;
        [Inject(Resource.String.sayHelloText)] private string sayHelloText;
        [Inject(Resource.String.authorText)] private string authorText;

        // normal fields
        private SimpleAdapter adapter;

		public MainActivity()
        {
            Needle.Debug = true;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

			// inject the views into the fields
            Needle.Inject(this);
            
            // Contrived code to use the bound fields.
            title.Text = titleText;
            subtitle.Text = subtitleText;
            footer.Text = "by " + authorText;
            hello.Text = sayHelloText;

            adapter = new SimpleAdapter(this);
            listview.Adapter = adapter;
        }

        //[InjectClick(Resource.Id.hello)]
        //private void OnSayHello()
        //{
        //    Toast.MakeText(this, "Hello, views!", ToastLength.Short).Show();
        //
        //    var index = 0;
        //    foreach (var view in headerViews)
        //    {
        //        var anim = new AlphaAnimation(0.0f, 1.0f);
        //        anim.FillBefore = true;
        //        anim.Duration = 500;
        //        anim.StartOffset = index++ * 100;
        //        view.StartAnimation(anim);
        //    }
        //}

        //[InjectLongClick(Resource.Id.hello)]
        //private bool OnSayGetOffMe()
        //{
        //    Toast.MakeText(this, "Let go of me!", ToastLength.Short).Show();
        //
        //    return true;
        //}

        //[InjectItemClick(Resource.Id.listview)]
        //private void OnItemClick(int position)
        //{
        //    Toast.MakeText(this, "You clicked: " + adapter[position], ToastLength.Short).Show();
        //}
    }
}
