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
using SQLite;
using Android.Graphics;

namespace FInalProject
{
    [Table("Puzzles")]
    public class Puzzle
    {
        [PrimaryKey,AutoIncrement,Column("_id")]
        public int id { get; set; }
        public string name { get; set; }
        public string picture { get; set; }



        /// <summary>
        /// empty constructor
        /// </summary>
        public Puzzle()
        { }



        /// <summary>
        /// constructor without id
        /// </summary>
        public Puzzle(string name, string picture)
        {
            this.name = name;
            this.picture = picture;
        }



        /// <summary>
        /// constructor with id
        /// </summary>
        public Puzzle(int id, string name, string picture)
        {
            this.id = id;
            this.name = name;
            this.picture = picture;
        }



        /// <summary>
        /// change puzzle info
        /// </summary>
        public void SetPuzzle(int id, string name, string picture)
        {
            this.id = id;
            this.name = name;
            this.picture = picture;
        }

    }
}