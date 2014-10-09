using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreOwnerApp.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string url { get; set; }
        public string image { get; set; }
        public object id { get; set; }
        public string title { get; set; }

        public virtual User User { get; set; }
        public virtual Store Store { get; set; }                   
    }

    //public class RootObject
    //{
    //    public string status { get; set; }
    //    public double time { get; set; }
    //    public List<Product> products { get; set; }
    //    public bool realtime_availability { get; set; }
    //    public int products_found { get; set; }
    //}
}