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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Zilla.Controllers
{
    public class TeamsController : Controller
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

        // GET: Teams
        public async Task<ActionResult> Index()
        {
            return View(await db.Teams.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: Teams/Members/5
        public async Task<ActionResult> Members(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(
                new MembersViewModel { Team = team, Members = team.Members }
            );
        }

        // POST: Teams/Members/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Members([Bind(Include = "TeamId,Members")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // GET: Teams/AddMember/5
        public async Task<ActionResult> AddMember(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            //return View();
            ICollection<ApplicationUser> users = await db.Users.ToListAsync();
            users = users.Except(team.Members).ToList();
            return View(
                new AddMemberViewModel { 
                    Team = team, 
                    Users = users.Select(x => new SelectListItem()
                    {
                        Selected = false,
                        Text = x.UserName,
                        Value = x.UserName
                    })
                }
            );
            
        }
        // POST: Teams/AddMember/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddMember(
            string id,
            AddMemberViewModel team)
        {
            if (ModelState.IsValid)
            {
                Team t = db.Teams.Find(int.Parse(id));
                foreach(string username in team.AddedMembers)
                {
                    ApplicationUser user = await UserManager.FindByNameAsync(username);
                    user = db.Users.Find(user.Id);
                    if (user != null)
                        //db.TeamMembers.Add(new TeamMembers {
                        //    TeamId = t.TeamId, 
                        //    ApplicationUserId = user.Id });
                        t.Members.Add(user);

                }

                await db.SaveChangesAsync();

                TempData["Toast"] = new Toast {
                    Title = "Team",
                    Body = "Member successfully added!",
                    Type = ToastType.Success
                };

                return RedirectToAction("Index");
            }
            TempData["Toast"] = new Toast
            {
                Title = "Team",
                Body = "Member add unsuccessful!",
                Type = ToastType.Danger
            };
            return View(team);
        }

        // Get: Teams/RemoveMember/5/1
        public async Task<ActionResult> RemoveMember (string id, string memberIndex)
        {
            if (id == null || memberIndex == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Team team = await db.Teams.FindAsync(int.Parse(id));
            if (team == null)
            {
                return HttpNotFound();
            }

            ApplicationUser user = team.Members.ElementAt(int.Parse(memberIndex));
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(
                new RemoveMemberViewModel
                {
                    Team = team,
                    User = user
                }
            );

        }
        // DELETE: Teams/RemoveMember/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveMember(
            string id,
            string memberIndex,
            RemoveMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                Team t = await db.Teams.FindAsync(int.Parse(id));
                ApplicationUser u = db.Users.Find(
                    t.Members.ElementAt(int.Parse(memberIndex)).Id
                );

                t.Members.Remove(u);
                
                await db.SaveChangesAsync();
                TempData["Toast"] = new Toast
                {
                    Title = "Team",
                    Body = "Member successfully removed!",
                    Type = ToastType.Success
                };
                return RedirectToAction("Index");
            }
            TempData["Toast"] = new Toast
            {
                Title = "Team",
                Body = "Member removal unsuccessful!",
                Type = ToastType.Danger
            };
            return View(model);
        }


        // GET: Teams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TeamId,Title,Description")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TeamId,Title,Description")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = await db.Teams.FindAsync(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Team team = await db.Teams.FindAsync(id);
            db.Teams.Remove(team);
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
