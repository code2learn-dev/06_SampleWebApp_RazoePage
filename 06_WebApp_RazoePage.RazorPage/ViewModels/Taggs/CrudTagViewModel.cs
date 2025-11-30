using System.ComponentModel.DataAnnotations;

namespace _06_WebApp_RazoePage.RazorPage.ViewModels.Taggs
{
	public class CrudTagViewModel : BaseViewModel
	{
		[Display(Name = "نام تگ")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "نام تگ را وارد کنید")]
		[StringLength(200, MinimumLength = 2, ErrorMessage = "عنوان تگ باید بین 2 تا 200 حرف باشد")]
		public string Name { get; set; } = string.Empty;

		public ActionMethod ActionMethod { get; set; } = ActionMethod.add;
	}

	public enum ActionMethod : byte
	{
		add = 1,
		edit,
		delete
	}
}
