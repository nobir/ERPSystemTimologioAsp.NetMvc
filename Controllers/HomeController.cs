using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPSystemTimologio.Auth;
using ERPSystemTimologio.EF;
using ERPSystemTimologio.Models;

namespace ERPSystemTimologio.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpGet, NotLoggedIn]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginModel user)
        {
            if(!ModelState.IsValid)
            {
                return View(user);
            }

            var db = new TimologioEntities();
            var db_user = db.Users.Where(u => u.Email == user.Email).SingleOrDefault();

            if (db_user == null)
            {
                TempData["error_message"] = "Invalid email or password";
                return RedirectToAction("Login", "Home");
            }

            bool verified_check = db_user.Verified == 1;

            if (!verified_check)
            {
                TempData["error_message"] = "Your account is not verified yet";
                return RedirectToAction("Login", "Home");
            }

            bool password_check = db_user.Password == user.Password;

            if (!password_check)
            {
                TempData["error_message"] = "Invalid email or password";
                return RedirectToAction("Login", "Home");
            }

            db_user.Status = 1;

            db.WorkingHours.Add(new WorkingHour
                {
                    UserId = db_user.Id,
                    Date = DateTime.Now.Date,
                    EntryTime = DateTime.Now,
                }
            );

            Session["user"] = db_user;
            Session["loggedin"] = true;

            db.SaveChanges();

            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult Logout()
        {
            var sessionUser = (User)Session["user"];
            if(sessionUser != null)
            {
                var db = new TimologioEntities();
                var sessionUserId = sessionUser.Id;

                var db_working_hour = db.WorkingHours.Where(w => w.UserId == sessionUserId && w.ExitTime == null).SingleOrDefault();

                if(db_working_hour != null)
                {
                    db_working_hour.ExitTime = DateTime.Now;
                    db.SaveChanges();
                }

                var user = db.Users.Where(t => t.Id == sessionUserId).SingleOrDefault();

                user.Status = 0;

                db.SaveChanges();
            }

            Session.RemoveAll();

            return RedirectToAction("Login", "Home");
        }
    }
}