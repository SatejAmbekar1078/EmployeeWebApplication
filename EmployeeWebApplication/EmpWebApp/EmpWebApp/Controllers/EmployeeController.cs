using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using EmpWebApp.Models;

public class EmployeeController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: Employee
    public async Task<ActionResult> Index()
    {
        var employees = db.Employees.Include(e => e.Department);
        return View(await employees.ToListAsync());
    }

    // GET: Employee/Create
    public ActionResult Create()
    {
        ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
        return View();
    }

    // POST: Employee/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind(Include = "EmployeeId,FirstName,LastName,Email,DateOfJoining,DepartmentId")] Employee employee)
    {
        if (ModelState.IsValid)
        {
            db.Employees.Add(employee);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
        return View(employee);
    }

    // GET: Employee/Edit/5
    public async Task<ActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Employee employee = await db.Employees.FindAsync(id);
        if (employee == null)
        {
            return HttpNotFound();
        }
        ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
        return View(employee);
    }

    // POST: Employee/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "EmployeeId,FirstName,LastName,Email,DateOfJoining,DepartmentId")] Employee employee)
    {
        if (ModelState.IsValid)
        {
            db.Entry(employee).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
        return View(employee);
    }
}
