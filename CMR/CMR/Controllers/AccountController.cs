using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMR.Models;
using System.Web.Security;
using CMR.Security;
using System.Net;
using System.Data.Entity;
using PagedList;


namespace CMR.Controllers
{
    public class AccountController : Controller
    {
        CRMContext db = new CRMContext();
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel m, String returnUrl)
        {
            if (ModelState.IsValid)
            {
                String pass = PasswordHandler.Encrypt(m.passWord);

                var user = db.Accounts.SingleOrDefault(u => u.userName == m.userName && u.userPassword == pass);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(m.userName, m.rememberMe);
                    System.Web.HttpContext.Current.Session["UserSession"] = (Account)user; 
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is invalid");
                }
            }
            return View();
        }
        public ActionResult logOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [CustomAuthorize(Roles="Admin")]
        public ActionResult Register()
        {
            ViewBag.roles = new SelectList(db.Roles, "roleId", "roleName");
            return View();
        }

        [CustomAuthorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Register(RegisterModel r)
        {
            if (ModelState.IsValid)
            {
                if (verifyUser(r.userName))
                    ModelState.AddModelError("userName", "User name have been existed");
                else
                {
                    Profile p = new Profile
                    {
                        name=r.fullName,
                        address=r.address,
                        telephone=r.telephone,
                        dateOfBirth=r.dateOfBirth
                    };
                    db.Profiles.Add(p);
                    db.SaveChanges();
                    int lastID = db.Profiles.Max(pro => pro.profileId);
                    Account a = new Account
                    {
                        userName = r.userName,
                        userPassword =PasswordHandler.Encrypt(r.passWord),
                        profileId = lastID,
                        roleId = 2
                    };

                    db.Accounts.Add(a);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.roles = new SelectList(db.Roles, "roleId", "roleName");
            return View(r);
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index(string sortOrder, String currentFilter,String searchString, int? page)
        {
            ViewBag.NameSortParm = sortOrder == "UserName" ? "name_desc" : "UserName";
            ViewBag.currentSort = sortOrder;
            ViewBag.currentFilter = searchString;
           
            var list = (from acc in db.Accounts
                        select acc);
            switch (sortOrder)
            {
                case "name_desc":
                    list = list.OrderByDescending(c => c.userName);
                    break;
                case "UserName":
                    list = list.OrderBy(c => c.userName);
                    break;
                default:
                    list = list.OrderBy(c => c.accountId);
                    break;
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(l => l.userName.Contains(searchString)||l.Profile.email.Contains(searchString)||l.Role.roleName.Contains(searchString));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber,pageSize));
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Detail(int accountId=0)
        {
            if (accountId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userDetail =( from acc in db.Accounts
                           join r in db.Roles on acc.roleId equals r.roleId
                           join pro in db.Profiles on acc.profileId equals pro.profileId
                           where acc.accountId == accountId
                           select acc).First();

            if (userDetail == null)
            {
                return HttpNotFound();
            }

            return View(userDetail);
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Edit(int? accountId=0)
        {
            if (accountId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userDetail = db.Accounts.SingleOrDefault(a => a.accountId == accountId);

            if (userDetail == null)
            {
                return HttpNotFound();
            }
            RegisterModel rm = new RegisterModel
            {
                accountId = userDetail.accountId,
                userName = userDetail.userName,
                passWord = userDetail.userPassword,
                confirmPassWord=userDetail.userPassword,
                roleId = (int)userDetail.roleId,
                fullName = userDetail.Profile.name,
                address = userDetail.Profile.address,
                email = userDetail.Profile.email,
                telephone = userDetail.Profile.telephone,
                dateOfBirth = (DateTime)userDetail.Profile.dateOfBirth

            };

            ViewBag.roles = new SelectList(db.Roles, "roleId", "roleName",userDetail.roleId);
            return View(rm);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Edit(RegisterModel rm)
        {
            if (ModelState.IsValid)
            {
                Account ac = db.Accounts.SingleOrDefault(a => a.accountId == rm.accountId);
                if(verifyUser(rm.userName,true,ac.userName)){
                    ModelState.AddModelError("userName", "This UserName have been existed");
                }
                else
                {
                    Account account = new Account
                    {
                        accountId=rm.accountId,
                        userName=rm.userName,
                        userPassword=rm.passWord,
                        profileId=ac.profileId,
                        roleId=2
                    };
                    db.Entry(ac).CurrentValues.SetValues(account);
                    db.SaveChanges();

                    Profile profile = new Profile
                    {
                        profileId =(int) ac.profileId,
                        name = rm.fullName,
                        address = rm.address,
                        email=rm.email,
                        telephone = rm.telephone,
                        dateOfBirth = rm.dateOfBirth
                    };

                    db.Entry(profile).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.roles = new SelectList(db.Roles, "roleId", "roleName",rm.roleId);
            return View(rm);
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Delete(int accountId=0)
        {
            if (accountId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(accountId);
            int profileId =(int) account.profileId;
            Profile profile = db.Profiles.Find(profileId);
            if (account == null)
            {
                return HttpNotFound();
            }
            db.Accounts.Remove(account);
            db.SaveChanges();
            db.Profiles.Remove(profile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [CustomAuthorize(Roles = "Admin")]
        public Boolean verifyUser(String userName,bool edit=false,String oldUser="")
        {
            if (edit)
            {
                if (userName == oldUser)
                {
                    return false;
                }      
            }
            var user = db.Accounts.SingleOrDefault(u => u.userName == userName); 
            return user == null ? false : true; 
        }

        [CustomAuthorize(Roles = "Admin")]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult insertDumyData()
        {
            ViewBag.annualCourses = new SelectList(db.AnnualCourses, "annualCourseId", "annualCourseId");
            return View();
        }

        [HttpPost]
        public ActionResult insertDumyData(int annualCourseId)
        {
            Random r = new Random();
            int range = r.Next(50, 100);

            for (int i = 0; i < range; i++)
            {
                int mark = r.Next(0, 100);
                Student s = new Student
                {
                    AnnualCourseId = annualCourseId,
                    Name = "Nguyen Van a " + i,
                    Mark = mark
                };
                db.Students.Add(s);
                db.SaveChanges();
            }
            ViewBag.annualCourses = new SelectList(db.AnnualCourses, "annualCourseId", "annualCourseId");
            return View();
        }
    }

    
}