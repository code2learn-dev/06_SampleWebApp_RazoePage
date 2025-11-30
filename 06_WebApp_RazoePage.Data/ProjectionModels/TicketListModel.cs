using _06_WebApp_RazoePage.Data.Models;

namespace _06_WebApp_RazoePage.Data.ProjectionModels
{
	public class TicketListModel : BaseEntity
	{ 
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string NationalCode { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public long TicketId { get; set; }
		public DateTime RegisterDate { get; set; }
		public DateTime ResevationDate { get; set; }
	}
}
