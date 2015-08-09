using Android.Content;
using Android.Views;
using Android.Widget;

using Genetics;
using Genetics.Attributes;

namespace GeneticsSample
{
    public class SimpleAdapter : BaseAdapter<string>
    {
        [Splice(Resource.Array.listContents)]
        private string[] contents;
        private readonly LayoutInflater inflater;

        public SimpleAdapter(Context context)
        {
            Geneticist.Splice(this, context);

            inflater = LayoutInflater.FromContext(context);
        }

        public override string this[int position]
        {
            get { return contents[position]; }
        }

        public override int Count
        {
            get { return contents.Length; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // get or create a new view holder
            ViewHolder holder;
            if (convertView != null)
            {
                holder = (ViewHolder)convertView.Tag;
            }
            else
            {
                convertView = inflater.Inflate(Resource.Layout.SimpleListItem, parent, false);
                holder = new ViewHolder(convertView);
                convertView.Tag = holder;
            }

            // set the values of the view
            string word = this[position];
            holder.word.Text = "Word: " + word;
            holder.length.Text = "Length: " + word.Length;
            holder.position.Text = "Position: " + position;

            // return the final view
            return convertView;
        }

        private class ViewHolder : Java.Lang.Object
        {
            [Splice(Resource.Id.word)] public TextView word;
            [Splice(Resource.Id.length)] public TextView length;
            [Splice(Resource.Id.position)] public TextView position;

            public ViewHolder(View view)
            {
                Geneticist.Splice(this, view);
            }
        }
    }
}
