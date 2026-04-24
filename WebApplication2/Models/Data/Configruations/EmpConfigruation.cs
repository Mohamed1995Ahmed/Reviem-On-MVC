using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication2.Models.Models;

namespace WebApplication2.Models.Data.Configruations
{
	public class EmpConfigruation : IEntityTypeConfiguration<Employee>
	{
		public void Configure(EntityTypeBuilder<Employee> builder)
		{
			builder.HasKey(a=>a.Id);
			builder.HasOne(d=>d.Department).WithMany(d=>d.Employees).HasForeignKey(d=>d.DepartmentId);
		}
	}
}
