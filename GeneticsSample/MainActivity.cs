using System;
using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Views.Animations;

using Genetics;
using Genetics.Attributes;

namespace GeneticsSample
{
    [Activity(Label = "GeneticsSample", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.DeviceDefault.NoActionBar")]
    public class MainActivity : Activity
    {
		// view splice fields
		[Splice(Resource.Id.title)] private TextView title;
		[Splice(Resource.Id.subtitle)] private TextView subtitle;
		[Splice(Resource.Id.hello)]  private Button hello;
		[Splice(Resource.Id.listview)]  private ListView listview;
		[Splice(Resource.Id.footer)]  private TextView footer;

        // resource splice fields
        [Splice(Resource.String.titleText)] private string titleText;
        [Splice(Resource.String.subtitleText)] private string subtitleText;
        [Splice(Resource.String.sayHelloText)] private string sayHelloText;
        [Splice(Resource.String.authorText)] private string authorText;

        // normal fields
        private SimpleAdapter adapter;
        private List<View> headerViews;

        public MainActivity()
        {
            Geneticist.Debug = true;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            // splice the views into the fields
            Geneticist.Splice(this);

            // Contrived code to use the bound fields.
            title.Text = titleText;
            subtitle.Text = subtitleText;
            footer.Text = "by " + authorText;
            hello.Text = sayHelloText;

            headerViews = new List<View> { title, subtitle };

            adapter = new SimpleAdapter(this);
            listview.Adapter = adapter;
        }

        protected override void OnDestroy()
        {
            Geneticist.Sever(this);

            base.OnDestroy();
        }

        [SpliceClick(Resource.Id.hello)]
        private void OnSayHello(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Hello, views!", ToastLength.Short).Show();

            var index = 0;
            foreach (var view in headerViews)
            {
                var anim = new AlphaAnimation(0.0f, 1.0f);
                anim.FillBefore = true;
                anim.Duration = 500;
                anim.StartOffset = index++ * 100;
                view.StartAnimation(anim);
            }
        }

        [SpliceLongClick(Resource.Id.hello)]
        private void OnSayGetOffMe(object sender, View.LongClickEventArgs e)
        {
            Toast.MakeText(this, "Let go of me!", ToastLength.Short).Show();
            e.Handled = true;
        }

        [SpliceItemClick(Resource.Id.listview)]
        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, "You clicked: " + adapter[e.Position], ToastLength.Short).Show();
        }
    }
}
