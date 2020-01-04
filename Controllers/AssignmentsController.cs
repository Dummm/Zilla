using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using Zilla.Models;

namespace Zilla.Controllers
{
    public class AssignmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Assignments
        public async Task<ActionResult> Index()
        {
            return View(await db.Assignments.ToListAsync());
        }

        // GET: Assignments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = await db.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // GET: Assignments/Create
        public async Task<ActionResult> Create(string projectId, string memberId)
        {
            if (projectId == null || memberId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project Project = await db.Projects.FindAsync(projectId);
            if (Project == null)
            {
                return HttpNotFound();
            }
            ApplicationUser user = Project.Members.ElementAt(int.Parse(memberId));
            if (user == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string projectId, string memberId, Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                Project project = await db.Projects.FindAsync(projectId);
                ApplicationUser user = project.Members.ElementAt(int.Parse(memberId));
                assignment.Assignee = user;
                assignment.Assigner = db.Users.Find(HttpContext.User.Identity.GetUserId());
                project.Assignments.Add(assignment);
                db.Assignments.Add(assignment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(assignment);
        }

        // GET: Assignments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = await db.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Status,StartDate,EndDate")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = await db.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Assignment assignment = await db.Assignments.FindAsync(id);
            db.Assignments.Remove(assignment);
            await db.SaveChangesAsync();
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



        public async Task<ActionResult> AddComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = await db.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            //return View();

            return View(
                new AddCommentViewModel
                {
                    Assignment = assignment
                }
            );

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddComment(int? id, AddCommentViewModel vm)
        {
            //if (ModelState.IsValid)
            //{
            Comment c = vm.Comment;
            c.Author = db.Users.Find(HttpContext.User.Identity.GetUserId());
            c.CreationDate = DateTime.Now;
            db.Comments.Add(c);

            Assignment a = db.Assignments.Find(id);
            a.Comments.Add(c);

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
            //}

            //return View(vm);
        }

    }
}
