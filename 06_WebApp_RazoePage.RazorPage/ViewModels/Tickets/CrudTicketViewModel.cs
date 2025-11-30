using System.ComponentModel.DataAnnotations;

namespace _06_WebApp_RazoePage.RazorPage.ViewModels.Tickets
{
	public class CrudTicketViewModel : BaseViewModel
	{
		[Required(
			AllowEmptyStrings = false, 
			ErrorMessage = "مشتری برای رزرئ بلیت انتخاب نشده است")]
		public long CustomerId { get; set; }

		[Required(
			AllowEmptyStrings = false, 
			ErrorMessage = "قیلمی برای رزرئ بلیت انتخاب نشده است")]
		public long MovieId { get; set; }

		[Display(Name = "نام مشتری")]
		public string CustomerName { get; set; } = string.Empty;
		
		[Display(Name = "کد ملی مشتری")]
		public string NationalCode { get; set; } = string.Empty;

		[Display(Name = "تاریخ ثبت فیلم")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "تاریخ ثبت بلیت را وارد کنید")]
		[TicketDateValidator(ErrorMessage = "تاریخ ثبت بلیت باید بزرگتر یا برابر تاریخ جاری باشد")]
		public string RegisterDate { get; set; } = string.Empty;
		
		[Display(Name = "تاریخ رزرو")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "تاریخ رزرو بلیت را وارد کنید")]
		[TicketDateValidator(ErrorMessage = "تاریخ رزور بلیت باید بزرگتر یا برابر تاریخ جاری باشد")]
		public string ResevationDate { get; set; } = string.Empty;

		[Display(Name = "عنوان فیلم")]
		public string Title { get; set; } = string.Empty;
	}
}
