using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OnlineShopping.Helpers;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
{
    public class JobController : Controller
    {
        private readonly OnlineShoppingEntity _db = new OnlineShoppingEntity();

        //
        // GET: /User/
        [AllowAnonymous]
        public ActionResult Index(string search, string category, int page = 0)
        {
            var uId = 0;
            int.TryParse(Convert.ToString(Session["UserId"]), out uId);
            var cat = 0;
            int.TryParse(category, out cat);
            ViewBag.SearchJob = search;

            var result =
                _db.JobPostings
                .OrderByDescending(x => x.Added)
                //.Skip(0)
                //.Take(12)
                    .Where(
                        x =>
                            ((string.IsNullOrEmpty(search)) || (new[] { 0, 1 }.Contains(cat) && x.Title.Contains(search)) || (new[] { 0, 2 }.Contains(cat) && x.CompanyName.Contains(search)) || (new[] { 0, 3 }.Contains(cat) && x.JobCategory.Name.Contains(search))
                            ))
                    .Select(
                        x =>
                            new JobModel
                            {
                                UserId = x.UserId,
                                UserName = x.UserLogin.FullName,
                                CategoryName = x.JobCategory.Name,
                                City = x.City,
                                CompanyName = x.CompanyName,
                                Contact = x.Contact,
                                Education = x.Education,
                                Email = x.Email,
                                Experience = x.Experience,
                                JobId = x.JobId,
                                LastDate = x.EndDate,
                                Openings = x.Openings,
                                Posted = x.Added,
                                Requirements = x.Requirements,
                                Salary = x.Salary,
                                Specifications = x.Specifications,
                                Title = x.Title,
                                Url = x.URL
                            });
            //if (!job.Any())
            //    return HttpNotFound();
            var presult = new PaginatedList<JobModel>(result, page, 10, 5, search, cat);
            return View(presult);
        }

        public ActionResult Edit(int id)
        {
            var job = _db.JobPostings.Find(id);
            if (job == null)
                return HttpNotFound();

            var e = new JobEditModel
            {
                City = job.City,
                CompanyName = job.CompanyName,
                Contact = job.Contact,
                Education = job.Education,
                Email = job.Email,
                Experience = job.Experience,
                JobId = id,
                LastDate = job.EndDate,
                Openings = job.Openings,
                Requirements = job.Requirements,
                Salary = job.Salary,
                Specifications = job.Specifications,
                Title = job.Title,
                Url = job.URL
            };
            return View(e);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobEditModel request)
        {
            var uId = Convert.ToInt32(Session["UserId"]);
            if (ModelState.IsValid && uId > 0)
            {
                var job = _db.JobPostings.Find(request.JobId);
                if (job == null)
                    return HttpNotFound();
                if (job.UserId != uId)
                {
                    ModelState.AddModelError(string.Empty, "The job is not posted by this user.");
                    return View(request);
                }
                if (request.LastDate < DateTime.Today)
                {
                    ModelState.AddModelError(string.Empty, "The end date should be a future date.");
                    return View(request);
                }
                job.CompanyName = request.CompanyName;
                job.Title = request.Title;
                job.Salary = request.Salary;
                job.Experience = request.Experience;
                job.Openings = request.Openings;
                job.Education = request.Education;
                job.City = request.City;
                job.Contact = request.Contact;
                job.Email = request.Email;
                job.URL = request.Url;
                job.EndDate = request.LastDate;
                job.Requirements = request.Requirements;
                job.Specifications = request.Specifications;
                _db.SaveChanges();

                return RedirectToAction("Detail", new { id = job.JobId });
            }
            return View(request);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        [NonAction]
        public List<SelectListItem> GetCategories()
        {
            var lst = new List<SelectListItem>(_db.JobCategories.ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.JobCategoryId.ToString() }));
            return lst;
        }

        [HttpGet]
        public ActionResult Create()
        {
            var job = new JobModel
            {
                Categories = GetCategories(),
                LastDate = DateTime.Today.AddDays(20)
            };

            return View(job);
        }

        [HttpPost]
        public ActionResult Create(JobModel request)
        {
            var uId = 0;
            int.TryParse(Convert.ToString(Session["UserId"]), out uId);
            if (ModelState.IsValid && uId > 0)
            {
                var job = new JobPosting
                {
                    UserId = uId,
                    CompanyName = request.CompanyName,
                    Title = request.Title,
                    JobCategoryId = request.CategoryId,
                    Salary = request.Salary,
                    Experience = request.Experience,
                    Openings = request.Openings,
                    Education = request.Education,
                    City = request.City,
                    Requirements = request.Requirements,
                    Specifications = request.Specifications,
                    Contact = request.Contact,
                    Email = request.Email,
                    URL = request.Url,
                    EndDate = request.LastDate,
                    Added = DateTime.Now
                };
                _db.JobPostings.Add(job);
                _db.SaveChanges();

                return RedirectToAction("Detail", new { id = job.JobId });
            }
            request.Categories = GetCategories();
            return View(request);
        }

        public ActionResult Detail(int id)
        {
            var job = _db.JobPostings.Where(x => x.JobId == id)
                .Select(
                        x =>
                            new JobModel
                            {
                                UserId = x.UserId,
                                UserName = x.UserLogin.FullName,
                                CategoryName = x.JobCategory.Name,
                                City = x.City,
                                CompanyName = x.CompanyName,
                                Contact = x.Contact,
                                Education = x.Education,
                                Email = x.Email,
                                Experience = x.Experience,
                                JobId = x.JobId,
                                LastDate = x.EndDate,
                                Openings = x.Openings,
                                Posted = x.Added,
                                Requirements = x.Requirements,
                                Salary = x.Salary,
                                Specifications = x.Specifications,
                                Title = x.Title,
                                Url = x.URL
                            }).FirstOrDefault();

            if (job == null)
                return HttpNotFound();

            return View(job);
        }

        public ActionResult Delete(int id)
        {
            var job = _db.JobPostings.Find(id);
            if (job != null)
            {
                _db.JobPostings.Remove(job);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}