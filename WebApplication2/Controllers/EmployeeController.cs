using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using WebApplication2.Models.Data;
using WebApplication2.Models.Models;

public class EmployeeController : Controller
{
	private readonly AppDBContext _context;

	public EmployeeController(AppDBContext context)
	{
		_context = context;
	}

	public IActionResult Index()
	{
		var employees = _context.Employees.ToList();
		return View(employees);
	}

	public IActionResult Create()
	{
		ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");
		return View();
	}

	[HttpPost]
	public IActionResult Create(Employee employee)
	{
		if (ModelState.IsValid)
		{
			_context.Employees.Add(employee);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}

		ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");
		return View(employee);
	}
}