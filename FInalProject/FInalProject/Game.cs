using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Java.IO;
using SQLite;
using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace FInalProject
{
    [Activity(Label = "Game")]
    public class Game : AppCompatActivity
    {
        Android.Content.ISharedPreferences sp;
        ///
        Stopwatch timer;
        ///
        TextView piece;
        LinearLayout linearlayout, linear1, linear2, linear3;
        LinearLayout.LayoutParams lp1, lp2, lp3;
        //
        ImageView[] _img;
        ImageView pieceSelected;
        //
        Bitmap[] imgs;
        Bitmap[] newImgs;
        Bitmap[] temp;
        Bitmap[] correct;
        Bitmap i0, i1, i2, i3, i4, i5, i6, i7, i8, i9, i10, i11, i12, i13, i14, i15, _thePicture;
        //
        int[] place;
        int first = 0, second = 0, pos = 0, width = 0, height = 0, pieces = 0;
        bool firstclick = false, secondclick = false, stop = false, finish = true;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Game_layout);
            //
            timer = new Stopwatch();
            timer.Start();
            //
            linearlayout = FindViewById<LinearLayout>(Resource.Id.linearLayout);
            //
            _thePicture = Helper.Base64ToBitmap(Intent.GetStringExtra("pic"));//get the picture
            pieces = Int32.Parse(Intent.GetStringExtra("pieces"));//get board size


            if (pieces == 4)
            { l1(); }
            else if (pieces == 9)
            { l2(); }
            else if (pieces == 16)
            { l3(); }
        }



        /// <summary>
        /// function when player puit the game
        /// </summary>
        private void quitClick(object sender, EventArgs e)
        {
            //add to the failed puzzles on shared preferenc
            sp = this.GetSharedPreferences("details", Android.Content.FileCreationMode.Private);
            string strFail = sp.GetString("Fail", null);
            var editor = sp.Edit();


            //if its the first lose
            if (strFail == null)
            {
                editor.PutString("Fail", "1");
            }
            //if its not the first lose
            else
            {
                int fail = int.Parse(strFail);
                fail++;
                editor.PutString("Fail", fail.ToString());
            }
            editor.Commit();



            //move to statistics activity
            Intent intent = new Intent(this, typeof(StatisticActivity));
            StartActivity(intent);
        }

        /// <summary>
        /// check if the game is finished
        /// </summary>
        private void onFinish()
        {
            finish = true;



            //check if all pieces are in place
            for (int i = 0; i < pieces; i++)
            {
                if (_img[i].Clickable == true)
                {
                    finish = false;
                }
            }



            //when game is finished
            if (finish == true)
            {
                //get time it took to finish the puzzle
                timer.Stop();
                TimeSpan timeTaken = timer.Elapsed;
                string time = timeTaken.ToString(@"h\:mm\:ss");
                Toast.MakeText(this, time, ToastLength.Long).Show();



                //add to the succeded puzzles on shared preferenc
                sp = this.GetSharedPreferences("details", Android.Content.FileCreationMode.Private);
                string strSuccess = sp.GetString("Success", null);
                var editor = sp.Edit();


                //if its the first win
                if (strSuccess == null)
                {
                    editor.PutString("Success", "1");
                }
                //if its not the first win
                else
                {
                    int success = int.Parse(strSuccess);
                    success++;
                    editor.PutString("Success", success.ToString());
                }
                editor.Commit();



                //enter the puzzle info (size,time,picture) to sql of results
                var db = new SQLiteConnection(Helper.Path2());
                db.CreateTable<Results>();
                string size = (Math.Sqrt(pieces)).ToString() + "x" + (Math.Sqrt(pieces)).ToString();
                Results r = new Results(time, size, Helper.BitmapToBase64(_thePicture));
                db.Insert(r);



                //move to reuslt activity
                Intent intent = new Intent(this, typeof(ResultActivity));
                StartActivity(intent);
            }
            //if game isnt finished
            else
            {
                finish = false;
            }
        }



        /// <summary>
        /// create board of levle 1
        /// </summary>
        public void l1()
        {
            imgs = new Bitmap[4];
            newImgs = new Bitmap[4];
            temp = new Bitmap[4];
            correct = new Bitmap[4];
            //
            _img = new ImageView[4];
            place = new int[4] { -1, -1, -1, -1 };
            width = _thePicture.Width / 2;
            height = _thePicture.Height / 2;
            pos = 0;



            //create board
            for (int x = 0; x < 2; ++x)
            {
                //create row of puzzle
                linear1 = new LinearLayout(this);
                linear1.Orientation = Orientation.Horizontal;
                linear1.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, 400);
                linearlayout.AddView(linear1);
                lp1 = new LinearLayout.LayoutParams(500, 400);
                lp1.SetMargins(20, 20, 20, 20);


                //add pieces to row of puzzle
                for (int y = 0; y < 2; ++y)
                {
                    imgs[pos] = Bitmap.CreateBitmap(_thePicture, x * width, y * height, width, height);
                    _img[pos] = new ImageView(this);
                    _img[pos].Clickable = true;
                    _img[pos].SetScaleType(ImageView.ScaleType.FitXy);
                    _img[pos].Click += new EventHandler(level);
                    linear1.AddView(_img[pos], lp1);
                    pos++;
                }
            }


            //show the last piece selected
            linear2 = new LinearLayout(this);
            linear2.Orientation = Orientation.Horizontal;
            linear2.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, 400);
            linearlayout.AddView(linear2);

            lp2 = new LinearLayout.LayoutParams(500, Android.Views.ViewGroup.LayoutParams.MatchParent);
            lp2.SetMargins(20, 100, 0, 0);
            ///
            piece = new TextView(this);
            piece.Text = "piece selected: ";
            piece.TextSize = 20;
            linear2.AddView(piece, lp2);
            ///
            pieceSelected = new ImageView(this);
            pieceSelected.SetScaleType(ImageView.ScaleType.FitXy);
            linear2.AddView(pieceSelected, lp2);



            //add quit button
            linear3 = new LinearLayout(this);
            linear3.Orientation = Orientation.Horizontal;
            linear3.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, 300);
            linearlayout.AddView(linear3);

            lp3 = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.WrapContent);
            lp3.SetMargins(40, 50, 40, 0);

            Button button = new Button(this);
            button.Text = "quit";
            button.TextSize = 20;
            button.SetBackgroundColor(Color.Red);
            button.Click += new EventHandler(quitClick);
            linear3.AddView(button, lp3);



            //switch pieces to correct place
            i0 = imgs[0];
            i1 = imgs[1];
            i2 = imgs[2];
            i3 = imgs[3];
            imgs[1] = i2;
            imgs[2] = i1;


            //save the correct order
            for (int i = 0; i < 4; i++)
            {
                correct[i] = imgs[i];
            }
            //randomize pieces place
            for (int i = 0; i < 4; i++)
            {
                //check that piece isnt in the new order and not in the correct place
                while (stop != true)
                {
                    Random rnd = new Random();
                    int k = rnd.Next(0, 4);
                    if ((!(place.Contains(k))) && k != i)
                    {
                        place[i] = k;
                        stop = true;
                    }
                }
                stop = false;
            }
            //create random board
            for (int i = 0; i < 4; i++)
            {
                newImgs[i] = imgs[place[i]];
            }
            //put new images in the old board
            for (int i = 0; i < 4; i++)
            {
                imgs[i] = newImgs[i];
            }
            //show put new board
            for (int i = 0; i < 4; i++)
            {
                _img[i].SetImageBitmap(imgs[i]);
            }
        }



        /// <summary>
        /// create board of levle 2
        /// </summary>
        public void l2()
        {
            imgs = new Bitmap[9];
            newImgs = new Bitmap[9];
            temp = new Bitmap[9];
            correct = new Bitmap[9];
            //
            _img = new ImageView[9];
            place = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            width = _thePicture.Width / 3;
            height = _thePicture.Height / 3;
            pos = 0;



            //create board
            for (int x = 0; x < 3; ++x)
            {
                //create row of puzzle
                linear1 = new LinearLayout(this);
                linear1.Orientation = Orientation.Horizontal;
                linear1.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, 300);
                linearlayout.AddView(linear1);
                lp1 = new LinearLayout.LayoutParams(320, 300);
                lp1.SetMargins(20, 20, 20, 20);


                //add pieces to row of puzzle
                for (int y = 0; y < 3; ++y)
                {
                    imgs[pos] = Bitmap.CreateBitmap(_thePicture, x * width, y * height, width, height);
                    _img[pos] = new ImageView(this);
                    _img[pos].Clickable = true;
                    _img[pos].SetScaleType(ImageView.ScaleType.FitXy);
                    _img[pos].Click += new EventHandler(level);
                    linear1.AddView(_img[pos], lp1);
                    pos++;
                }
            }



            //show the last piece selected
            linear2 = new LinearLayout(this);
            linear2.Orientation = Orientation.Horizontal;
            linear2.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, 300);
            linearlayout.AddView(linear2);

            lp2 = new LinearLayout.LayoutParams(320, 300);
            lp2.SetMargins(140, 100, 0, 0);
            ///
            piece = new TextView(this);
            piece.Text = "piece selected: ";
            piece.TextSize = 20;
            linear2.AddView(piece, lp2);
            ///
            pieceSelected = new ImageView(this);
            pieceSelected.SetScaleType(ImageView.ScaleType.FitXy);
            linear2.AddView(pieceSelected, lp2);



            //add quit button
            linear3 = new LinearLayout(this);
            linear3.Orientation = Orientation.Horizontal;
            linear3.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, 300);
            linearlayout.AddView(linear3);

            lp3 = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.WrapContent);
            lp3.SetMargins(40, 50, 40, 0);

            Button button = new Button(this);
            button.Text = "quit";
            button.TextSize = 20;
            button.SetBackgroundColor(Color.Red);
            button.Click += new EventHandler(quitClick);
            linear3.AddView(button, lp3);



            //switch pieces to correct place
            i0 = imgs[0];
            i1 = imgs[1];
            i2 = imgs[2];
            i3 = imgs[3];
            i4 = imgs[4];
            i5 = imgs[5];
            i6 = imgs[6];
            i7 = imgs[7];
            i8 = imgs[8];
            imgs[1] = i3;
            imgs[3] = i1;
            imgs[2] = i6;
            imgs[6] = i2;
            imgs[5] = i7;
            imgs[7] = i5;



            //save the correct order
            for (int i = 0; i < 9; i++)
            {
                correct[i] = imgs[i];
            }
            //randomize pieces place
            for (int i = 0; i < 9; i++)
            {
                //check that piece isnt in the new order and not in the correct place
                while (stop != true)
                {
                    Random rnd = new Random();
                    int k = rnd.Next(0, 9);
                    if ((!(place.Contains(k))) && k != i)
                    {
                        place[i] = k;
                        stop = true;
                    }
                }
                stop = false;
            }
            //create random board
            for (int i = 0; i < 9; i++)
            {
                newImgs[i] = imgs[place[i]];
            }
            //put new images in the old board
            for (int i = 0; i < 9; i++)
            {
                imgs[i] = newImgs[i];
            }
            //show put new board
            for (int i = 0; i < 9; i++)
            {
                _img[i].SetImageBitmap(imgs[i]);
            }
        }



        /// <summary>
        /// create board of levle 3
        /// </summary>
        public void l3()
        {
            imgs = new Bitmap[16];
            newImgs = new Bitmap[16];
            temp = new Bitmap[16];
            correct = new Bitmap[16];
            //
            _img = new ImageView[16];
            place = new int[16] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            width = _thePicture.Width / 4;
            height = _thePicture.Height / 4;
            pos = 0;



            //create board
            for (int x = 0; x < 4; ++x)
            {
                //create row of puzzle
                linear1 = new LinearLayout(this);
                linear1.Orientation = Orientation.Horizontal;
                linear1.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, 300);
                linearlayout.AddView(linear1);
                lp1 = new LinearLayout.LayoutParams(230, 300);
                lp1.SetMargins(20, 20, 20, 20);



                //add pieces to row of puzzle
                for (int y = 0; y < 4; ++y)
                {

                    imgs[pos] = Bitmap.CreateBitmap(_thePicture, x * width, y * height, width, height);
                    _img[pos] = new ImageView(this);
                    _img[pos].Clickable = true;
                    _img[pos].SetScaleType(ImageView.ScaleType.FitXy);
                    _img[pos].Click += new EventHandler(level);
                    linear1.AddView(_img[pos], lp1);
                    pos++;
                }
            }



            //show the last piece selected
            linear2 = new LinearLayout(this);
            linear2.Orientation = Orientation.Horizontal;
            linear2.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, 300);
            linearlayout.AddView(linear2);

            lp2 = new LinearLayout.LayoutParams(300, 300);
            lp2.SetMargins(140, 100, 40, 0);
            ///
            piece = new TextView(this);
            piece.Text = "piece \n selected: ";
            piece.TextSize = 20;
            linear2.AddView(piece, lp2);
            ///
            pieceSelected = new ImageView(this);
            pieceSelected.SetScaleType(ImageView.ScaleType.FitXy);
            linear2.AddView(pieceSelected, lp2);



            //add quit button
            linear3 = new LinearLayout(this);
            linear3.Orientation = Orientation.Horizontal;
            linear3.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, 300);
            linearlayout.AddView(linear3);

            lp3 = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.WrapContent);
            lp3.SetMargins(40, 50, 40, 0);

            Button button = new Button(this);
            button.Text = "quit";
            button.TextSize = 20;
            button.SetBackgroundColor(Color.Red);
            button.Click += new EventHandler(quitClick);
            linear3.AddView(button, lp3);



            //switch pieces to correct place
            i0 = imgs[0];
            i1 = imgs[1];
            i2 = imgs[2];
            i3 = imgs[3];
            i4 = imgs[4];
            i5 = imgs[5];
            i6 = imgs[6];
            i7 = imgs[7];
            i8 = imgs[8];
            i9 = imgs[9];
            i10 = imgs[10];
            i11 = imgs[11];
            i12 = imgs[12];
            i13 = imgs[13];
            i14 = imgs[14];
            i15 = imgs[15];

            imgs[1] = i4;
            imgs[4] = i1;
            imgs[2] = i8;
            imgs[8] = i2;
            imgs[3] = i12;
            imgs[12] = i3;
            imgs[6] = i9;
            imgs[9] = i6;
            imgs[7] = i13;
            imgs[13] = i7;
            imgs[11] = i14;
            imgs[14] = i11;



            //save the correct order
            for (int i = 0; i < 16; i++)
            {
                correct[i] = imgs[i];
            }
            //randomize pieces place
            for (int i = 0; i < 16; i++)
            {
                //check that piece isnt in the new order and not in the correct place
                while (stop != true)
                {
                    Random rnd = new Random();
                    int k = rnd.Next(0, 16);
                    if ((!(place.Contains(k))) && k != i)
                    {
                        place[i] = k;
                        stop = true;
                    }
                }
                stop = false;
            }
            //create random board
            for (int i = 0; i < 16; i++)
            {
                newImgs[i] = imgs[place[i]];
            }
            //put new images in the old board
            for (int i = 0; i < 16; i++)
            {
                imgs[i] = newImgs[i];
            }
            //show put new board
            for (int i = 0; i < 16; i++)
            {
                _img[i].SetImageBitmap(imgs[i]);
            }
        }



        /// <summary>
        /// switch pieces and disable them if they are in the correct place
        /// </summary>
        private void level(object sender, EventArgs e)
        {
            //get image clicked
            ImageView v = sender as ImageView;


            //check whichplace is the piece on the board
            for (int i = 0; i < pieces; i++)
            {
                if (v == _img[i])
                {
                    //check if its the first piece to be selected
                    if (firstclick == false)
                    {
                        first = i;
                        firstclick = true;
                    }
                    //check if its the second piece to be selected
                    else if (secondclick == false)
                    {
                        second = i;
                        secondclick = true;
                    }
                }
            }



            //show the first piece that was selected
            if (firstclick == true && secondclick == false)
            {
                pieceSelected.SetImageBitmap(imgs[first]);
            }



            //switch places of pieces
            if (firstclick == true && secondclick == true)
            {
                Bitmap b1 = imgs[first];
                Bitmap b2 = imgs[second];

                imgs[first] = b2;
                imgs[second] = b1;



                //set board order
                for (int i = 0; i < pieces; ++i)
                {

                    _img[i].SetImageBitmap(imgs[i]);

                }
                firstclick = false;
                secondclick = false;



                //make piece in the right places not clickable
                for (int i = 0; i < pieces; ++i)
                {

                    if (correct[i] == imgs[i])
                    {
                        _img[i].Clickable = false;
                    }
                    pos++;

                }
                pieceSelected.SetImageBitmap(null);

            }

            //check if game ended
            onFinish();
        }
    }
}