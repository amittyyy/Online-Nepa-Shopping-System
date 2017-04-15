using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class UserController : Controller
    {
        private OnlineShoppingEntity db = new OnlineShoppingEntity();

        //
        // GET: /User/

        [AllowAnonymous]
        public ActionResult Index(int id)
        {
            var user =
                db.UserLogins.Where(x => x.UserId == id)
                    .Select(
                        x =>
                            new UserDisplayModel
                            {
                                UserId = x.UserId,
                                ContactNumber = x.ContactNumber,
                                Date = x.JoinedDate,
                                Email = x.Email,
                                Address = x.Address,
                                FullName = x.FullName,
                                TotalProducts = x.Products.Count,
                                TotalLikes =
                                    db.ProductThumbs.Count(
                                        y =>
                                            x.Products.Select(z => z.ProductId).Contains(y.ProductId) &&
                                            y.UserId != x.UserId && y.Up),
                                TotalDislikes =
                        db.ProductThumbs.Count(
                            y =>
                                x.Products.Select(z => z.ProductId).Contains(y.ProductId) &&
                                y.UserId != x.UserId && !y.Up),
                                TotalJobPostings = db.JobPostings.Count(y => y.UserId == id)
                            }).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Edit()
        {
            if (Session["UserId"] != null)
            {
                var userlogin = db.UserLogins.Find(Convert.ToInt32(Session["UserId"].ToString()));
                if (userlogin == null)
                    return HttpNotFound();

                var e = new UserEditModel
                {
                    ContactNumber = userlogin.ContactNumber,
                    Address = userlogin.Address,
                    FullName = userlogin.FullName
                };
                return View(e);
            }
            return RedirectToAction("Login", "Account");
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditModel userInfo)
        {
            var uId = Convert.ToInt32(Session["UserId"]);
            if (ModelState.IsValid && uId > 0)
            {
                var userlogin = db.UserLogins.Find(uId);
                if (userlogin == null)
                    return HttpNotFound();
                userlogin.Address = userInfo.Address;
                userlogin.ContactNumber = userInfo.ContactNumber;
                userlogin.FullName = userInfo.FullName;
                db.SaveChanges();

                return RedirectToAction("Index", new { id = uId });
            }
            return View(userInfo);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}