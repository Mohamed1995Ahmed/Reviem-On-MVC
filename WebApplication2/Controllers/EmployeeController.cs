using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using WebApplication2.Models.Data;
using WebApplication2.Models.Models;

public class EmployeeController : Controller
{
	private readonly AppDBContext _context;

	private readonly IWebHostEnvironment _env;

	public EmployeeController(AppDBContext context, IWebHostEnvironment env)
	{
		_context = context;
		_env = env;
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
	public IActionResult DownloadImage(int id)
	{
		var emp = _context.Employees.Find(id);

		if (emp == null || string.IsNullOrEmpty(emp.Image))
			return NotFound();

		// full path: wwwroot/images/xxxx.jpg
		var path = Path.Combine(_env.WebRootPath, emp.Image.TrimStart('/'));
		  
		// check if file exists
		if (!System.IO.File.Exists(path))
			return NotFound();

		var bytes = System.IO.File.ReadAllBytes(path);

		var fileName = Path.GetFileName(path);

		return File(bytes, "application/octet-stream", fileName);
	}

	[HttpPost]
	public async Task<IActionResult> Create(Employee employee, IFormFile file)
	{
		
			if (file != null && file.Length > 0)
			{
				// 📁 Folder
				var folder = Path.Combine(_env.WebRootPath, "images");

				if (!Directory.Exists(folder))
					Directory.CreateDirectory(folder);

				// 🔹 Unique name
				var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
				var path = Path.Combine(folder, fileName);

				// 🔹 Save file
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				// 🔹 Save path in DB
				employee.Image = "/images/" + fileName;
			

			_context.Employees.Add(employee);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}

		ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name");
		return View(employee);
	}
	public IActionResult Edit(int id)
	{
		var emp = _context.Employees.Find(id);
		if (emp == null) return NotFound();

		ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", emp.DepartmentId);
		return View(emp);
	}
	[HttpPost]
	public async Task<IActionResult> Edit(int id, Employee employee, IFormFile file)
	{
		if (id != employee.Id) return NotFound();

		if (ModelState.IsValid)
		{
			var existingEmp = _context.Employees.AsNoTracking().FirstOrDefault(e => e.Id == id);
			if (existingEmp == null) return NotFound();

			// 🔹 If new image uploaded → replace old one
			if (file != null && file.Length > 0)
			{
				var folder = Path.Combine(_env.WebRootPath, "images");

				// delete old image
				if (!string.IsNullOrEmpty(existingEmp.Image))
				{
					var oldPath = Path.Combine(_env.WebRootPath, existingEmp.Image.TrimStart('/'));
					if (System.IO.File.Exists(oldPath))
						System.IO.File.Delete(oldPath);
				}

				// save new image
				var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
				var path = Path.Combine(folder, fileName);

				using (var stream = new FileStream(path, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				employee.Image = "/images/" + fileName;
			}
			else
			{
				// keep old image
				employee.Image = existingEmp.Image;
			}

			_context.Employees.Update(employee);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}

		ViewBag.Departments = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
		return View(employee);
	}
	public IActionResult Delete(int id)
	{
		var emp = _context.Employees.Find(id);
		if (emp == null) return NotFound();

		return View(emp);
	}
	[HttpPost, ActionName("Delete")]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var emp = _context.Employees.Find(id);
		if (emp == null) return NotFound();

		// 🔹 delete image from server
		if (!string.IsNullOrEmpty(emp.Image))
		{
			var path = Path.Combine(_env.WebRootPath, emp.Image.TrimStart('/'));
			if (System.IO.File.Exists(path))
				System.IO.File.Delete(path);
		}

		_context.Employees.Remove(emp);
		await _context.SaveChangesAsync();

		return RedirectToAction("Index");
	}
}