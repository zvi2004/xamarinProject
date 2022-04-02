using Android.App;
using Android.Content;
using Android.Graphics;
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
    [Activity(Label = "Edit info / add picture")]
    public class EditActivity : AppCompatActivity
    {
        Button btnSave, btnAddPic;
        EditText etName;
        ImageView iv;
        Bitmap bitmap;
        int pos = -1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditInfo_layout);
            
            iv = FindViewById<ImageView>(Resource.Id.ivProduct);
            btnSave = FindViewById<Button>(Resource.Id.btnSave);
            etName= FindViewById<EditText>(Resource.Id.etName);
            pos = Intent.GetIntExtra("pos", -1);
            btnAddPic = FindViewById<Button>(Resource.Id.btnAddPic);

            btnSave.Click += BtnSave_Click;
            btnAddPic.Click += BtnTakePic_Click;



            //show puzzle to user
            if (pos != -1)
            {
                Puzzle temp = PickPuzzle.puzzleList[pos];
                etName.Text = temp.name;
                iv.SetImageBitmap(Helper.Base64ToBitmap(temp.picture));
            }
        }



        /// <summary>
        /// function to take picture 
        /// </summary>
        private void BtnTakePic_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Android.Provider.MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);
        }



        /// <summary>
        /// function to save changes or save new image
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            string name = etName.Text;
            Puzzle puzzle = null;
            var db = new SQLiteConnection(Helper.Path());
            

            //update picture and name
            if (pos != -1)
            {
                puzzle = PickPuzzle.puzzleList[pos];
                if (bitmap == null)
                {
                    bitmap = Helper.Base64ToBitmap(puzzle.picture);
                }
                if (name != "")
                {
                    PickPuzzle.puzzleList[pos].SetPuzzle(puzzle.id, name, Helper.BitmapToBase64(bitmap));
                    PickPuzzle.puzzleList[pos] = puzzle;
                    db.Update(puzzle);
                    Finish();
                }
            }


            //create picture and name
            else
            {
                if (name != "" && bitmap != null)
                {
                    puzzle = new Puzzle(name, Helper.BitmapToBase64(bitmap));
                    db.Insert(puzzle);
                    PickPuzzle.puzzleList.Add(puzzle);
                    Finish();
                }
            }
        }



        /// <summary>
        /// function to take the picture and display it to the user
        /// </summary>
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 0)
            {
                if (resultCode == Result.Ok)
                {
                    bitmap = (Android.Graphics.Bitmap)data.Extras.Get("data");
                    iv.SetImageBitmap(bitmap);
                }
            }
        }
    }
}