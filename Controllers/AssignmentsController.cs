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
    [Authorize(Roles = "Administrator, User")]
    public class AssignmentsController : Controller
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

        // GET: Assignments
        public async Task<ActionResult> Index()
        {
            if(HttpContext.User.IsInRole("Administrator"))
            {
                return View(await db.Assignments.ToListAsync());
            }
            TempData["Toast"] = new Toast
            {
                Title = "Project",
                Body = "nu poti!",
                Type = ToastType.Danger
            };
            return RedirectToAction("Index", "Home");
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
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(assignment.Project.Members.Union(assignment.Project.Organizers).Contains(au) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Assignment",
                    Body = "nu poti!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
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
            Project project = await db.Projects.FindAsync(projectId);
            if (project == null)
            {
                return HttpNotFound();
            }
            ApplicationUser user = project.Members.ElementAt(int.Parse(memberId));
            if (user == null)
            {
                return HttpNotFound();
            }
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(project.Members.Union(project.Organizers).Contains(au) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Assignment",
                    Body = "nu poti!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
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
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(assignment.Project.Members.Union(assignment.Project.Organizers).Contains(au) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Assignment",
                    Body = "nu poti!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
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
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(assignment.Project.Members.Union(assignment.Project.Organizers).Contains(au) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Assignment",
                    Body = "nu poti!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
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
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(assignment.Project.Members.Union(assignment.Project.Organizers).Contains(au) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Assignment",
                    Body = "nu poti!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
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
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(assignment.Project.Members.Union(assignment.Project.Organizers).Contains(au) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Assignment",
                    Body = "nu poti!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Assignment assignment = await db.Assignments.FindAsync(id);
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(assignment.Project.Members.Union(assignment.Project.Organizers).Contains(au) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Assignment",
                    Body = "nu poti!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
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

            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(assignment.Project.Members.Union(assignment.Project.Organizers).Contains(au) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Assignment",
                    Body = "nu poti!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }

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

            TempData["Toast"] = new Toast
            {
                Title = "Project",
                Body = "Comment successfully added!",
                Type = ToastType.Success
            };
            return RedirectToAction("Details", "Assignments", new { id = a.AssignmentId });
            //}

            //return View(vm);
        }

    }
}
