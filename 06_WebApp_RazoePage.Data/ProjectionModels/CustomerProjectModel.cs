using _06_WebApp_RazoePage.Data.Models;

namespace _06_WebApp_RazoePage.Data.ProjectionModels
{
	public class CustomerProjectModel : BaseEntity
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string NationalCode { get; set; } = string.Empty;
	}
}
