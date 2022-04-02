using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FInalProject
{
    [Activity(Label = "Statistics")]
    public class StatisticActivity : AppCompatActivity
    {
        Android.Content.ISharedPreferences sp;
        TextView success, fail;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Statistics_layout);
            // Create your application here

            fail = FindViewById<TextView>(Resource.Id.fail);
            success = FindViewById<TextView>(Resource.Id.success);

            sp = this.GetSharedPreferences("details", Android.Content.FileCreationMode.Private);
            string strFail = sp.GetString("Fail", null);
            string strSuccess = sp.GetString("Success", null);
            //no wins
            if (strFail == null)
            {
                fail.Text = "0";
            }
            else
            {
                fail.Text = strFail;
            }


            //no lose
            if (strSuccess == null)
            {
                success.Text = "0";
            }
            else
            {
                success.Text = strSuccess;
            }
        }



        /// <summary>
        /// create the menu
        /// </summary>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return true;
        }



        /// <summary>
        /// create event for every option on the menu
        /// </summary>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            //move to pick puzzle
            if (item.ItemId == Resource.Id.action_pickPuzzle)
            {
                Intent intent = new Intent(this, typeof(PickPuzzle));
                StartActivity(intent);
                return true;
            }



            //move to results
            if (item.ItemId == Resource.Id.action_results)
            {
                Intent intent = new Intent(this, typeof(ResultActivity));
                StartActivity(intent);
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}