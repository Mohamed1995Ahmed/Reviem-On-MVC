using Microsoft.EntityFrameworkCore;
using WebApplication2.Models.Models;

namespace WebApplication2.Models.Data
{
	public class AppDBContext : DbContext
	{
		public AppDBContext(DbContextOptions<AppDBContext> options)
			: base(options)
		{
		}

		public DbSet<Employee> Employees { get; set; }
		public DbSet<Department> Departments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
			base.OnModelCreating(modelBuilder);
		}
	}
}