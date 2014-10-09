using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace StoreOwnerApp.Controllers
{
    [EnableCors(origins: "http://localhost:2971",
    headers: "accept,content-type,origin,x-my-header", methods: "*")]
    public class StoresApiController : ApiController
    {        
        public JArray GetStores()
        {
            var url = "http://api.goodzer.com/products/v0.1/search_locations/?query=v-neck+sweater&lat=40.714353&lng=-74.005973 &radius=5.0&priceRange=30:120&apiKey=f7d93213c281896d44c093a36a4f544a";

            using (var client = new WebClient())
            {
                var data = client.DownloadString(url);
                var array = (JObject)JsonConvert.DeserializeObject(data);
                var arraytest = array.Properties().ToArray();
                JObject o = JObject.Parse(data);
                IList<JProperty> loc = o.Properties().ToList();

                JArray list = (JArray)loc[1].Value;                        

                return list;
            }
        }
     
    
    }
}
