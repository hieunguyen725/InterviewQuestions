using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Interview.Models;
using Interview.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Security.Application;

namespace Interview.Controllers
{
    public class PostAnswersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateAnswer([Bind(Include = "PostID, Answer")]PostAnswerViewModel vm)
        {
            if (ModelState.IsValid)
            {
                PostAnswer answer = new PostAnswer()
                {
                    Answer = Sanitizer.GetSafeHtmlFragment(vm.Answer),
                    CreatedAt = DateTime.Now,
                    UserID = User.Identity.GetUserId(),
                    PostID = vm.PostID
                };
                db.PostAnswers.Add(answer);
                db.SaveChanges();
                return RedirectToAction("Details", "Posts", new { id = vm.PostID });
            }
            return RedirectToAction("Details", "Posts", new { id = vm.PostID });
        }

        // GET: PostAnswers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostAnswer postAnswer = db.PostAnswers.Find(id);
            if (postAnswer == null)
            {
                return HttpNotFound();
            }
            return View(postAnswer);
        }

        // POST: PostAnswers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostAnswerID,Answer,CreatedAt,PostID,UserID")] PostAnswer postAnswer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postAnswer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Posts", new { id= postAnswer.PostID});
            }
            return View(postAnswer);
        }

        // GET: PostAnswers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostAnswer postAnswer = db.PostAnswers.Find(id);
            if (postAnswer == null)
            {
                return HttpNotFound();
            }
            return View(postAnswer);
        }

        // POST: PostAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostAnswer postAnswer = db.PostAnswers.Find(id);
            db.PostAnswers.Remove(postAnswer);
            db.SaveChanges();
            return RedirectToAction("Details", "Posts", new { id = postAnswer.PostID });
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
