namespace _06_WebApp_RazoePage.WebApi.DtoModels.Customers
{
	public class CustomerItemDtoModel : BaseDtoModel
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty; 
		public string NationalCode { get; set; } = string.Empty;
	}
}
