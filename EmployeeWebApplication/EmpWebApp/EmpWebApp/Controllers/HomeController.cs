using EmpWebApp.Data;
using EmpWebApp.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;

namespace EmpWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.EmployeeRoleMappings)
                .ThenInclude(erm => erm.EmployeeRole)
                .ToListAsync();

            if (User.IsInRole("Admin"))
            {
                return View("AdminEmployeeList", employees);
            }
            else
            {
                return View("UserEmployeeList", employees);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewBag.EmployeeRoleId = new SelectList(_context.EmployeeRoles, "EmployeeRoleId", "RoleName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(Employee employee, int[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                foreach (var roleId in selectedRoles)
                {
                    _context.EmployeeRoleMappings.Add(new EmployeeRoleMapping
                    {
                        EmployeeId = employee.EmployeeId,
                        EmployeeRoleId = roleId
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
            ViewBag.EmployeeRoleId = new SelectList(_context.EmployeeRoles, "EmployeeRoleId", "RoleName");
            return View(employee);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.EmployeeRoleMappings)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return HttpNotFound();
            }

            ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
            ViewBag.EmployeeRoleId = new MultiSelectList(_context.EmployeeRoles, "EmployeeRoleId", "RoleName", employee.EmployeeRoleMappings.Select(erm => erm.EmployeeRoleId));
            return View(employee);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int id, Employee employee, int[] selectedRoles)
        {
            if (id != employee.EmployeeId)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(employee).State = EntityState.Modified;

                    var existingMappings = _context.EmployeeRoleMappings.Where(erm => erm.EmployeeId == id).ToList();
                    _context.EmployeeRoleMappings.RemoveRange(existingMappings);

                    foreach (var roleId in selectedRoles)
                    {
                        _context.EmployeeRoleMappings.Add(new EmployeeRoleMapping
                        {
                            EmployeeId = employee.EmployeeId,
                            EmployeeRoleId = roleId
                        });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Employees.Any(e => e.EmployeeId == employee.EmployeeId))
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
            ViewBag.EmployeeRoleId = new MultiSelectList(_context.EmployeeRoles, "EmployeeRoleId", "RoleName", employee.EmployeeRoleMappings.Select(erm => erm.EmployeeRoleId));
            return View(employee);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.EmployeeRoleMappings)
                .ThenInclude(erm => erm.EmployeeRole)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
