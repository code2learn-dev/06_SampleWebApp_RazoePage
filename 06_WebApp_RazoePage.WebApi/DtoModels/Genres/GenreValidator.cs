using FluentValidation;

namespace _06_WebApp_RazoePage.WebApi.DtoModels.Genres
{
	public class GenreValidator : AbstractValidator<CrudGenreDtoModel>
	{
		public GenreValidator()
		{
			RuleFor(a => a.Title)
				.NotEmpty().WithMessage("عنوان دسته فیلم را وارد کنید")
				.Length(2, 200).WithMessage("عنوان دسته فیلم باید بین 2 تا 200 حرف باشد");
		}
	}
}
