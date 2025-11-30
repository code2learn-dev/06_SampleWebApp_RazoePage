using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis
{
	public class DeleteMovieViewModel : BaseViewModel
	{
		[Display(Name = "عنوان فیلم")]
		public string Title { get; set; } = string.Empty;
		
		[Display(Name = "توضیحات درباره فیلم")]
		public string Description { get; set; } = string.Empty;
		
		[Display(Name = "امتیاز فیلم")]
		public string ScoreString { get; set; } = string.Empty;
		
		public decimal Score { get; set; }
		
		public string ImageName { get; set; } = string.Empty;

		[Display(Name = "تاریخ شروع نمایش فیلم")]
		public string StartDateViewModel { get; set; } = string.Empty;

		[Display(Name = "تاریخ اتمام نمایش فیلم")]
		public string EndDateViewModel { get; set; } = string.Empty;
		  
		public string TagsList { get; set; } = string.Empty;

		[Display(Name = "انتخاب دسته فیلم")]
		public long GenreId { get; set; }

		public List<SelectListItem>? GenreSelectList { get; set; }
	}
}
