using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using System;
using SQLite;
using Android.Content;
using Android.Widget;
using Android.Graphics;
using System.Collections.Generic;
using Android.Views;
using AlertDialog = Android.App.AlertDialog;

namespace FInalProject
{
    [Activity(Label = "pick puzzle", Theme = "@style/AppTheme")]
    public class PickPuzzle : AppCompatActivity, ListView.IOnItemClickListener, ListView.IOnItemLongClickListener, Android.Widget.RadioGroup.IOnCheckedChangeListener
    {
        int pos;
        public static List<Puzzle> puzzleList { get; set; }
        PuzzleAdapter PuzzleAdapter;
        ListView listView;
        Button btnAdd;
        RadioGroup radioGroup;
        bool o1 = true, o2 = false, o3 = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Puzzle_layout);

            var db = new SQLiteConnection(Helper.Path());
            db.CreateTable<Puzzle>();

            puzzleList = GetAllPuzzle();

            PuzzleAdapter = new PuzzleAdapter(this, puzzleList);

            radioGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup);
            radioGroup.SetOnCheckedChangeListener(this);

            btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            btnAdd.Click += btnAdd_Click;
            listView = FindViewById<ListView>(Resource.Id.listView);
            listView.Adapter = PuzzleAdapter;

            listView.OnItemClickListener = this;
            listView.OnItemLongClickListener = this;

            string strsql = string.Format("SELECT * FROM Puzzles");
            var Puzzles = db.Query<Puzzle>(strsql);


            //add puzzles to sql table if table is empty
            if (Puzzles.Count == 0)
            {
                Bitmap image1 = BitmapFactory.DecodeResource(Resources, Resource.Drawable.first);
                Puzzle p1 = new Puzzle("0", Helper.BitmapToBase64(image1));

                Bitmap image2 = BitmapFactory.DecodeResource(Resources, Resource.Drawable.second);
                Puzzle p2 = new Puzzle("1", Helper.BitmapToBase64(image2));

                db.Insert(p1);
                db.Insert(p2);
                PickPuzzle.puzzleList.Add(p1);
                PickPuzzle.puzzleList.Add(p2);
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
            //move to pick results
            if (item.ItemId == Resource.Id.action_results)
            {
                Intent intent = new Intent(this, typeof(ResultActivity));
                StartActivity(intent);
                return true;
            }


            //move to pick statistics
            if (item.ItemId == Resource.Id.action_statistics)
            {
                Intent intent = new Intent(this, typeof(StatisticActivity));
                StartActivity(intent);
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }



        /// <summary>
        /// move to adding picture to table
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(EditActivity));
            StartActivity(intent);
        }



        /// <summary>
        /// when return to page
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            PuzzleAdapter.NotifyDataSetChanged();
        }



        /// <summary>
        /// get all puzzles from sql table
        /// </summary>
        public List<Puzzle> GetAllPuzzle()
        {
            List<Puzzle> PuzzlesList = new List<Puzzle>();
            var db = new SQLiteConnection(Helper.Path());

            string strsql = string.Format("SELECT * FROM Puzzles");
            var Puzzles = db.Query<Puzzle>(strsql);


            //add all puzzles to puzzle list
            foreach (var item in Puzzles)
            {
                PuzzlesList.Add(item);
            }
            return PuzzlesList;
        }



        /// <summary>
        /// function to move to change info of row in table
        /// </summary>
        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            Intent intent = new Intent(this, typeof(EditActivity));
            Puzzle temp = PickPuzzle.puzzleList[position];
            intent.PutExtra("pos", position);
            StartActivity(intent);
        }



        /// <summary>
        /// function to go play or delete image
        /// </summary>
        public bool OnItemLongClick(AdapterView parent, View view, int position, long id)
        {
            pos = position;
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("play or deleate");
            builder.SetMessage("play or deleate");
            builder.SetCancelable(true);

            builder.SetPositiveButton("play", PlayAction);
            builder.SetNegativeButton("deleate", DeleateAction);
            AlertDialog dialog = builder.Create();
            dialog.Show();

            return true;
        }



        /// <summary>
        /// delete picture from list and sql table
        /// </summary>
        private void DeleateAction(object sender, DialogClickEventArgs e)
        {
            var db = new SQLiteConnection(Helper.Path());
            db.Delete<Puzzle>(puzzleList[pos].id);
            PickPuzzle.puzzleList.RemoveAt(pos);
            PuzzleAdapter.NotifyDataSetChanged();
        }



        /// <summary>
        /// starting the game
        /// </summary>
        private void PlayAction(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(Game));


            //get the size of puzzle to create
            if (o1 == true)
            {
                intent.PutExtra("pieces", "4");
            }
            else if (o2 == true)
            {
                intent.PutExtra("pieces", "9");
            }
            else if (o3 == true)
            {
                intent.PutExtra("pieces", "16");
            }


            Puzzle temp = PickPuzzle.puzzleList[pos];
            intent.PutExtra("pic", temp.picture);
            StartActivity(intent);
        }



        /// <summary>
        /// change the option of board size
        /// </summary>
        public void OnCheckedChanged(RadioGroup group, int checkedId)
        {
            if (Resource.Id.radioGroup == group.Id)
            {
                //board size 2x2
                if (Resource.Id.radioButton1 == checkedId)
                {
                    o1 = true;
                    o2 = false;
                    o3 = false;
                }


                //board size 3x3
                else if (Resource.Id.radioButton2 == checkedId)
                {
                    o1 = false;
                    o2 = true;
                    o3 = false;
                }


                //board size 4x4
                else if (Resource.Id.radioButton3 == checkedId)
                {
                    o1 = false;
                    o2 = false;
                    o3 = true;
                }
            }
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}