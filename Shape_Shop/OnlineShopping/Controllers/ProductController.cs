using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OnlineShopping.Helpers;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class ProductController : Controller
    {
        private readonly OnlineShoppingEntity _db = new OnlineShoppingEntity();

        [HttpGet]
        public ActionResult Create()
        {
            var prd = new ProductModel()
            {
                Categories = GetCategories(),
                ExpiryDate = DateTime.Today.AddDays(20)
            };
            prd.SubCategories =
                new List<SelectListItem>(
                    _db.ProductCategories.ToList()
                        .Where(x => x.MainCategoryId == Convert.ToInt32(prd.Categories.FirstOrDefault().Value))
                        .Select(x => new SelectListItem() { Text = x.Name, Value = x.ProductCategoryId.ToString() }));
            return View(prd);
        }

        [HttpPost]
        public ActionResult Create(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var fileName = string.Empty;
                if (model.Image != null &&
                    model.Image.ContentType.StartsWith("Image", StringComparison.OrdinalIgnoreCase))
                {
                    fileName = Guid.NewGuid().ToString();
                    fileName += model.Image.FileName.Substring(model.Image.FileName.LastIndexOf("."));
                    var fullName = System.IO.Path.Combine(Server.MapPath("~/Images"), "Products", fileName);
                    model.Image.SaveAs(fullName);
                }

                _db.Products.Add(new Product()
                {
                    Added = DateTime.UtcNow,
                    Description = model.Description,
                    Image = fileName,
                    Name = model.ProductName,
                    ProductCategoryId = model.SubCategoryId,
                    UserId = Convert.ToInt32(Session["UserId"]),
                    ExpiryDate = model.ExpiryDate,
                    Price = model.Price,
                    IsUsed = model.IsUsed
                });
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            model.Categories = GetCategories();
            model.SubCategories = new List<SelectListItem>(
                _db.ProductCategories.ToList()
                    .Where(x => x.MainCategoryId == Convert.ToInt32(model.Categories.FirstOrDefault().Value))
                    .Select(x => new SelectListItem() { Text = x.Name, Value = x.ProductCategoryId.ToString() }));
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Detail(int id)
        {
            var uId = Convert.ToInt32(Session["UserId"]);
            var product =
                _db.Products.Select(
                    x =>
                        new ProductListingModel()
                        {
                            Added = x.Added,
                            Description = x.Description,
                            FullName = x.UserLogin.FullName,
                            Image = x.Image,
                            MainProductCategory = x.ProductCategory.MainCategory.Name,
                            ProductId = x.ProductId,
                            ProductName = x.Name,
                            UserId = x.UserId,
                            ContactNumber = x.UserLogin.ContactNumber,
                            Email = x.UserLogin.Email,
                            Price = x.Price,
                            IsUsed = x.IsUsed,
                            Thumbs =
                                new ProductInfoModel
                                {
                                    ProductId = x.ProductId,
                                    Up = x.ProductThumbs.Count(y => y.Up),
                                    Down = x.ProductThumbs.Count(y => !y.Up),
                                    YourResponse =
                                        uId == 0
                                            ? (bool?)null
                                            : (x.ProductThumbs.Count(y => y.UserId == uId) == 0
                                                ? (bool?)null
                                                : x.ProductThumbs.Where(y => y.UserId == uId)
                                                    .Select(y => y.Up)
                                                    .FirstOrDefault()),
                                    Comments = x.UserComments.Count()
                                }
                        }).FirstOrDefault(x => x.ProductId == id);
            if (product != null)
            {
                return View(product);
            }
            return View("Error");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product =
                _db.Products.Select(
                    x =>
                        new ProductEditModel()
                        {
                            ProductId = x.ProductId,
                            ProductName = x.Name,
                            Description = x.Description,
                            ExpiryDate = x.ExpiryDate,
                            IsUsed = x.IsUsed,
                            Price = x.Price
                        })
                    .FirstOrDefault(x => x.ProductId == id);
            if (product != null)
            {
                return View(product);
            }
            return View("Error");
        }

        [HttpPost]
        public ActionResult Edit(ProductEditModel model)
        {
            if (ModelState.IsValid)
            {
                var prd = _db.Products.Find(model.ProductId);

                var existingFile = System.IO.Path.Combine(Server.MapPath("Images"), "Products", prd.Image);
                if (System.IO.File.Exists(existingFile))
                    System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("Images"), "Products", prd.Image));

                var fileName = string.Empty;
                if (model.Image != null &&
                    model.Image.ContentType.StartsWith("Image", StringComparison.OrdinalIgnoreCase))
                {
                    fileName = Guid.NewGuid().ToString();
                    fileName += model.Image.FileName.Substring(model.Image.FileName.LastIndexOf("."));
                    var fullName = System.IO.Path.Combine(Server.MapPath("~/Images"), "Products", fileName);
                    model.Image.SaveAs(fullName);
                }

                prd.Image = fileName;
                prd.Name = model.ProductName;
                prd.Description = model.Description;
                prd.IsUsed = model.IsUsed;
                prd.Price = model.Price;
                prd.ExpiryDate = model.ExpiryDate;

                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var prd = _db.Products.Find(id);
            if (prd != null)
            {
                _db.Products.Remove(prd);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Index(int page = 0)
        {
            var uId = Convert.ToInt32(Session["UserId"]);
            ViewBag.ItemsInRow = 4;

            var result =
                _db.Products.OrderByDescending(x => x.Added)
                    .Select(
                        x =>
                            new ProductListingModel
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
                                        YourResponse = x.ProductThumbs.Any(y => y.UserId == uId),
                                        Comments = x.UserComments.Count()
                                    }
                            }).Where(x => x.UserId == uId);
            var presult = new PaginatedList<ProductListingModel>(result, page, ViewBag.ItemsInRow * 2, 1);
            return View(presult);
        }

        [HttpGet]
        public PartialViewResult ThumbsUpDown(int id, int up)
        {
            try
            {
                var uId = Convert.ToInt32(Session["UserId"]);

                var prd = _db.Products.Find(id);
                var notif =
                               _db.Notifications.FirstOrDefault(
                                   x =>
                                       x.NotificationTypeId == 2 && x.OriginatorId == uId && x.UserId == prd.UserId &&
                                       x.ProductId == id);

                if (prd != null)
                {
                    var thmb = _db.ProductThumbs.FirstOrDefault(x => x.ProductId == id && x.UserId == uId);
                    if (up < 2)
                    {
                        if (thmb == null)
                        {
                            _db.ProductThumbs.Add(new ProductThumb { ProductId = id, Up = up == 1, UserId = uId });
                        }
                        else
                            thmb.Up = up == 1;

                        if (uId != prd.UserId)
                        {
                            if (notif != null)
                            {
                                notif.SentDate = DateTime.Now;
                                notif.ReadDate = null;
                            }
                            else
                                _db.Notifications.Add(new Notification
                                {
                                    NotificationTypeId = 2,
                                    OriginatorId = uId,
                                    UserId = prd.UserId,
                                    SentDate = DateTime.Now,
                                    ReadDate = null,
                                    ProductId = id
                                });
                        }
                        _db.SaveChanges();
                    }
                    if (up == 2 && thmb != null)
                    {
                        _db.ProductThumbs.Remove(thmb);
                        if (notif != null) _db.Notifications.Remove(notif);
                        _db.SaveChanges();
                    }
                    var you = prd.ProductThumbs.Where(y => y.UserId == uId).Select(x => x.Up).ToList();
                    return PartialView("_thumbs", new ProductInfoModel
                 {
                     ProductId = prd.ProductId,
                     Up = prd.ProductThumbs.Count(y => y.Up),
                     Down = prd.ProductThumbs.Count(y => !y.Up),
                     YourResponse = you.Any() ? you.First() : (bool?)null,
                     Comments = prd.UserComments.Count()
                 });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        [HttpGet]
        public ActionResult ThumbsDetail(int id, bool up = true, int page = 0)
        {
            try
            {
                //var uId = Convert.ToInt32(Session["UserId"]);

                var prd = _db.ProductThumbs.Where(x => x.ProductId == id && x.Up == up);
                if (prd.Any())
                {
                    var result =
                        prd.Select(
                            x =>
                                new ProductThumbModel
                                {
                                    UserId = x.UserId,
                                    Email = x.UserLogin.Email,
                                    FirstName = x.UserLogin.FullName
                                }).OrderBy(y => y.FirstName);
                    var presult = new PaginatedList<ProductThumbModel>(result, page, 10, 5);
                    return View(presult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return HttpNotFound();
        }

        public List<SelectListItem> GetCategories()
        {
            var lst = new List<SelectListItem>(_db.MainCategories.ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.MainCategoryId.ToString() }));
            return lst;
        }

        [HttpGet]
        public JsonResult GetSubCategories(string id)
        {
            var prdid = Convert.ToInt32(id);
            var lst =
                new List<SelectListItem>(
                    _db.ProductCategories.Where(x => x.MainCategoryId == prdid)
                        .ToList()
                        .Select(x => new SelectListItem() { Text = x.Name, Value = x.ProductCategoryId.ToString() }));
            return new JsonResult() { Data = lst, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult DeleteProductComment(int id, int cmtId, int page = 0)
        {
            var prdCmt = _db.UserComments.Find(cmtId);
            if (prdCmt != null)
            {
                _db.UserComments.Remove(prdCmt);
                _db.SaveChanges();
            }
            return GetProductComments(id.ToString(), page);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetProductComments(string id, int page = 0)
        {
            var prdid = Convert.ToInt32(id);
            var result =
                _db.UserComments.Where(x => x.ProductId == prdid)
                .OrderByDescending(x => x.Date)
                    .Select(
                        x =>
                            new ProductComment
                            {
                                UserCommentId = x.UserCommentId,
                                Comment = x.Comment,
                                Date = x.Date,
                                FullName = x.UserLogin.FullName,
                                ProductId = x.ProductId,
                                UserId = x.UserId
                            });
            var presult = new PaginatedList<ProductComment>(result, page, 10, 5);

            var uId = 0;
            if (Session["UserId"] != null)
                int.TryParse(Session["UserId"].ToString(), out uId);
            ViewBag.Owner = (_db.Products.Find(prdid).UserId == uId);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_comment", presult);
            }
            return View("_comment", presult);
        }

        public ActionResult SetProductComment(string productId, string comment)
        {
            var uId = Convert.ToInt32(Session["UserId"]);
            if (uId > 0)
            {
                if (string.IsNullOrEmpty(comment))
                {
                    ModelState.AddModelError(string.Empty, "Comment cannot be null.");
                    return GetProductComments(productId);
                }

                var prd = _db.Products.Find(Convert.ToInt32(productId));
                if (prd != null)
                {
                    _db.UserComments.Add(new UserComment()
                    {
                        Comment = comment,
                        ProductId = prd.ProductId,
                        UserId = Convert.ToInt32(Session["UserId"]),
                        Date = DateTime.Now
                    });
                    var notif =
                        _db.Notifications.FirstOrDefault(
                            x =>
                                x.NotificationTypeId == 1 && x.OriginatorId == uId && x.UserId == prd.UserId &&
                                x.ProductId == prd.ProductId);
                    if (uId != prd.UserId)
                    {
                        if (notif != null)
                        {
                            notif.SentDate = DateTime.Now;
                            notif.ReadDate = null;
                        }
                        else
                            _db.Notifications.Add(new Notification
                            {
                                NotificationTypeId = 1,
                                OriginatorId = uId,
                                UserId = prd.UserId,
                                SentDate = DateTime.Now,
                                ReadDate = null,
                                ProductId = prd.ProductId
                            });
                    }
                    _db.SaveChanges();
                }
            }
            return GetProductComments(productId);
        }
    }
}
