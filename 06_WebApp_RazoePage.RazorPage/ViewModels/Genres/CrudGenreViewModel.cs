using System.ComponentModel.DataAnnotations;

namespace _06_WebApp_RazoePage.RazorPage.ViewModels.Genres
{
	public class CrudGenreViewModel : BaseViewModel
	{
		[Display(Name = "عنوان دسته بندی")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "عنوان دسته فیلم را وارد کنید")]
		[StringLength(200, MinimumLength = 2, ErrorMessage = "عنوان دسته فیلم باید بین 2 تا 200 حرف باشد")]
		public string Title { get; set; } = string.Empty;
	}
}
