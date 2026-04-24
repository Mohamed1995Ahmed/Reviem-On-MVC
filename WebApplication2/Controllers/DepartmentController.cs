using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApplication2.Models.Data;
using WebApplication2.Models.Models;

public class DepartmentController : Controller
{
	private readonly AppDBContext _context;

	public DepartmentController(AppDBContext context)
	{
		_context = context;
	}

	public IActionResult Index()
	{
		var departments = _context.Departments.Include(d => d.Employees).ToList();
		return View(departments);
	}

	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	public IActionResult Create(Department department)
	{
		if (ModelState.IsValid)
		{
			_context.Departments.Add(department);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
		return View(department);
	}
}