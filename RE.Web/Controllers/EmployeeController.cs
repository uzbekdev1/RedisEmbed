using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RE.Client.Core;
using RE.Client.Entities;
using RE.Web.Data;

namespace RE.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RedisProxy<Employee> _proxy;

        public EmployeeController(ApplicationDbContext context, RedisProxy<Employee> proxy)
        {
            _context = context;
            _proxy = proxy;
        }

        public IActionResult Index()
        {
            var model = _proxy.GetAll();

            return View(model);
        }

        public IActionResult Form(int? id)
        {
            var model = id > 0 ? _proxy.Get(id.Value) : new Employee();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Form(int? id, [Bind("FullName,DateOfBirth,SalaryExpectation,Id,IsDeleted")] Employee employee)
        {
            if (id > 0 && id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (id > 0)
                {
                    _context.Update(employee);
                }
                else
                {
                    _context.Add(employee);
                }
                await _context.SaveChangesAsync();

                //cache
                _proxy.Store(employee);

                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            //cache
            _proxy.Delete(id.Value);

            return RedirectToAction(nameof(Index));
        }

    }
}
