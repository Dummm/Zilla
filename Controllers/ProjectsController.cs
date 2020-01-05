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
    [Authorize(Roles = "Administrator, User")]
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
            if (HttpContext.User.IsInRole("Administrator"))
            {
                return View(await db.Projects.ToListAsync());
            }
            TempData["Toast"] = new Toast
            {
                Title = "Project",
                Body = "Access unauthorized!",
                Type = ToastType.Danger
            };
            return RedirectToAction("Index", "Home");
        }

        // GET: Projects/Details/5
        public async Task<ActionResult> Details(int? id)
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
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(project) || au.MemberInProjects.Contains(project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            return View(project);
        }

        // GET: Projects/Members/5
        public async Task<ActionResult> Members(int? id)
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
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(project) || au.MemberInProjects.Contains(project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            return View(
                new MembersViewModel { Project = project, Members = project.Members }
            );
        }

        // POST: Projects/Members/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Members([Bind(Include = "ProjectId,Members")] Project project)
        {
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(project) || au.MemberInProjects.Contains(project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/AddMember/5
        public async Task<ActionResult> AddMember(int? id)
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
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(project) || au.MemberInProjects.Contains(project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            //return View();
            ICollection<ApplicationUser> users = await db.Users.ToListAsync();
            users = users.Except(project.Members).ToList();
            return View(
                new AddMemberViewModel { 
                    Project = project, 
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
            AddMemberViewModel model)
        {
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            Project project = await db.Projects.FindAsync(int.Parse(id));
            if (!(au.OrganizerInProjects.Contains(project) || au.MemberInProjects.Contains(project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                foreach(string username in model.AddedMembers)
                {
                    ApplicationUser user = await UserManager.FindByNameAsync(username);
                    user = db.Users.Find(user.Id);
                    if (user != null)
                        //db.ProjectMembers.Add(new ProjectMembers {
                        //    ProjectId = t.ProjectId, 
                        //    ApplicationUserId = user.Id });
                        project.Members.Add(user);

                }

                await db.SaveChangesAsync();

                TempData["Toast"] = new Toast {
                    Title = "Project",
                    Body = "Member successfully added!",
                    Type = ToastType.Success
                };

                return RedirectToAction("Details", new { id = project.ProjectId });
            }
            TempData["Toast"] = new Toast
            {
                Title = "Project",
                Body = "Member add unsuccessful!",
                Type = ToastType.Danger
            };
            return View(model);
        }

        // Get: Projects/RemoveMember/5/1
        public async Task<ActionResult> RemoveMember (string id, string memberIndex)
        {
            if (id == null || memberIndex == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = await db.Projects.FindAsync(int.Parse(id));
            if (project == null)
            {
                return HttpNotFound();
            }

            ApplicationUser user = project.Members.ElementAt(int.Parse(memberIndex));
            if (user == null)
            {
                return HttpNotFound();
            }
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(project) || au.MemberInProjects.Contains(project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }

            return View(
                new RemoveMemberViewModel
                {
                    Project = project,
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
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            Project p = await db.Projects.FindAsync(int.Parse(id));
            if (!(au.OrganizerInProjects.Contains(p) || au.MemberInProjects.Contains(p) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                
                ApplicationUser u = db.Users.Find(
                    p.Members.ElementAt(int.Parse(memberIndex)).Id
                );

                p.Members.Remove(u);
                
                await db.SaveChangesAsync();
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Member successfully removed!",
                    Type = ToastType.Success
                };
                return RedirectToAction("Details", new { id = p.ProjectId });
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
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(project) || au.MemberInProjects.Contains(project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            return View(
                new OrganizersViewModel { Project = project, Organizers = project.Organizers }
            );
        }

        // POST: Projects/Organizers/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Organizers([Bind(Include = "ProjectId,Organizers")] Project project)
        {
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(project) || au.MemberInProjects.Contains(project)  || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/AddOrganizer/5
        public async Task<ActionResult> AddOrganizer(int? id)
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
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(project) || au.MemberInProjects.Contains(project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            //return View();
            ICollection<ApplicationUser> users = await db.Users.ToListAsync();
            users = users.Except(project.Organizers).ToList();
            return View(
                new AddOrganizerViewModel
                {
                    Project = project,
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
            AddOrganizerViewModel project)
        {
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            Project p = db.Projects.Find(int.Parse(id));
            if (!(au.OrganizerInProjects.Contains(p) || au.MemberInProjects.Contains(p) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                foreach (string username in project.AddedOrganizers)
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
            return View(project);
        }

        // Get: Projects/RemoveOrganizer/5/1
        public async Task<ActionResult> RemoveOrganizer(string id, string organizerIndex)
        {
            if (id == null || organizerIndex == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = await db.Projects.FindAsync(int.Parse(id));
            if (project == null)
            {
                return HttpNotFound();
            }

            ApplicationUser user = project.Organizers.ElementAt(int.Parse(organizerIndex));
            if (user == null)
            {
                return HttpNotFound();
            }
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            Project p = db.Projects.Find(int.Parse(id));
            if (!(au.OrganizerInProjects.Contains(p) || au.MemberInProjects.Contains(p) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }

            return View(
                new RemoveOrganizerViewModel
                {
                    Project = project,
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
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            Project p = await db.Projects.FindAsync(int.Parse(id));
            if (!(au.OrganizerInProjects.Contains(p) || au.MemberInProjects.Contains(p) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {

                ApplicationUser u = db.Users.Find(
                    p.Organizers.ElementAt(int.Parse(organizerIndex)).Id
                );

                p.Organizers.Remove(u);

                await db.SaveChangesAsync();
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Organizer successfully removed!",
                    Type = ToastType.Success
                };
                return RedirectToAction("Details", new { id = p.ProjectId });
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

            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
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
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            Project p = await db.Projects.FindAsync(id);
            if (!(au.OrganizerInProjects.Contains(p) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            //if (ModelState.IsValid)
            //{
            Assignment a = vm.Assignment;
            a.Assigner = db.Users.Find(HttpContext.User.Identity.GetUserId());
            a.Assignee = db.Users.Find(UserManager.FindByName(vm.Asignee).Id);
            db.Assignments.Add(a);

            p.Assignments.Add(a);

                await db.SaveChangesAsync();

            TempData["Toast"] = new Toast
            {
                Title = "Project",
                Body = "Assignment successfully added!",
                Type = ToastType.Success
            };
            return RedirectToAction("Details", "Assignments", new { id = a.AssignmentId});

            //}

            //return View(vm);
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
                return RedirectToAction("Details", "Projects", new { id = project.ProjectId });
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
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind(Include = "ProjectId,Title,Description")] Project model)
        {
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            Project project = await db.Projects.FindAsync(id);
            project.Description = model.Description;
            project.Title = model.Title;
            System.Diagnostics.Debug.WriteLine(project.Organizers.Count);
            if (!(au.OrganizerInProjects.Contains(project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Project edited successfully!",
                    Type = ToastType.Success
                };
                return RedirectToAction("Details", "Projects", new { id = model.ProjectId });
            }
            return View(model);
        }

        // GET: Projects/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(project) || au.MemberInProjects.Contains(project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Project project = await db.Projects.FindAsync(id);
            ApplicationUser au = db.Users.Find(HttpContext.User.Identity.GetUserId());
            if (!(au.OrganizerInProjects.Contains(project) || UserManager.IsInRole(au.Id, "Administrator")))
            {
                TempData["Toast"] = new Toast
                {
                    Title = "Project",
                    Body = "Access unauthorized!",
                    Type = ToastType.Danger
                };
                return RedirectToAction("Index", "Home");
            }
            db.Projects.Remove(project);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
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
