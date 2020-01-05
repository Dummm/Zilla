using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Zilla.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Zilla.Controllers
{
    [Authorize(Roles = "Administrator, User")]
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Comments
        public async Task<ActionResult> Index()
        {
            if (HttpContext.User.IsInRole("Administrator"))
            {
                return View(await db.Comments.ToListAsync());
            }
            TempData["Toast"] = new Toast
            {
                Title = "Project",
                Body = "Access unauthorized!",
                Type = ToastType.Danger
            };
            return RedirectToAction("Index", "Home");
        }

        // GET: Comments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(comment.Assignment.Project) || au.MemberInProjects.Contains(comment.Assignment.Project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Comment",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CommentId,Content,CreationDate")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(comment.Assignment.Project) || au.MemberInProjects.Contains(comment.Assignment.Project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Comment",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Details", "Assignments", new { id = comment.Assignment.AssignmentId });
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CommentId,Content,CreationDate")] Comment comment)
        {
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(comment.Assignment.Project) || au.MemberInProjects.Contains(comment.Assignment.Project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Comment",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Assignments", new { id = comment.Assignment.AssignmentId });
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(comment.Assignment.Project) || au.MemberInProjects.Contains(comment.Assignment.Project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Comment",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Comment comment = await db.Comments.FindAsync(id);
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(comment.Assignment.Project) || au.MemberInProjects.Contains(comment.Assignment.Project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Comment",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            var aid = comment.Assignment.AssignmentId;
            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Assignments", new { id = aid });
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
