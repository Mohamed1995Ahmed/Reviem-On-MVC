using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication2.Models.Models;

namespace WebApplication2.Models.Data.Configruations
{
	public class DeptConfigruation : IEntityTypeConfiguration<Department>
	{
		public void Configure(EntityTypeBuilder<Department> builder)
		{
			builder.HasKey(x => x.Id);
			
		}
	}
}
