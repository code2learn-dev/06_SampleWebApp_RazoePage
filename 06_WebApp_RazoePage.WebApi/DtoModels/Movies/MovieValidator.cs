using FluentValidation;
using System.Text.RegularExpressions;

namespace _06_WebApp_RazoePage.WebApi.DtoModels.Movies
{
	public class MovieValidator : AbstractValidator<CrudMovieDtoModel>
	{
		public MovieValidator() {
			RuleFor(a => a.Title)
				.NotEmpty().WithMessage("عنوان فیلم را وارد کنید")
				.Length(2, 200).WithMessage("عنوان فیلم باید بین 2 تا 200 حرف باشد");

			RuleFor(a => a.Description)
				.NotEmpty().WithMessage("توضیحات فیلم را وارد کنید")
				.Length(20, 2000).WithMessage("توضیحات فیلم باید بین 20 تا 2000 حرف باشد");

			RuleFor(a => a.ScoreString)
				.NotEmpty().WithMessage("نمره فیلم را وارد کنید")
				.Must(a => Regex.IsMatch(a, @"^(([0-9])|([0-9]\.[0-9])|10)$"))
				.WithMessage("نمره دوره باید یک عدد اعشاری بین 0 تا 10 باشد");

			RuleFor(a => a.GenreId)
				.NotEmpty().WithMessage("دسته فیلم را انتخاب کنید")
				.Must(a => Regex.IsMatch(a.ToString(), @"^([1-9][0-9]*)$"))
				.WithMessage("دسته فیلم انتخابی نامعتبر می باشد");

			RuleFor(a => a.StateDateDispaly)
				.NotNull().WithMessage("تاریخ شروع نمایش را وارد کنید")
				.NotEqual(default(DateTime)).WithMessage("تاریخ شروع نمایش نامعتبر می باشد")
				.LessThanOrEqualTo(DateTime.Now).WithMessage("تاریخ شروع نمایش نباید تاریخی در آینده باشد");

			RuleFor(a => a.EndDateDisplay)
				.NotNull().WithMessage("تاریخ اتمام نمایش را وارد کنید")
				.NotEqual(default(DateTime)).WithMessage("تاریخ اتمام نمایش فیلم نامعتبر می باشد")
				.GreaterThan(DateTime.Now).WithMessage("تاریخ اتمام نمایش باید تاریخی در آینده باشد");

			RuleFor(a => a.ImageName) 
				.MaximumLength(200).WithMessage("نام فایل تصویر کاور فیلم باید بین 2 تا 200 حرف باشد");

		}
	}
}
