namespace _06_WebApp_RazoePage.WebApi.DtoModels.Tickets
{
	public class TicketListDtoModel : BaseDtoModel
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
