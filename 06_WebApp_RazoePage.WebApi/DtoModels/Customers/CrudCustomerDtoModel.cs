namespace _06_WebApp_RazoePage.WebApi.DtoModels.Customers
{
	public class CrudCustomerDtoModel : BaseDtoModel
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string NationalCode { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public string ProfileImage { get; set; } = string.Empty;

		public IFormFile? File { get; set; }
	}
}
