using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace StoreOwnerApp.Models
{
    public class Location
    {
        public string lat { get; set; }
        public string lng { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string state { get; set; }
        public string store_id { get; set; }
        public int Rating { get; set; }
        public int TotalRaters { get; set; }
       
    }

    public class RootObject
    {
        public string status { get; set; }
        public IEnumerable<Location> locations { get; set; }
        public double time { get; set; }
    }
}