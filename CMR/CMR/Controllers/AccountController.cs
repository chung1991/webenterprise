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
                var user = db.Accounts.SingleOrDefault(u => u.userName == m.userName && u.userPassword == m.passWord);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(m.userName, m.rememberMe);
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
                    Profile p = new Profile(r.fullName, r.address,r.telephone,r.dateOfBirth);
                    db.Profiles.Add(p);
                    db.SaveChanges();
                    int lastID = db.Profiles.Max(pro => pro.profileId);
                    Account a = new Account(r.accountId, r.userName, r.passWord, lastID, r.roleId);
                    db.Accounts.Add(a);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.roles = new SelectList(db.Roles, "roleId", "roleName");
            return View(r);
        }
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<UserDetailModel> model=new List<UserDetailModel>();
            var userDetail =( from acc in db.Accounts
                             join r in db.Roles on acc.roleId equals r.roleId
                             join pro in db.Profiles on acc.profileId equals pro.profileId
                             select new {acc.accountId, acc.userName, r.roleName, pro.name, pro.address, pro.telephone, pro.dateOfBirth }).ToList();
            foreach (var item in userDetail)
            {
                model.Add(new UserDetailModel(item.accountId,item.userName,item.name,item.roleName,item.address,item.telephone,item.dateOfBirth));
            }

            return View(model);
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
            var userDetail = (from acc in db.Accounts
                             join r in db.Roles on acc.roleId equals r.roleId
                             join pro in db.Profiles on acc.profileId equals pro.profileId
                             where acc.accountId == accountId
                             select new { acc.accountId, acc.userName,acc.userPassword,acc.roleId, pro.name, pro.address, pro.telephone, pro.dateOfBirth }).First();
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            RegisterModel rm = new RegisterModel(userDetail.accountId, userDetail.userName,userDetail.userPassword,userDetail.userPassword, userDetail.roleId,userDetail.name,userDetail.address,userDetail.telephone,userDetail.dateOfBirth);
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
                    Account account = new Account(rm.accountId, rm.userName,rm.passWord,ac.profileId,rm.roleId);
                    db.Entry(ac).CurrentValues.SetValues(account);
                    db.SaveChanges();

                    Profile profile = new Profile(ac.profileId,rm.fullName,rm.address,rm.telephone,rm.dateOfBirth);
                    db.Entry(profile).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.roles = new SelectList(db.Roles, "roleId", "roleName",rm.roleId);
            return View(rm);
        }

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
    }
}