using Lab_23_Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
            if (Session["LoggedInUser"] != null)
            {
                return RedirectToAction("Shop");
            }
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

            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            if (Session["LoggedInUser"] != null)
            {
                Session["Error"] = "";
            }

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
                    ViewBag.Shop = db.Items;

                    Session["Error"] = "";
                    return RedirectToAction("Shop");
                }
            }
            Session["Error"] = "Username and Password don't match. Please try again.";
            return RedirectToAction("Login");
        }

        public ActionResult Shop()
        {
            if (Session["LoggedInUser"] == null)
            {
                Session["Error"] = "";
            }

            User u = (User)Session["LoggedInUser"];
            ViewBag.User = u;
            ViewBag.Shop = db.Items;
            return View();
        }

        public ActionResult Buy(int id)
        {
            User u = (User)Session["LoggedInUser"];
            Item i = db.Items.Find(id);

            if (i.Price <=  u.Balance && i.Quantity > 0)
            {
                i.Quantity--;
                u.Balance -= i.Price;

                db.Users.AddOrUpdate(u);
                db.Items.AddOrUpdate(i);

                UserItem ui = new UserItem(); // create new "UserItem" object
                ui.ItemID = i.id; // add to ItemID column the "id" from Item id key
                ui.UserID = u.id; // add to UserID column the "id" from User id key

                db.UserItems.AddOrUpdate(ui); // 

                db.SaveChanges();

                User uu = (User)Session["LoggedInUser"];

                ViewBag.Shop = uu;

            }
            else if(i.Quantity < 1)
            {
                Session["Error"] = $"Sorry, there aren't any more {i.Name}s left.";
            }
            else
            {
                Session["Error"] = $"You only have ${u.Balance} left in your account, but that item costs ${i.Price}.";

                return RedirectToAction("Error");
            }

            return RedirectToAction("Shop");
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["LoggedInUser"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult List(int id)
        {
            List<Item> i = db.Items.ToList();
            List<UserItem> ui = db.UserItems.ToList();
            ViewBag.Items = i;
            ViewBag.UserItems = ui;

            return View();
        }


    }
}