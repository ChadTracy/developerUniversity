﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DeveloperUniversity.Models;
using DeveloperUniversity.Models.ViewModels;

namespace DeveloperUniversity.Controllers
{
    public class UserManagementController : Controller
    {
        //Refer to this github repo for another example of how to do this.
        //https://github.com/TypecastException/AspNetRoleBasedSecurityExample/blob/master/AspNetRoleBasedSecurity/Views/Account/Edit.cshtml

        readonly ApplicationDbContext _db = new ApplicationDbContext();


        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var users = _db.Users;
            var model = new List<SelectUserRolesViewModel>();
            foreach (var user in users)
            {
                var u = new SelectUserRolesViewModel(user);
                u.Id = user.Id;
                model.Add(u);
            }
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.FirstOrDefault(u => u.Id == id);
            var model = new EditUserViewModel(user);
            //ViewBag.MessageId = Message;
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Db = new ApplicationDbContext();
                var user = Db.Users.First(u => u.UserName == model.UserName);
                //user.FirstName = model.FirstName;
                //user.LastName = model.LastName;
                user.Email = model.Email;
                Db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //[Authorize(Roles = "Admin")]
        //public ActionResult Delete(string id = null)
        //{
        //    var Db = new ApplicationDbContext();
        //    var user = Db.Users.First(u => u.UserName == id);
        //    var model = new EditUserViewModel(user);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return System.Web.UI.WebControls.View(model);
        //}


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    var Db = new ApplicationDbContext();
        //    var user = Db.Users.First(u => u.UserName == id);
        //    Db.Users.Remove(user);
        //    Db.SaveChanges();
        //    return RedirectToAction("Index");
        //}


        //[Authorize(Roles = "Admin")]
        //public ActionResult UserRoles(string id)
        //{
        //    var Db = new ApplicationDbContext();
        //    var user = Db.Users.First(u => u.UserName == id);
        //    var model = new SelectUserRolesViewModel(user);
        //    return System.Web.UI.WebControls.View(model);
        //}


        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //[ValidateAntiForgeryToken]
        //public ActionResult UserRoles(SelectUserRolesViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var idManager = new IdentityManager();
        //        var Db = new ApplicationDbContext();
        //        var user = Db.Users.First(u => u.UserName == model.UserName);
        //        idManager.ClearUserRoles(user.Id);
        //        foreach (var role in model.Roles)
        //        {
        //            if (role.Selected)
        //            {
        //                idManager.AddUserToRole(user.Id, role.RoleName);
        //            }
        //        }
        //        return RedirectToAction("index");
        //    }
        //    return View();
        //}
    }
}