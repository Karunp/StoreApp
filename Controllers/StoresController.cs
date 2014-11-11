using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreOwnerApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Net.Http.Headers;
using StoreOwnerApp.ViewModels;
using PagedList;
using GoogleMaps.LocationServices;


namespace StoreOwnerApp.Controllers
{
    
    public class StoresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Stores
        public ActionResult Index()
        {                        
            return View(db.Stores.ToList());        
        }

        public ActionResult Browse(string category)
        {
            // Retrieve Genre and its Associated Albums from database
            var catModel = db.Categories.Include("Products")
                .Single(g => g.Name == category);

            return View(catModel);
        }

        public ActionResult ProductDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product== null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
       
        public ActionResult AddToCart(int id)
        {

            // Retrieve the album from the database
            var addedAlbum = db.Products
                .Single(album => album.ProductId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedAlbum);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        public ActionResult FindStores()
        {
            //instantiate new client and model
            var client = new HttpClient();
            var model = new List<Location>();

            //Multiple locations
            //send get request to specified Uri as an asynchorous operation
            var task = client.GetAsync("http://api.goodzer.com/products/v0.1/location_details/?locationId=fKAKgREd&apiKey=f7d93213c281896d44c093a36a4f544a")
                .ContinueWith((taskwithmsg) =>
                {
                    var response = taskwithmsg.Result;
                    //process content
                    var jsonTask = response.Content.ReadAsAsync<JObject>();
                    jsonTask.Wait();
                    var jsonObject = jsonTask.Result;

                    //Skip first index to access JProperties of "location" index - bind to model
                    model = (jsonObject.Children().Skip(1).Cast<JProperty>()
                                        .Select(j => new Location
                                        {
                                            city = j.Value["city"].ToString(),
                                            address = j.Value["address"].ToString(),
                                            name = j.Value["name"].ToString(),
                                            lat = j.Value["lat"].ToString(),
                                            lng = j.Value["lng"].ToString()
                                        })).ToList();

                });
            task.Wait();
            return View(model);
        }

        public ActionResult storeproducts()
        {
            //instantiate new client and model
            var client = new HttpClient();
            var model = new List<Store>();
            var prodmodel = new List<Product>();
            var address = db.Users.Find(User.Identity.GetUserId()).Address;
            var locationService = new GoogleLocationService();
            var point = locationService.GetLatLongFromAddress(address);
            var latitude = point.Latitude;
            var longitude = point.Longitude;

            //send get request to specified Uri as an asynchorous operation
            var task = client.GetAsync("http://api.goodzer.com/products/v0.1/search_stores/?query=halo&lat=" +latitude+ "&lng=" +longitude +"&radius=20&priceRange=30:120&apiKey=f7d93213c281896d44c093a36a4f544a")
                .ContinueWith((taskwithmsg) =>
                {
                    var response = taskwithmsg.Result;
                    //process content
                    var jsonTask = response.Content.ReadAsAsync<JObject>();
                    jsonTask.Wait();
                    var jsonObject = jsonTask.Result;

                    model.AddRange((from JObject jo in (JArray)jsonObject["stores"]
                                    select new Store
                                    {
                                        Name = jo["name"].ToString(),
                                        //Products = jo["stores"][0]["products"].Values().ToList(),                                        
                                        //url = jo["url"].ToString(),
                                        //image = jo["image"].ToString(),
                                    }
                                       ));
                    int i = 0;
                    while (i < 6)
                    {
                       
                    prodmodel.AddRange((from JObject jo in (JArray)jsonObject["stores"][i]["products"]
                                        select new Product
                                        {
                                            title = jo["title"].ToString(),
                                            url = jo["url"].ToString(),
                                            image = jo["image"].ToString(),
                                        }
                                      ));
                    i++;
                    }



                });              
            task.Wait();
            ViewBag.store = model;
            ViewBag.prod = prodmodel;
            return View(model);
        }

        public  ActionResult location_details()
        {
            //instantiate new client and model
            var client = new HttpClient();
            var model = new List<Location>();

            //Multiple locations
            //send get request to specified Uri as an asynchorous operation
            var task = client.GetAsync("http://api.goodzer.com/products/v0.1/location_details/?locationId=fKAKgREd&apiKey=f7d93213c281896d44c093a36a4f544a")
                .ContinueWith((taskwithmsg) =>
                {
                    var response = taskwithmsg.Result;
                    //process content
                    var jsonTask = response.Content.ReadAsAsync<JObject>();
                    jsonTask.Wait();
                    var jsonObject = jsonTask.Result;

                    //Skip first index to access JProperties of "location" index - bind to model
                    model = (jsonObject.Children().Skip(1).Cast<JProperty>()
                                        .Select(j => new Location { 
                                            city = j.Value["city"].ToString(),
                                            address = j.Value["address"].ToString(),
                                            name = j.Value["name"].ToString(),
                                            lat = j.Value["lat"].ToString(),
                                            lng = j.Value["lng"].ToString()
                                        })).ToList();                   

                });
            task.Wait();
            return View(model);
        }

        //public ActionResult location_details_api()
        //{
        //    var url = "http://api.goodzer.com/products/v0.1/location_details/?locationId=fKAKgREd&apiKey=f7d93213c281896d44c093a36a4f544a";

        //    //instantiate new client and model
        //    using (var client = new WebClient())
        //    {
        //        var data = client.DownloadString(url);
        //        var array =  (JObject)JsonConvert.DeserializeObject(data);
        //        var arraytest = array.Properties().ToArray();
        //        JObject o = JObject.Parse(data);               
                
        //        return Json(data[1], JsonRequestBehavior.AllowGet);
        //    }           
        //}



        //[JsonTest]
        public ActionResult search_stores(string productname, decimal? fromprice, decimal? toprice)
        {
            //instantiate new client and model
            var client = new HttpClient();            
            var model = new List<Location>();

            var address = db.Users.Find(User.Identity.GetUserId()).Address;
            var locationService = new GoogleLocationService();
            var point1 = locationService.GetLatLongFromAddress(address);
            var latitude = point1.Latitude;
            var longitude = point1.Longitude;

          
            if (!String.IsNullOrEmpty(productname))
            {
                //send get request to specified Uri as an asynchorous operation
                var task = client.GetAsync("http://api.goodzer.com/products/v0.1/search_locations/?query=" + productname + "&lat=" + latitude + "&lng=" + longitude + "&radius=20&priceRange= " + fromprice + ":" + toprice + "&apiKey=f7d93213c281896d44c093a36a4f544a")
                    .ContinueWith((taskwithmsg) =>
                    {
                        var response = taskwithmsg.Result;
                        //process content
                        var jsonTask = response.Content.ReadAsAsync<JObject>();
                        jsonTask.Wait();
                        var jsonObject = jsonTask.Result;
                        model.AddRange((from JObject jo in (JArray)jsonObject["locations"]
                                        select new Location
                                        {
                                            name = jo["name"].ToString(),
                                            store_id = jo["store_id"].ToString(),
                                            lat = jo["lat"].ToString(),
                                            lng = jo["lng"].ToString()
                                        }
                                           ));

                    });
                task.Wait();
            }
            ViewBag.model = model;
            ViewBag.productname = productname;
            ViewBag.fromprice = fromprice;
            ViewBag.toprice = toprice;

            ViewBag.mylat = latitude;
            ViewBag.mylng = longitude;

            return View(model);
        }

        public ActionResult search_in_store(string storeID, string productname,  decimal? fromprice, decimal? toprice, int? page, string lat, string lng)
        {                      
            //instantiate new client and model
            var client = new HttpClient();
            var model = new List<Product>();
            var address = db.Users.Find(User.Identity.GetUserId()).Address;
            var locationService = new GoogleLocationService();
            var point = locationService.GetLatLongFromAddress(address);
            ViewBag.mylat = point.Latitude;
            ViewBag.mylng = point.Longitude;

            if (!String.IsNullOrEmpty(productname))
            {
                ViewBag.userproducts = db.Products.Where(a => a.Name.Contains(productname) && a.Price >= fromprice && a.Price <= toprice);
                ViewBag.prodcount = db.Products.Where(b => b.Name.Contains(productname)).Count();
                var task = client.GetAsync("http://api.goodzer.com/products/v0.1/search_in_store/?storeId=" + storeID + "&query=" + productname + "&priceRange=" + fromprice + ":" + toprice + "&apiKey=f7d93213c281896d44c093a36a4f544a")
               .ContinueWith((taskwithmsg) =>
               {
                   var response = taskwithmsg.Result;
                   //process content
                   var jsonTask = response.Content.ReadAsAsync<JObject>();
                   jsonTask.Wait();
                   var jsonObject = jsonTask.Result;
                   model.AddRange((from JObject jo in (JArray)jsonObject["products"]
                                   select new Product
                                   {
                                       id = jo["id"].ToString(),
                                       title = jo["title"].ToString(),
                                       url = jo["url"].ToString(),
                                       image = jo["image"].ToString(),
                                       Price = Convert.ToDecimal(jo["price"])
                                   }
                                      ));

               });
                if (Request.HttpMethod != "GET")
                {
                    page = 1;
                }
                int pageSize = 2;
                int pageNumber = (page ?? 1);
                ViewBag.pageSize = pageSize;
                ViewBag.pageNumber = pageNumber;
                task.Wait();
                ViewBag.myModel = model.OrderBy(a => a.Name).ToPagedList(pageNumber, pageSize);
                ViewBag.pagecount = model.OrderBy(a => a.Name).ToPagedList(pageNumber, pageSize).PageCount;
                ViewBag.prevpage = model.OrderBy(a => a.Name).ToPagedList(pageNumber, pageSize).HasPreviousPage;
                ViewBag.nextpage = model.OrderBy(a => a.Name).ToPagedList(pageNumber, pageSize).HasNextPage;
                ViewBag.modelcount = model.Count();
                ViewBag.storelat = lat;
                ViewBag.storelng = lng;
                ViewBag.productname = productname;
                ViewBag.storeID = storeID;
                ViewBag.fromprice = fromprice;
                ViewBag.toprice = toprice;

            }
            //send get request to specified Uri as an asynchorous operation

            return View(model);
        }

        public PartialViewResult ajax_store_products(string storeID, string productname, decimal? fromprice, decimal? toprice, int? page, string lat, string lng)
        {
            //instantiate new client and model
            var client = new HttpClient();
            var model = new List<Product>();
            var address = db.Users.Find(User.Identity.GetUserId()).Address;
            var locationService = new GoogleLocationService();
            var point = locationService.GetLatLongFromAddress(address);
            ViewBag.mylat = point.Latitude;
            ViewBag.mylng = point.Longitude;

            if (!String.IsNullOrEmpty(productname))
            {
                ViewBag.userproducts = db.Products.Where(a => a.Name.Contains(productname) && a.Price >= fromprice && a.Price <= toprice);
                ViewBag.prodcount = db.Products.Where(b => b.Name.Contains(productname)).Count();
                var task = client.GetAsync("http://api.goodzer.com/products/v0.1/search_in_store/?storeId=" + storeID + "&query=" + productname + "&priceRange=" + fromprice + ":" + toprice + "&apiKey=f7d93213c281896d44c093a36a4f544a")
               .ContinueWith((taskwithmsg) =>
               {
                   var response = taskwithmsg.Result;
                   //process content
                   var jsonTask = response.Content.ReadAsAsync<JObject>();
                   jsonTask.Wait();
                   var jsonObject = jsonTask.Result;
                   model.AddRange((from JObject jo in (JArray)jsonObject["products"]
                                   select new Product
                                   {
                                       id = jo["id"].ToString(),
                                       title = jo["title"].ToString(),
                                       url = jo["url"].ToString(),
                                       image = jo["image"].ToString(),
                                       Price = Convert.ToDecimal(jo["price"])
                                   }
                                      ));

               });
                if (Request.HttpMethod != "GET")
                {
                    page = 1;
                }
                int pageSize = 2;
                int pageNumber = (page ?? 1);
                ViewBag.pageSize = pageSize;
                ViewBag.pageNumber = pageNumber;
                task.Wait();
                ViewBag.myModel = model.OrderBy(a => a.Name).ToPagedList(pageNumber, pageSize);
                ViewBag.pagecount = model.OrderBy(a => a.Name).ToPagedList(pageNumber, pageSize).PageCount;
                ViewBag.prevpage = model.OrderBy(a => a.Name).ToPagedList(pageNumber, pageSize).HasPreviousPage;
                ViewBag.nextpage = model.OrderBy(a => a.Name).ToPagedList(pageNumber, pageSize).HasNextPage;
                ViewBag.modelcount = model.Count();
                ViewBag.storelat = lat;
                ViewBag.storelng = lng;
                ViewBag.productname = productname;
                ViewBag.storeID = storeID;
                ViewBag.fromprice = fromprice;
                ViewBag.toprice = toprice;

            }
            //send get request to specified Uri as an asynchorous operation

            return PartialView(model);
        }


        [HttpPost]
        public JsonResult search_stores_post()
        {
            //instantiate new client and model
            var client = new HttpClient();
            var model = new List<Location>();

            //send get request to specified Uri as an asynchorous operation
            var task = client.GetAsync("http://api.goodzer.com/products/v0.1/search_locations/?query=v-neck+sweater&lat=40.714353&lng=-74.005973 &radius=5.0&priceRange=30:120&apiKey=f7d93213c281896d44c093a36a4f544a")
                .ContinueWith((taskwithmsg) =>
                {
                    var response = taskwithmsg.Result;
                    //process content
                    var jsonTask = response.Content.ReadAsAsync<JObject>();
                    jsonTask.Wait();
                    var jsonObject = jsonTask.Result;
                    model.AddRange((from JObject jo in (JArray)jsonObject["locations"]
                                    select new Location
                                    {
                                        name = jo["name"].ToString(),
                                        store_id = jo["store_id"].ToString(),
                                        lat = jo["lat"].ToString(),
                                        lng = jo["lng"].ToString()
                                    }
                                       ));

                });
            task.Wait();
            return Json(new {jResult = model} );
        }
                 

        public JArray storeloc()
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

                //List<object> parameters = 
                //    (from p in o.Properties()
                //         where p.Value is JValue
                //         select ((JValue)p.Value).Value).ToList();                  
                //JArray jsonVal = JArray.Parse(data) as JArray;
                //dynamic locations = jsonVal;

                //var serializer = new JsonSerializer();
                //var rootobj = serializer.Deserialize<RootObject>(array[0].CreateReader());
                //rootobj.locations = serializer.Deserialize<List<Location>>(array[1].CreateReader());

                //foreach (dynamic root in array)
                //{
                //    var status = root.status;
                //    foreach(dynamic location in array)
                //    {
                //        var city = location.city;
                //        var add = location.address;
                //    }

                //}                           
                 
                return list;
            }
        }

        
        public ActionResult StoreHome()
        {
            return View();
        }

        // GET: Stores/Details/5
        public ActionResult StoreDetails(int? id, string productname, string myStore, decimal? fromprice,decimal? toprice, int? page)
        {
            ViewBag.myAddress = db.Users.Find(User.Identity.GetUserId()).Address;            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            ViewBag.address = store.Address;
            //var fname = store.User.FirstName;
            if (store == null)
            {
                return HttpNotFound();
            }
            //instantiate new client and model
            var client = new HttpClient();
            var model = new List<Product>();

            if (!String.IsNullOrEmpty(productname))
            {
                if (store.Name == "Wal Mart")
                {
                    myStore = "KfALmUgd";
                }
                else if (store.Name == "Best Buy")
                {
                    myStore = "fAfVfDCd";
                }
                ViewBag.userproducts = db.Products.Where(a=> a.Name.Contains(productname) && a.Price >= fromprice && a.Price <= toprice);
                ViewBag.prodcount = db.Products.Where(b => b.Name.Contains(productname)).Count();
                var task = client.GetAsync("http://api.goodzer.com/products/v0.1/search_in_store/?storeId=" + myStore +"&query=" + productname + "&priceRange=" +fromprice+":"+toprice+"&apiKey=f7d93213c281896d44c093a36a4f544a")
               .ContinueWith((taskwithmsg) =>
               {
                   var response = taskwithmsg.Result;
                   //process content
                   var jsonTask = response.Content.ReadAsAsync<JObject>();
                   jsonTask.Wait();
                   var jsonObject = jsonTask.Result;
                   model.AddRange((from JObject jo in (JArray)jsonObject["products"]
                                   select new Product
                                   {
                                       id = jo["id"].ToString(),
                                       title = jo["title"].ToString(),
                                       url = jo["url"].ToString(),
                                       image = jo["image"].ToString(),
                                       Price = Convert.ToDecimal(jo["price"])
                                   }
                                      ));

               });
                if (Request.HttpMethod != "GET")
                {
                    page = 1;
                }
                int pageSize = 2;
                int pageNumber = (page ?? 1);
                ViewBag.pageSize = pageSize;
                ViewBag.pageNumber = pageNumber;
                task.Wait();
                ViewBag.myModel = model.OrderBy(a=> a.Name).ToPagedList(pageNumber, pageSize);
                ViewBag.pagecount = model.OrderBy(a => a.Name).ToPagedList(pageNumber, pageSize).PageCount;
                ViewBag.prevpage = model.OrderBy(a => a.Name).ToPagedList(pageNumber, pageSize).HasPreviousPage;
                ViewBag.nextpage = model.OrderBy(a => a.Name).ToPagedList(pageNumber, pageSize).HasNextPage;
                ViewBag.modelcount = model.Count();
                ViewBag.productname = productname;
                ViewBag.fromprice = fromprice;
                ViewBag.toprice = toprice;

            }
            //send get request to specified Uri as an asynchorous operation
           
            return View(store);
        }

      
       
        public ActionResult AddProduct()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product product, int? id)
        {
            var store = db.Stores.Find(id).StoreId;
            //var store = db.Stores.Find(User.Identity.Name).StoreId;
            if (ModelState.IsValid)
            {
                product.StoreId = store;
                //product.Store.StoreId = store;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);

            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);

            return View(product);
        }

        // GET: Stores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StoreId,Name,Address")] Store store)
        {
            //var Users = new UserManager<User>(new UserStore<User>(new ApplicationDbContext())); 
            //var currentuser = UserManager.FindByIdAsync(User.Identity.GetUserId());
            var curuser = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                store.User = curuser;
                db.Stores.Add(store);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(store);
        }

        // GET: Stores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StoreId,Name,Address")] Store store)
        {
            if (ModelState.IsValid)
            {
                db.Entry(store).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(store);
        }

        // GET: Stores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Store store = db.Stores.Find(id);
            db.Stores.Remove(store);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
