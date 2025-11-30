using FluentValidation;

namespace _06_WebApp_RazoePage.WebApi.DtoModels.Tags
{
	public class TagValidator : AbstractValidator<CrudTagDtoModel>
	{
		public TagValidator() {
			RuleFor(a => a.Name)
				.NotEmpty().WithMessage("نام تگ را وارد کنید")
				.Length(2, 200).WithMessage("عنوان تگ باید بین 2 تا 200 حرف باشد");
		}
	}
}
