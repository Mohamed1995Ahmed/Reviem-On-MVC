using System.ComponentModel.DataAnnotations;

namespace WebApplication2.VModels
{
	public class CreateEmployeeVM
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public int DepartmentId { get; set; }
		public string? Image { get; set; }

	}
}
