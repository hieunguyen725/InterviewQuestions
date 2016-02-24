using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Interview.Models;
using Microsoft.AspNet.Identity;
using Interview.ViewModels;

namespace Interview.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private List<string> categories;

        public PostsController()
        {
            categories = new List<string>
            {
                "Data Structure", "Algorithm", "Operating System",
                "Programming Fundamentals", "Mobile Development",
                "Web Development", "Database", "Other"
            };
            ViewBag.Categories = categories;
        }

        [AllowAnonymous]
        public PartialViewResult LoadCategory(string category)
        {
            List<Post> model;
            if (category == "All")
            {
                model = db.Posts.ToList();
            }
            else
            {
                model = db.Posts.Where(q => q.SelectedCategory == category).ToList();
            }
            ViewBag.userId = User.Identity.GetUserId();
            return PartialView("_posts", model);
        }

        [AllowAnonymous]
        public PartialViewResult LatestPosts()
        {
            var model = db.Posts.OrderByDescending(p => p.CreatedAt).Take(10).ToList();
            return PartialView("_LatestPosts", model);
        }

        [AllowAnonymous]
        // GET: Posts
        public ActionResult Index()
        {
            ViewBag.userId = User.Identity.GetUserId();
            return View(db.Posts.ToList());
        }

        [AllowAnonymous]
        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.userId = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            post.ViewCount++;
            db.SaveChanges();
            PostAnswerViewModel vm = new PostAnswerViewModel
            {
                PostID = post.PostID,
                PostName = post.Name,
                PostQuestion = post.PostQuestion,
                PostAnswers = post.PostAnswers,
                CreatedAt = post.CreatedAt,
                User = post.User
            };
            return View(vm);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,PostQuestion,SelectedCategory")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.UserID = User.Identity.GetUserId();
                post.CreatedAt = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if(User.Identity.GetUserId() != post.UserID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,Name,PostQuestion,CreatedAt,UserID,ViewCount,SelectedCategory")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (User.Identity.GetUserId() != post.UserID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
