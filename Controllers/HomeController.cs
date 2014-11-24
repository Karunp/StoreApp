using StoreOwnerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;


namespace StoreOwnerApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult MyProfile()
        {
            string name = User.Identity.Name;
            var user = db.Users.Where(a=> a.UserName == name).First();
            return View(user);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //public JsonResult SendRating(int thisVote, string sectionId, int autoId, string url)
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return Json("Not authenticated!");
        //    }

        //    if (autoId.Equals(0))
        //    {
        //        return Json("Sorry, record to vote doesn't exists");
        //    }

        //    switch (sectionId)
        //    {
        //        case "5": // school voting
        //            // check if he has already voted
        //            var isIt = db.VoteLogs.Where(v => v.SectionId == sectionId &&
        //                v.UserName.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase) && v.AutoId == autoId).FirstOrDefault();
        //            if (isIt != null)
        //            {
        //                // keep the school voting flag to stop voting by this member
        //                HttpCookie cookie = new HttpCookie(url, "true");
        //                Response.Cookies.Add(cookie);
        //                return Json("<br />You have already rated this post, thanks !");
        //            }

        //            var sch = db.Locations.Where(sc => sc.id == sectionId).FirstOrDefault();
        //            if (sch != null)
        //            {
        //                object obj = sch.votes;

        //                string updatedVotes = string.Empty;
        //                string[] votes = null;
        //                if (obj != null && obj.ToString().Length > 0)
        //                {
        //                    string currentVotes = obj.ToString(); // votes pattern will be 0,0,0,0,0
        //                    votes = currentVotes.Split(',');
        //                    // if proper vote data is there in the database
        //                    if (votes.Length.Equals(5))
        //                    {
        //                        // get the current number of vote count of the selected vote, always say -1 than the current vote in the array 
        //                        int currentNumberOfVote = int.Parse(votes[thisVote - 1]);
        //                        // increase 1 for this vote
        //                        currentNumberOfVote++;
        //                        // set the updated value into the selected votes
        //                        votes[thisVote - 1] = currentNumberOfVote.ToString();
        //                    }
        //                    else
        //                    {
        //                        votes = new string[] { "0", "0", "0", "0", "0" };
        //                        votes[thisVote - 1] = "1";
        //                    }
        //                }
        //                else
        //                {
        //                    votes = new string[] { "0", "0", "0", "0", "0" };
        //                    votes[thisVote - 1] = "1";
        //                }

        //                // concatenate all arrays now
        //                foreach (string ss in votes)
        //                {
        //                    updatedVotes += ss + ",";
        //                }
        //                updatedVotes = updatedVotes.Substring(0, updatedVotes.Length - 1);

        //                db.Entry(sch).State = EntityState.Modified;
        //                sch.votes = updatedVotes;
        //                db.SaveChanges();

        //                VoteLog vm = new VoteLog()
        //                {
        //                    Active = true,
        //                    SectionId = sectionId,
        //                    UserName = User.Identity.Name,
        //                    Vote = thisVote,
        //                    VoteForId = autoId
        //                };

        //                db.VoteLogs.Add(vm);

        //                db.SaveChanges();

        //                // keep the school voting flag to stop voting by this member
        //                HttpCookie cookie = new HttpCookie(url, "true");
        //                Response.Cookies.Add(cookie);
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //    return Json("<br />You rated " + thisVote + " star(s), thanks !");
        //}
    }
}