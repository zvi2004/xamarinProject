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
    public class MyHandler:Handler
    {
        Context context;

        public MyHandler(Context context)
        {
            this.context = context;
        }
    }
}