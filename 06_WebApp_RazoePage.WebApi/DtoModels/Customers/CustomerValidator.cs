using FluentValidation;
using System.Text.RegularExpressions;

namespace _06_WebApp_RazoePage.WebApi.DtoModels.Customers
{
	public class CustomerValidator : AbstractValidator<CrudCustomerDtoModel>
	{
		public CustomerValidator()
		{
			RuleFor(a => a.FirstName)
				.NotEmpty().WithMessage("نام کاربر را وارد کنید")
				.Length(2, 200).WithMessage("نام کاربر لابد بین 2 تا 200 حرف باشد");

			RuleFor(a => a.LastName)
				.NotEmpty().WithMessage("نام خانوادگی کاربر را وارد کنید")
				.Length(2, 200).WithMessage("نام خانوادگی کاربر لابد بین 2 تا 200 حرف باشد");

			RuleFor(a => a.NationalCode)
				.NotEmpty().WithMessage("کد ملی کاربر را وارد کنید")
				.Must(c => Regex.IsMatch(c.ToString(), @"^(\d{10})$"))
				.WithMessage("کد ملی کاربر باید عددی معتبر باشد");

			RuleFor(a => a.PhoneNumber)
				.NotEmpty().WithMessage("شماره تماس کاربر را وارد کنید")
				.Must(c => Regex.IsMatch(c.ToString(), @"^(\d{11})$"))
				.WithMessage("شماره تماس کاربر باید عددی معتبر باشد");

			RuleFor(a => a.ProfileImage)
				.MaximumLength(200).WithMessage("نام تصویر پروفایل باید کمتر از 200 حرف باشد");
		}
	}
}
