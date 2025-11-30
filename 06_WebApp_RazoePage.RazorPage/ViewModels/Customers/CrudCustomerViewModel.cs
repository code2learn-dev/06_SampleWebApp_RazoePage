using System.ComponentModel.DataAnnotations;

namespace _06_WebApp_RazoePage.RazorPage.ViewModels.Customers
{
	public class CrudCustomerViewModel : BaseViewModel
	{
		[Display(Name = "نام")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "نام کاربر را وارد کنید")]
		[StringLength(200, MinimumLength = 2, ErrorMessage = "نام کاربر باید بین 2 تا 200 حرف باشد")]
		public string FirstName { get; set; } = string.Empty;

		[Display(Name = "نام خانوادگی")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "نام خانوادگی کاربر را وارد کنید")]
		[StringLength(200, MinimumLength = 2, ErrorMessage = "نام خانوادگی کاربر باید بین 2 تا 200 حرف باشد")]
		public string LastName { get; set; } = string.Empty;

		[Display(Name = "کد ملی")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "کد ملی کاربر را وارد کنید")]
		[RegularExpression(@"^(\d{10})$", ErrorMessage = "کد ملی باید یک عدد 10 رقمی باشد")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public string NationalCode { get; set; } = string.Empty; 

		[Display(Name = "شماره تماس")]
		[Required(AllowEmptyStrings = false, ErrorMessage = "شماره تماس کاربر را وارد کنید")]
		[RegularExpression(@"^(\d{11})$", ErrorMessage = "شماره تماس باید یک عدد 11 رقمی باشد")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public string PhoneNumber { get; set; } = string.Empty; 

		[MaxLength(200, ErrorMessage = "نام فایل تصویر پروفایل نمی تواند بیش از 200 کاراکتر باشد")]
		public string ProfileImage { get; set; } = string.Empty;

		[ProfileImageValidator]
		public IFormFile? File { get; set; }
	}
}
