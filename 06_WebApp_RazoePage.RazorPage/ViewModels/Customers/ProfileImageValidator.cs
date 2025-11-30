using System.ComponentModel.DataAnnotations;

namespace _06_WebApp_RazoePage.RazorPage.ViewModels.Customers
{
	public class ProfileImageValidator : ValidationAttribute
	{
		private string[] validFormats = ["image/jpg", "image/jpeg", "image/png"];
		private decimal validaFileSize = 1024 * 1024 * 10;

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if(value is not null &&
				value is IFormFile file &&
				file.Length > 0)
			{
				if (file.Length > validaFileSize)
					return new ValidationResult("اندازه فایل انتخابی باید کمتر از 10 مگابایت  باشد");
				if (!validFormats.Contains(file.ContentType))
					return new ValidationResult("فرمت فایل انتخابی نامعتبر می باشد");
			}

			return ValidationResult.Success;
		} 
	}
}
