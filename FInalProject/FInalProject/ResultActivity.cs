using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FInalProject
{
    [Activity(Label = "Results")]
    public class ResultActivity : AppCompatActivity
    {
        public static List<Results> puzzleList { get; set; }
        ResultAdapter ResultAdapter;
        ListView listView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Result_layout);
            // Create your application here

            var db = new SQLiteConnection(Helper.Path2());
            db.CreateTable<Puzzle>();

            puzzleList = GetAllPuzzle();

            ResultAdapter = new ResultAdapter(this, puzzleList);
            listView = FindViewById<ListView>(Resource.Id.listView);
            listView.Adapter = ResultAdapter;
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



            //move to statistics
            if (item.ItemId == Resource.Id.action_statistics)
            {
                Intent intent = new Intent(this, typeof(StatisticActivity));
                StartActivity(intent);
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }



        /// <summary>
        /// get all puzzles from sql table
        /// </summary>
        public List<Results> GetAllPuzzle()
        {
            List<Results> PuzzlesList = new List<Results>();
            var db = new SQLiteConnection(Helper.Path2());

            string strsql = string.Format("SELECT * FROM Results");


            //tring to get result table
            try
            {
                var Puzzles = db.Query<Results>(strsql);

                foreach (var item in Puzzles)
                {
                    PuzzlesList.Add(item);
                }
            }


            //if result table was empty
            catch(Exception ex)
            {
                Toast.MakeText(this, "no results", ToastLength.Long).Show();
            }
            return PuzzlesList;
        }
    }
}