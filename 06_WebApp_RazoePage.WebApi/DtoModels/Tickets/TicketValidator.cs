using FluentValidation;

namespace _06_WebApp_RazoePage.WebApi.DtoModels.Tickets
{
	public class TicketValidator : AbstractValidator<CrudTicketDtoItem>
	{
		public TicketValidator()
		{
			RuleFor(a => a.CustomerId)
				.NotEmpty().WithMessage("مشتری برای ثبت بلیت انتخاب نشده است");

			RuleFor(a => a.MovieId)
				.NotEmpty().WithMessage("فیلمی برای ثبت بلیت انتخاب نشده است");

			RuleFor(a => a.RegisterDate)
				.NotEmpty().WithMessage("تاریخ ثبت بلیت وارد نشده است")
				.NotEqual(default(DateTime)).WithMessage("تاریخ ثبت بلیت نامعتبر می باشد")
				.GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("تاریخ ثبت فیلم باید از تاریخ جاری در نظر گرفته شود");

			RuleFor(a => a.ResevationDate)
				.NotEmpty().WithMessage("تاریخ رزرو فیلم را وارد کنید")
				.NotEqual(default(DateTime)).WithMessage("تاریخ رزرو فیلم نامعتبر می باشد")
				.GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("تاریخ رزرو فیلم نباید قبل از تاریخ جاری باشد");
		}
	}
}
