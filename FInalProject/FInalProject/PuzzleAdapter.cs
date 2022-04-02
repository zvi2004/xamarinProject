using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FInalProject
{
    class PuzzleAdapter:BaseAdapter<Puzzle>
    {
        Android.Content.Context context;
        List<Puzzle> obj;
        public PuzzleAdapter(Android.Content.Context context, System.Collections.Generic.List<Puzzle> obj)
        {
            this.context = context;
            this.obj = obj;
        }
        public List<Puzzle> getList()
        {
            return this.obj;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override int Count
        {
            get { return this.obj.Count; }
        }
        public override Puzzle this[int position]
        {
            get { return this.obj[position]; }
        }



        /// <summary>
        /// enter info to row of table
        /// </summary>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Android.Views.LayoutInflater layoutInflater = ((PickPuzzle)context).LayoutInflater;
            Android.Views.View view = layoutInflater.Inflate(Resource.Layout.PuzzleList_layout, parent, false);
            TextView PuzzleName = view.FindViewById<TextView>(Resource.Id.PuzzleName);
            ImageView image = view.FindViewById<ImageView>(Resource.Id.PuzzleImageView);

            Puzzle Temp = obj[position];
            if (Temp != null)
            {
                Android.Graphics.Bitmap b = Helper.Base64ToBitmap(Temp.picture);
                PuzzleName.Text = Temp.name;
                image.SetImageBitmap(b);
            }
            return view;
        }
    }
}