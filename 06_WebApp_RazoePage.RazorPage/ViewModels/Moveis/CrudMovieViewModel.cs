using _06_WebApp_RazoePage.RazorPage.ViewModels.Taggs;
using _06_WebApp_RazoePage.WebApi.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SqlServer.Server;
using System.ComponentModel.DataAnnotations;

namespace _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis
{
	public class CrudMovieViewModel : BaseViewModel
	{
		[Display(Name = "عنوان فیلم")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "عنوان فیلم را وارد کنید")]
		[StringLength(200, MinimumLength = 2, ErrorMessage = "عنوان فیلم باید بین 2 تا 200 حرف باشد")]
		public string Title { get; set; } = string.Empty;

		[Display(Name = "توضیحات درباره فیلم")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "توضیحات فیلم را وارد کنید")]
		[StringLength(2000, MinimumLength = 20, ErrorMessage = "توضیحات فیلم باید بین 20 تا 2000 حرف باشد")] 
		public string Description { get; set; } = string.Empty;

		[Display(Name = "امتیاز فیلم")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "امتیاز فیلم را وارد کنید")]
		[RegularExpression(@"^(([0-9])|([0-9]\.[0-9])|10)$", ErrorMessage = "امیتاز فیلم باید عددی بین صفر و 10 باشد")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public string ScoreString { get; set; } = string.Empty;

		public decimal Score { get; set; } = decimal.Zero;

		[Display(Name = "تصویر کاور فیلم")] 
		public string ImageName { get; set; } = string.Empty;

		[Display(Name = "تاریخ شروع نمایش فیلم")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "تاریخ شروع نمایش فیلم را وارد کنید")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/mm/dd}")]
		public string StartDateViewModel { get; set; } = string.Empty;
	
		[Display(Name = "تاریخ اتمام نمایش فیلم")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "تاریخ اتمام نمایش فیلم را وارد کنید")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/mm/dd}")]
		public string EndDateViewModel { get; set; } = string.Empty;

		[Display(Name = "انتخاب دسته فیلم")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "دسته فیلم را انتخاب کنید")]
		[RegularExpression(@"^([1-9][0-9]*)$", ErrorMessage = "دسته انتخابی فیلم مامعتبر می باشد")]
		public long GenreId { get; set; }

		public List<SelectListItem>? GenreSelectList { get; set; }

		[MovieFileValidation]
		public IFormFile? File { get; set; } 

		public string TagsList { get; set; } = string.Empty;
	}
}
