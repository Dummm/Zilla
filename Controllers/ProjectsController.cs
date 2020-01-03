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
using System.Web.Security;
using System.Data.Entity.Validation;

namespace Zilla.Controllers
{
    public class ProjectsController : Controller
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

        // GET: Projects
        public async Task<ActionResult> Index()
        {
            return View(await db.Projects.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project Project = await db.Projects.FindAsync(id);
            if (Project == null)
            {
                return HttpNotFound();
            }
            return View(Project);
        }

        // GET: Projects/Members/5
        public async Task<ActionResult> Members(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project Project = await db.Projects.FindAsync(id);
            if (Project == null)
            {
                return HttpNotFound();
            }
            return View(
                new MembersViewModel { Project = Project, Members = Project.Members }
            );
        }

        // POST: Projects/Members/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Members([Bind(Include = "ProjectId,Members")] Project Project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Project);
        }

        // GET: Projects/AddMember/5
        public async Task<ActionResult> AddMember(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project Project = await db.Projects.FindAsync(id);
            if (Project == null)
            {
                return HttpNotFound();
            }
            //return View();
            ICollection<ApplicationUser> users = await db.Users.ToListAsync();
            users = users.Except(Project.Members).ToList();
            return View(
                new AddMemberViewModel { 
                    Project = Project, 
                    Users = users.Select(x => new SelectListItem()
                    {
                        Selected = false,
                        Text = x.UserName,
                        Value = x.UserName
                    })
                }
            );
            
        }
        // POST: Projects/AddMember/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddMember(
            string id,
            AddMemberViewModel Project)
        {
            if (ModelState.IsValid)
            {
                Project p = db.Projects.Find(int.Parse(id));
                foreach(string username in Project.AddedMembers)
                {
                    ApplicationUser user = await UserManager.FindByNameAsync(username);
                    user = db.Users.Find(user.Id);
                    if (user != null)
                        //db.ProjectMembers.Add(new ProjectMembers {
                        //    ProjectId = t.ProjectId, 
                        //    ApplicationUserId = user.Id });
                        p.Members.Add(user);

                }

                await db.SaveChangesAsync();

                TempData["Toast"] = new Toast {
                    Title = "Project",
                    Body = "Member successfully added!",
                    Type = ToastType.Success
                };

                return RedirectToAction("Details", new { id = p.ProjectId });
            }
            TempData["Toast"] = new Toast
            {
                Title = "Project",
                Body = "Member add unsuccessful!",
                Type = ToastType.Danger
            };
            return View(Project);
        }

        // Get: Projects/RemoveMember/5/1
        public async Task<ActionResult> RemoveMember (string id, string memberIndex)
        {
            if (id == null || memberIndex == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project Project = await db.Projects.FindAsync(int.Parse(id));
            if (Project == null)
            {
                return HttpNotFound();
            }

            ApplicationUser user = Project.Members.ElementAt(int.Parse(memberIndex));
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(
                new RemoveMemberViewModel
                {
                    Project = Project,
                    User = user
                }
            );

        }
        // DELETE: Projects/RemoveMember/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveMember(
            string id,
            string memberIndex,
            RemoveMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                Project t = await db.Projects.FindAsync(int.Parse(id));
                ApplicationUser u = db.Users.Find(
                    t.Members.ElementAt(int.Parse(memberIndex)).Id
                );

                t.Members.Remove(u);
                
                await db.SaveChangesAsync();
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Member successfully removed!",
                    Type = ToastType.Success
                };
                return RedirectToAction("Details", new { id = t.ProjectId });
            }
            TempData["Toast"] = new Toast
            {
                Title = "Project",
                Body = "Member removal unsuccessful!",
                Type = ToastType.Danger
            };
            return View(model);
        }


        // GET: Projects/Organizers/5
        public async Task<ActionResult> Organizers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project Project = await db.Projects.FindAsync(id);
            if (Project == null)
            {
                return HttpNotFound();
            }
            return View(
                new OrganizersViewModel { Project = Project, Organizers = Project.Organizers }
            );
        }

        // POST: Projects/Organizers/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Organizers([Bind(Include = "ProjectId,Organizers")] Project Project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Project);
        }

        // GET: Projects/AddOrganizer/5
        public async Task<ActionResult> AddOrganizer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project Project = await db.Projects.FindAsync(id);
            if (Project == null)
            {
                return HttpNotFound();
            }
            //return View();
            ICollection<ApplicationUser> users = await db.Users.ToListAsync();
            users = users.Except(Project.Organizers).ToList();
            return View(
                new AddOrganizerViewModel
                {
                    Project = Project,
                    Users = users.Select(x => new SelectListItem()
                    {
                        Selected = false,
                        Text = x.UserName,
                        Value = x.UserName
                    })
                }
            );

        }
        // POST: Projects/AddOrganizer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrganizer(
            string id,
            AddOrganizerViewModel Project)
        {
            if (ModelState.IsValid)
            {
                Project p = db.Projects.Find(int.Parse(id));
                foreach (string username in Project.AddedOrganizers)
                {
                    ApplicationUser user = await UserManager.FindByNameAsync(username);
                    user = db.Users.Find(user.Id);
                    if (user != null)
                        //db.ProjectOrganizers.Add(new ProjectOrganizers {
                        //    ProjectId = t.ProjectId, 
                        //    ApplicationUserId = user.Id });
                        p.Organizers.Add(user);

                }

                await db.SaveChangesAsync();

                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Organizer successfully added!",
                    Type = ToastType.Success
                };

                return RedirectToAction("Details", new { id = p.ProjectId });
            }
            TempData["Toast"] = new Toast
            {
                Title = "Project",
                Body = "Organizer add unsuccessful!",
                Type = ToastType.Danger
            };
            return View(Project);
        }

        // Get: Projects/RemoveOrganizer/5/1
        public async Task<ActionResult> RemoveOrganizer(string id, string organizerIndex)
        {
            if (id == null || organizerIndex == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project Project = await db.Projects.FindAsync(int.Parse(id));
            if (Project == null)
            {
                return HttpNotFound();
            }

            ApplicationUser user = Project.Organizers.ElementAt(int.Parse(organizerIndex));
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(
                new RemoveOrganizerViewModel
                {
                    Project = Project,
                    User = user
                }
            );

        }
        // DELETE: Projects/RemoveOrganizer/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveOrganizer(
            string id,
            string organizerIndex,
            RemoveOrganizerViewModel model)
        {
            if (ModelState.IsValid)
            {

                Project t = await db.Projects.FindAsync(int.Parse(id));
                ApplicationUser u = db.Users.Find(
                    t.Organizers.ElementAt(int.Parse(organizerIndex)).Id
                );

                t.Organizers.Remove(u);

                await db.SaveChangesAsync();
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Organizer successfully removed!",
                    Type = ToastType.Success
                };
                return RedirectToAction("Details", new { id = t.ProjectId });
            }
            TempData["Toast"] = new Toast
            {
                Title = "Project",
                Body = "Organizer removal unsuccessful!",
                Type = ToastType.Danger
            };
            return View(model);
        }


        public async Task<ActionResult> AddAssignment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            //return View();
            
            ICollection<ApplicationUser> users = await db.Users.ToListAsync();
            users = users.Intersect(project.Members).ToList();
            return View(
                new AddAssignmentViewModel
                {
                    Project = project,
                    Members = users.Select(x => new SelectListItem()
                    {
                        Selected = false,
                        Text = x.UserName,
                        Value = x.UserName
                    })
                }
            );

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAssignment(int? id, AddAssignmentViewModel vm)
        {
            //if (ModelState.IsValid)
            //{
            Assignment a = vm.Assignment;
                a.Assigner = db.Users.Find(User.Identity.GetUserId());
                a.Assignee = db.Users.Find(UserManager.FindByName(vm.Asignee).Id);
                db.Assignments.Add(a);

            Project p = db.Projects.Find(id);
                p.Assignments.Add(a);

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            //}

            return View(vm);
        }


        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProjectId,Title,Description")]  Project project)
        {
            if (ModelState.IsValid)
            {                
                ApplicationUser usr = db.Users.Find(HttpContext.User.Identity.GetUserId());
                project.Organizers.Add(usr);
                db.Projects.Add(project);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project Project = await db.Projects.FindAsync(id);
            if (Project == null)
            {
                return HttpNotFound();
            }
            return View(Project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProjectId,Title,Description")] Project Project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Project);
        }

        // GET: Projects/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project Project = await db.Projects.FindAsync(id);
            if (Project == null)
            {
                return HttpNotFound();
            }
            return View(Project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Project Project = await db.Projects.FindAsync(id);
            db.Projects.Remove(Project);
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
