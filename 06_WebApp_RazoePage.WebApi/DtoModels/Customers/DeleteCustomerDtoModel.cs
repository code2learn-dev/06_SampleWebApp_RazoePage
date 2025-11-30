namespace _06_WebApp_RazoePage.WebApi.DtoModels.Customers
{
	public class DeleteCustomerDtoModel : BaseDtoModel
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string NationalCode { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public string ProfileImage { get; set; } = string.Empty;
	}
}
