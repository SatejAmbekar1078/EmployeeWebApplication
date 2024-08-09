using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using EmpWebApp.Models;

namespace EmpWebApp.Controllers
{
    public class EmployeeRoleController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: EmployeeRoles
        public async Task<ActionResult> Index()
        {
            return View(await db.EmployeeRoles.ToListAsync());
        }

        // GET: EmployeeRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeRoles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RoleId,RoleName")] EmployeeRole employeeRole)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeRoles.Add(employeeRole);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employeeRole);
        }

        // GET: EmployeeRoles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeRole employeeRole = await db.EmployeeRoles.FindAsync(id);
            if (employeeRole == null)
            {
                return HttpNotFound();
            }
            return View(employeeRole);
        }

        // POST: EmployeeRoles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RoleId,RoleName")] EmployeeRole employeeRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeRole).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employeeRole);
        }

        // GET: EmployeeRoles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeRole employeeRole = await db.EmployeeRoles.FindAsync(id);
            if (employeeRole == null)
            {
                return HttpNotFound();
            }
            return View(employeeRole);
        }

        // POST: EmployeeRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EmployeeRole employeeRole = await db.EmployeeRoles.FindAsync(id);
            db.EmployeeRoles.Remove(employeeRole);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
