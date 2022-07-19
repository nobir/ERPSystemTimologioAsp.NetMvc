using ERPSystemTimologio.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPSystemTimologio.EF;
using ERPSystemTimologio.Models;

namespace ERPSystemTimologio.Controllers
{
    //[LoggedIn, IsAdmin]
    public class AdminController : Controller
    {
        private readonly TimologioEntities db = new TimologioEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewVerifiedUsers(int? id)
        {
            int perpage = 5;
            int currentpage = id ?? 1;
            var users = this.db.Users.Where(u => u.Verified == 1).ToList();

            users = users.Skip((currentpage - 1) * perpage).Take(perpage).ToList();
            int maxpage = (int)Math.Ceiling(Convert.ToDecimal(users.Count()) / Convert.ToDecimal(perpage));

            ViewBag.Users = users;

            if((currentpage - 1) <= maxpage)
            {
                ViewBag.NextPageUrl = Url.Action("ViewVerifiedUsers", new { id = currentpage + 1 });
            }

            if(1 < currentpage)
            {
                ViewBag.PreviousPageUrl = Url.Action("ViewVerifiedUsers", new { id = currentpage - 1 });
            }

            return View();
        }

        [HttpPost]
        public ActionResult ViewVerifiedUsers(string SearchValue, string SearchBy, int? id)
        {
            List<User> users = null;

            if(SearchValue.Trim().Length == 0)
            {
                TempData["error_message"] = "Enter search value";
                ViewBag.Users = this.db.Users.Where(u => u.Verified == 1).ToList();
                return RedirectToAction("ViewVerifiedUsers", "Admin");
            }
            else if (SearchBy == "Id")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Id.ToString() == SearchValue.ToString()) select user).ToList();
            }
            else if (SearchBy == "LocalAddress")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Address.LocalAddress.ToString() == SearchValue.ToString()) select user).ToList();
            }
            else if(SearchBy == "PoliceStation")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Address.PoliceStation.ToString() == SearchValue.ToString()) select user).ToList();
            }
            else if (SearchBy == "City")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Address.City.ToString() == SearchValue.ToString()) select user).ToList();
            }
            else if (SearchBy == "ZipCode")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Address.ZipCode.ToString() == SearchValue.ToString()) select user).ToList();
            }
            else if (SearchBy == "Country")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Address.Country.ToString() == SearchValue.ToString()) select user).ToList();
            }
            else if(SearchBy == "Region")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Region.Name.ToString() == SearchValue.ToString()) select user).ToList();
            }
            else if (SearchBy == "Branch")
            {
                users = this.db.Users.Where(u => u.Verified == 1).ToList();
                users = (from user in users where (user.Branch.Name.ToString() == SearchValue.ToString()) select user).ToList();
            }
            else if (SearchBy == "Permission")
            {
                users = (from user in this.db.Users.Where(u => u.Verified == 1).ToList() where user.Permissions.Any(p => p.Name.Contains(SearchValue.ToString())) select user).ToList();
            }
            else
            {
                users = this.db.Users.ToList();
            }

            if(users == null || users.Count() == 0)
            {
                TempData["error_message"] = "No users found";
                return RedirectToAction("ViewVerifiedUsers", "Admin");
            }

            //int perpage = 5;
            //users = users.Skip((int)((id - 1) * perpage)).Take(perpage).ToList();

            ViewBag.Users = users;
            //ViewBag.NextPageUrl = Url.Action("ViewVerifiedUsers", new { id });

            return View();
        }

        [HttpGet]
        public ActionResult CreateUser()
        {
            ViewBag.Branches = this.db.Branches.ToList();
            ViewBag.Regions = this.db.Regions.ToList();
            ViewBag.Permissions = this.db.Permissions.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(UserCreateAdminModel _user)
        {
            ViewBag.Branches = this.db.Branches.ToList();
            ViewBag.Regions = this.db.Regions.ToList();
            ViewBag.Permissions = this.db.Permissions.ToList();

            if (_user.PermissionIds.Count() > 0)
            {
                _user.PermissionIds.RemoveAll(n => n == 0);
                _user.PermissionIds = _user.PermissionIds.Distinct().ToList();
            }

            if (!ModelState.IsValid)
            {
                return View(_user);
            }

            User user = new User
            {
                Verified = _user.Verified,
                Name = _user.Name,
                Username = _user.Username,
                Email = _user.Email,
                Salary = _user.Salary,
                Password = _user.Password,
                Type = _user.Type,
                HireDate = _user.HireDate
            };

            if (_user.RegionId != null && _user.BranchId == null)
            {
                if(db.Regions.Where(r => r.Id == _user.RegionId).Count() == 0)
                {
                    TempData["error_message"] = "Region not found";
                    return RedirectToAction("CreateUser", "Admin");
                }

                user.RegionId = _user.RegionId;
            }
            else if(_user.BranchId != null)
            {
                if (db.Branches.Where(b => b.Id == _user.BranchId).Count() == 0)
                {
                    TempData["error_message"] = "Branch not found";
                    return RedirectToAction("CreateUser", "Admin");
                }

                user.BranchId = _user.BranchId;
            }

            Address address = new Address
            {
                LocalAddress = _user.LocalAddress,
                PoliceStation = _user.PoliceStation,
                City = _user.City,
                Country = _user.Country,
                ZipCode = _user.ZipCode,
            };

            db.Addresses.Add(address);

            db.SaveChanges();

            user.AddressId = address.Id;

            db.Users.Add(user);

            db.SaveChanges();

            for (var i = 0; i < _user.PermissionIds.Count(); ++i)
            {
                var id = _user.PermissionIds[i];
                var permission = db.Permissions.Where(p => p.Id == id).SingleOrDefault();

                db.Permissions.Attach(permission);
                user.Permissions.Add(permission);
                db.SaveChanges();
            }

            db.SaveChanges();

            TempData["success_message"] = "Successfully created user";

            return RedirectToAction("CreateUser", "Admin");
        }
    }
}