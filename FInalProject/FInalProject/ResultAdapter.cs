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
    class ResultAdapter : BaseAdapter<Results>
    {
        Android.Content.Context context;
        List<Results> obj;
        public ResultAdapter(Android.Content.Context context, System.Collections.Generic.List<Results> obj)
        {
            this.context = context;
            this.obj = obj;
        }
        public List<Results> getList()
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
        public override Results this[int position]
        {
            get { return this.obj[position]; }
        }



        /// <summary>
        /// enter info to row of table
        /// </summary>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Android.Views.LayoutInflater layoutInflater = ((ResultActivity)context).LayoutInflater;
            Android.Views.View view = layoutInflater.Inflate(Resource.Layout.ResultList_layout ,parent, false);
            TextView PuzzleSize = view.FindViewById<TextView>(Resource.Id.PuzzleSize);
            TextView PuzzleTime = view.FindViewById<TextView>(Resource.Id.PuzzleTime);
            ImageView image = view.FindViewById<ImageView>(Resource.Id.PuzzleImageView);

            Results Temp = obj[position];
            if (Temp != null)
            {
                Android.Graphics.Bitmap b = Helper.Base64ToBitmap(Temp.picture);
                PuzzleSize.Text = Temp.size;
                PuzzleTime.Text = Temp.time;
                image.SetImageBitmap(b);
            }
            return view;
        }
    }
}