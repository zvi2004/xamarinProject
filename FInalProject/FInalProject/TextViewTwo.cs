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
    class TextViewTwo:TextViewOne
    {
        public ButtonText button = new ButtonText();
        public string tv2 = "click on the button below to go to explanation video about how to play the correct way.";
        public override string getString()
        {
            return tv2;
        }
    }
}