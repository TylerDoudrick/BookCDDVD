﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// For serialization
using System.Runtime.Serialization.Formatters.Binary;
using BookCDDVD;

namespace BookCDDVD
{
    [Serializable()]
    class DVD : Product
    {
        string hiddenLeadActor = "";
        DateTime hiddenReleaseDate = new DateTime();
        int hiddenRunTime = 0;


        //Takes a dictionary as a parameter which contains everything unique to it and not it's base class (Product)
        public DVD(int UPC, decimal price, string title, int quantity, IDictionary<string, string> param) : base(UPC, price, title, quantity)
        {
            hiddenLeadActor = param["DVDLeadActor"];
            hiddenReleaseDate = DateTime.Parse(param["DVDReleaseDate"]);
            hiddenRunTime = Int32.Parse(param["DVDRuntime"]);
        }

        public string getActor()
        {
            return hiddenLeadActor;
        }
        public DateTime getReleaseDate()
        {
            return hiddenReleaseDate;
        }

        public int getRunTime()
        {
            return hiddenRunTime;
        }

        public override string ToString()
        {
            string s = base.ToString() + "\n";
            s += "DVD Lead Actor: " + hiddenLeadActor + "\n";
            s += "DVD Release Date: " + hiddenReleaseDate + "\n";
            s += "DVD Run Time: " + hiddenRunTime + "\n";
            return s;
        }  // end ToString
    }
}
