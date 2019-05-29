using Lab_23_Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab_23_Entity.Controllers
{
    public class HomeController : Controller
    {
        ShopDBEntities db = new ShopDBEntities();
        public ActionResult Index()
        {
            User u = (User)Session["LoggedInUser"];
            ViewBag.User = u;

            // LINQ code that didn't end up working
            //var user = Session["LoggedInUser"];
            //List<User> p = (List<User>)user;

            //User u = p[0];

            //ViewBag.User = u;

            return View();
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

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult MakeNewUser(User u)
        {
            db.Users.Add(u);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserName, string PassWord)
        {
            List<User> Users = db.Users.ToList();

            foreach (User u in Users)
            {
                if (u.UserName == UserName && u.PassWord == PassWord)
                {
                    Session["LoggedInUser"] = u;
                    return RedirectToAction("Index");
                }
            }
            Session["Error"] = "Username and Password don't match. Please try again.";
            return RedirectToAction("Login");
        }
    }
}