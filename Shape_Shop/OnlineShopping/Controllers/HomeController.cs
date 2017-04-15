// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Cosmos">
//  Cosmos On-line Shopping
// </copyright>
// <summary>
//   The main page controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OnlineShopping.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using OnlineShopping.Helpers;
    using OnlineShopping.Models;

    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly OnlineShoppingEntity _db = new OnlineShoppingEntity();
        public ActionResult Index(string search, string category, int page = 0)
        {
            var uId = 0;
            int.TryParse(Convert.ToString(Session["UserId"]), out uId);
            var cat = 0;
            int.TryParse(category, out cat);
            ViewBag.ItemsInRow = 4;

            var result =
                _db.Products.OrderByDescending(x => x.Added)
                //.Skip(0)
                //.Take(12)
                    .Where(x => (string.IsNullOrEmpty(search) || x.Name.Contains(search)))
                    .Where(x => ((cat <= 0) || x.ProductCategory.MainCategory.MainCategoryId == cat))
                    .Select(
                        x =>
                            new ProductListingModel()
                            {
                                ProductId = x.ProductId,
                                Description = x.Description,
                                FullName = x.UserLogin.FullName,
                                Image = x.Image,
                                MainProductCategory = x.ProductCategory.MainCategory.Name,
                                ProductName = x.Name,
                                UserId = x.UserId,
                                Added = x.Added,
                                Price = x.Price,
                                ContactNumber = x.UserLogin.ContactNumber,
                                Email = x.UserLogin.Email,
                                IsUsed = x.IsUsed,
                                Thumbs =
                                    new ProductInfoModel
                                    {
                                        ProductId = x.ProductId,
                                        Up = x.ProductThumbs.Count(y => y.Up),
                                        Down = x.ProductThumbs.Count(y => !y.Up),
                                        YourResponse = null,
                                        Comments = x.UserComments.Count()
                                    }
                            });
            ViewBag.Search = search;
            ViewBag.Category = category;
            var presult = new PaginatedList<ProductListingModel>(result, page, ViewBag.ItemsInRow * 2, 5, search, cat);
            return View(presult);
        }

        public ActionResult GetListOfCategories()
        {
            var lst = new List<SelectListItem>(_db.MainCategories.OrderBy(x => x.Name).ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.MainCategoryId.ToString() }));
            return PartialView(lst);
        }

        public JsonResult GetCategories()
        {
            var lst = new List<SelectListItem>(_db.MainCategories.OrderBy(x => x.Name).ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.MainCategoryId.ToString() }));
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NotifCount()
        {
            var uId = 0;
            int.TryParse(Convert.ToString(Session["UserId"]), out uId);
            return Content(uId > 0 ? _db.Notifications.Count(x => x.UserId == uId && x.ReadDate == null).ToString() : 0.ToString());
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ScrollList(int displayCount = 20, int lastProduct = 0)
        {
            var newItemCount = lastProduct <= 0 ? 20 : _db.Products.Count(x => x.ProductId > lastProduct);

            if (newItemCount <= 0)
                return Json(string.Empty, JsonRequestBehavior.AllowGet);

            var result =
                _db.Products.OrderByDescending(x => x.Added)
                    .Take(displayCount)
                //.Where(x => x.ProductId > lastProduct)
                    .Select(
                        x =>
                            new ProductScrollModel
                            {
                                ProductId = x.ProductId,
                                Title = x.Name,
                                Category = x.ProductCategory.Name,
                                Image = x.Image
                            });


            if (Request.IsAjaxRequest())
            {
                //if (!result.Any())
                //    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                ViewBag.Start = (displayCount / 2) - newItemCount;
                return PartialView("_ScrollPane", result.OrderBy(x => x.ProductId));
            }


            return PartialView("_ScrollPane", result.OrderBy(x => x.ProductId));
        }

        [HttpGet]
        public ActionResult NotifList(int page = 0)
        {
            var uId = 0;
            int.TryParse(Convert.ToString(Session["UserId"]), out uId);
            if (uId > 0)
            {
                var result =
                    _db.Notifications
                        .Where(x => x.UserId == uId)
                        .OrderByDescending(x => x.SentDate)
                        .Select(
                            x =>
                                new NotificationModel
                                {
                                    NotificationId = x.NotificationId,
                                    UserId = x.OriginatorId,
                                    FullName = x.UserLogin1.FullName,
                                    NotificationTypeId = x.NotificationTypeId,
                                    ProductId = x.ProductId,
                                    ProductName = x.Product.Name,
                                    SentDate = x.SentDate,
                                    IsNew = x.ReadDate == null
                                });

                var presult = new PaginatedList<NotificationModel>(result, page, 10, 5);
                foreach (var notificationModel in presult)
                    _db.Notifications.Find(notificationModel.NotificationId).ReadDate = DateTime.Now;
                _db.SaveChanges();

                return View(presult);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Delete(int id, string uri)
        {
            if (Session["UserName"] != null
                && Session["UserName"].ToString().StartsWith("admin", StringComparison.OrdinalIgnoreCase))
            {
                var prd = _db.Products.Find(id);
                if (prd != null)
                {
                    _db.Products.Remove(prd);
                    _db.SaveChanges();
                    var fullName = System.IO.Path.Combine(Server.MapPath("~/Images"), "Products", prd.Image);
                    if (System.IO.File.Exists(fullName))
                        System.IO.File.Delete(fullName);
                }
            }
            return Redirect(uri);
        }
    }
}
