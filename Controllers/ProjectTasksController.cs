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
            Assignment Assignment = await db.Assignments.FindAsync(id);
            if (Assignment == null)
            {
                return HttpNotFound();
            }
            return View(Assignment);
        }

        // GET: Assignments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,Status,StartDate,EndDate")] Assignment Assignment)
        {
            if (ModelState.IsValid)
            {
                db.Assignments.Add(Assignment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(Assignment);
        }

        // GET: Assignments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment Assignment = await db.Assignments.FindAsync(id);
            if (Assignment == null)
            {
                return HttpNotFound();
            }
            return View(Assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Status,StartDate,EndDate")] Assignment Assignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Assignment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Assignment);
        }

        // GET: Assignments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment Assignment = await db.Assignments.FindAsync(id);
            if (Assignment == null)
            {
                return HttpNotFound();
            }
            return View(Assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Assignment Assignment = await db.Assignments.FindAsync(id);
            db.Assignments.Remove(Assignment);
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
