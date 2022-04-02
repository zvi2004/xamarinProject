using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FInalProject
{
    [Table("Results")]
    public class Results
    {
        [PrimaryKey, AutoIncrement, Column("_id")]

        public int id { get; set; }
        public string time { get; set; }
        public string size { get; set; }
        public string picture { get; set; }



        /// <summary>
        /// empty constructor
        /// </summary>
        public Results()
        { }



        /// <summary>
        /// constructor without id
        /// </summary>
        public Results(string time, string size, string picture)
        {
            this.time = time;
            this.size = size;
            this.picture = picture;
        }



        /// <summary>
        /// constructor with id
        /// </summary>
        public Results(int id, string time, string size, string picture)
        {
            this.id = id;
            this.time = time;
            this.size = size;
            this.picture = picture;
        }
    }
}