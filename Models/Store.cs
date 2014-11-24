using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreOwnerApp.Models
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }
        public string storenum { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Prodname { get; set; }
        public string[] prodarray { get; set; }
        public string Votes { get; set; }

        public virtual User User { get; set; }
        public virtual IEnumerable<Newtonsoft.Json.Linq.JToken> Products { get; set; }

       
    }
}