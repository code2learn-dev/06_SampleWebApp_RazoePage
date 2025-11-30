using _06_WebApp_RazoePage.RazorPage.Extensions;
using System.ComponentModel.DataAnnotations;

namespace _06_WebApp_RazoePage.RazorPage.ViewModels.Tickets
{
	public class TicketDateValidator : ValidationAttribute
	{ 
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
				return new ValidationResult(ErrorMessage);

			DateTime inputDate = value.ToString().MapToGregorianDate();
			if (inputDate < DateTime.Now.Date)
				return new ValidationResult(ErrorMessage);

			return ValidationResult.Success;
		}
	}
}
