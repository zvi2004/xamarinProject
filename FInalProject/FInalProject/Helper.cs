using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FInalProject
{
    class Helper
    {
        public static string dbname = "dbTable";
        public static string dbResult = "dbResult";
        public Helper()
        { }



        /// <summary>
        /// create path of puzzle table
        /// </summary>
        public static string Path()
        {
            string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbname);
            return path;
        }



        /// <summary>
        /// create path of result table
        /// </summary>
        public static string Path2()
        {
            string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbResult);
            return path;
        }



        /// <summary>
        /// turn image to string
        /// </summary>
        public static string BitmapToBase64(Bitmap bitmap)
        {
            string str = "";
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                var bytes = stream.ToArray();
                str = Convert.ToBase64String(bytes);
            }
            return str;
        }



        /// <summary>
        /// turn string to image
        /// </summary>
        public static Bitmap Base64ToBitmap(string base64String)
        {
            byte[] imageAsBytes = Base64.Decode(base64String, Base64Flags.Default);
            return BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);
        }
    }
}