using System.ComponentModel.DataAnnotations;
using WebApplication2.Models.Data;

namespace WebApplication2.validation
{
	public class UniqueNameValidation:ValidationAttribute	
	{
		override protected ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var context = (AppDBContext)validationContext.GetService(typeof(AppDBContext));
			var name = value as string;
			var exists = context.Departments.Any(e => e.Name == name);
			if (exists)
				return new ValidationResult("Name must be unique");
			return ValidationResult.Success;
		}
	}
}
