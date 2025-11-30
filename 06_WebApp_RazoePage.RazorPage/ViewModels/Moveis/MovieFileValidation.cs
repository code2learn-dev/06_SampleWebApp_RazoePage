using _06_WebApp_RazoePage.WebApi.Common;
using System.ComponentModel.DataAnnotations;

namespace _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis
{
	public class MovieFileValidation : ValidationAttribute
	{
		private const long maxFileSize = 1024 * 1024 * 5;
		private string[] validFormats = ["image/jpg", "image/jpeg", "image/png"]; 

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			ModelState modelViewState = CheckModelViewState(validationContext.ObjectInstance);

			if (modelViewState is ModelState.create &&
				value is null)
				return new ValidationResult("تصویری برای کاور فیلم انتخاب نشده است");

			if(modelViewState is ModelState.create &&
				value is IFormFile formFile &&
				(formFile is null || formFile.Length == 0))
				return new ValidationResult("تصویری برای کاور فیلم انتخاب نشده است");

			if(value is not null &&
				value is IFormFile file &&
				file.Length > 0)
			{
				if (file.Length > maxFileSize)
					return new ValidationResult("حجم فایل انتخابی باید کمتر از 5 مگابایت باشد");

				if (!validFormats.Contains(file.ContentType))
					return new ValidationResult("فرمت فایل انتخابی نامعتبر می باشد");
			}
			
			return ValidationResult.Success;
		}

		private ModelState CheckModelViewState(object model)
		{
			var modelId = model.GetType().GetProperty("Id");
			if(modelId is not null)
			{
				var idValue = modelId.GetValue(model);
				if (idValue is null || idValue.Equals(0))
					return ModelState.create;
			}

			return ModelState.update;
		}
	}
}
