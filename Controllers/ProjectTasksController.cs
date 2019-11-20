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

namespace Zilla.Controllers
{
    public class ProjectTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectTasks
        public async Task<ActionResult> Index()
        {
            return View(await db.ProjectTasks.ToListAsync());
        }

        // GET: ProjectTasks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = await db.ProjectTasks.FindAsync(id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // GET: ProjectTasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,Status,StartDate,EndDate")] ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                db.ProjectTasks.Add(projectTask);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(projectTask);
        }

        // GET: ProjectTasks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = await db.ProjectTasks.FindAsync(id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // POST: ProjectTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Status,StartDate,EndDate")] ProjectTask projectTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTask).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(projectTask);
        }

        // GET: ProjectTasks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTask projectTask = await db.ProjectTasks.FindAsync(id);
            if (projectTask == null)
            {
                return HttpNotFound();
            }
            return View(projectTask);
        }

        // POST: ProjectTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProjectTask projectTask = await db.ProjectTasks.FindAsync(id);
            db.ProjectTasks.Remove(projectTask);
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
    }
}
