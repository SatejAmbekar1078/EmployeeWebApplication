using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using EmpWebApp.Models;

namespace EmpWebApp.Controllers
{
    public class EmployeeRoleMappingController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: EmployeeRoleMappings
        public async Task<ActionResult> Index()
        {
            var employeeRoleMappings = db.EmployeeRoleMappings.Include(e => e.Employee).Include(e => e.EmployeeRole);
            return View(await employeeRoleMappings.ToListAsync());
        }

        // GET: EmployeeRoleMappings/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "FirstName");
            ViewBag.RoleId = new SelectList(db.EmployeeRoles, "RoleId", "RoleName");
            return View();
        }

        // POST: EmployeeRoleMappings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeId,RoleId,AssignedAt")] EmployeeRoleMapping employeeRoleMapping)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeRoleMappings.Add(employeeRoleMapping);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "FirstName", employeeRoleMapping.EmployeeId);
            ViewBag.RoleId = new SelectList(db.EmployeeRoles, "RoleId", "RoleName", employeeRoleMapping.RoleId);
            return View(employeeRoleMapping);
        }

        // GET: EmployeeRoleMappings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeRoleMapping employeeRoleMapping = await db.EmployeeRoleMappings.FindAsync(id);
            if (employeeRoleMapping == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "FirstName", employeeRoleMapping.EmployeeId);
            ViewBag.RoleId = new SelectList(db.EmployeeRoles, "RoleId", "RoleName", employeeRoleMapping.RoleId);
            return View(employeeRoleMapping);
        }

        // POST: EmployeeRoleMappings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeId,RoleId,AssignedAt")] EmployeeRoleMapping employeeRoleMapping)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeRoleMapping).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "FirstName", employeeRoleMapping.EmployeeId);
            ViewBag.RoleId = new SelectList(db.EmployeeRoles, "RoleId", "RoleName", employeeRoleMapping.RoleId);
            return View(employeeRoleMapping);
        }

        // GET: EmployeeRoleMappings/Delete/5
        public async Task<ActionResult> Delete(int? employeeId, int? roleId)
        {
            if (employeeId == null || roleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeRoleMapping employeeRoleMapping = await db.EmployeeRoleMappings.FindAsync(employeeId, roleId);
            if (employeeRoleMapping == null)
            {
                return HttpNotFound();
            }
            return View(employeeRoleMapping);
        }

        // POST: EmployeeRoleMappings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int employeeId, int roleId)
        {
            EmployeeRoleMapping employeeRoleMapping = await db.EmployeeRoleMappings.FindAsync(employeeId, roleId);
            db.EmployeeRoleMappings.Remove(employeeRoleMapping);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
