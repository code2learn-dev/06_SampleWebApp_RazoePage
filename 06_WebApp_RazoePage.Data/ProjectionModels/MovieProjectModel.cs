using _06_WebApp_RazoePage.Data.Models;

namespace _06_WebApp_RazoePage.Data.ProjectionModels
{
	public class MovieProjectModel : BaseEntity
	{
		public string Title { get; set; } = string.Empty;
		public decimal Score { get; set; }
		public string ImageName { get; set; } = string.Empty;
	}
}
