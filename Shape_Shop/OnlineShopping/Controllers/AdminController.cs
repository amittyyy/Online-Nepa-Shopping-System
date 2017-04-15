namespace OnlineShopping.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using OnlineShopping.Helpers;

    public class AdminController : Controller
    {
        private readonly OnlineShoppingEntity _db = new OnlineShoppingEntity();


        public ActionResult Index(int page = 0)
        {
            if (Session["UserName"].ToString().StartsWith("admin", StringComparison.OrdinalIgnoreCase))
            {
                var result = _db.UserLogins.OrderBy(x => x.FullName);
                var presult = new PaginatedList<UserLogin>(result, page, 10, 5);
                return View(presult);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(int id, int page = 0)
        {
            var user = _db.UserLogins.Find(id);
            if (user != null)
            {
                var itsthmb = _db.ProductThumbs.Where(x => x.UserId == user.UserId);
                foreach (var productThumb in itsthmb) _db.ProductThumbs.Remove(productThumb);

                var itscmt = _db.UserComments.Where(x => x.UserId == user.UserId);
                foreach (var userComment in itscmt) _db.UserComments.Remove(userComment);

                var itsnotif = _db.Notifications.Where(x => x.UserId == user.UserId || x.OriginatorId == user.UserId);
                foreach (var notification in itsnotif) _db.Notifications.Remove(notification);

                var itsjob = _db.JobPostings.Where(x => x.UserId == user.UserId);
                foreach (var item in itsjob) _db.JobPostings.Remove(item);

                _db.UserLogins.Remove(user);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", "Admin", new { page = page });
        }

        public ActionResult DeleteJob(int id, string uri)
        {
            var job = _db.JobPostings.Find(id);
            if (job != null)
            {
                _db.JobPostings.Remove(job);
                _db.SaveChanges();
            }
            return Redirect(uri);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

    }
}