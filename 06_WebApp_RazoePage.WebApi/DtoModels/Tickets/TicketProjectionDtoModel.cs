namespace _06_WebApp_RazoePage.WebApi.DtoModels.Tickets
{
	public class TicketProjectionDtoModel : BaseDtoModel
	{
		public long CustomerId { get; set; }
		public long MovieId { get; set; }
		public DateTime RegisterDate { get; set; }
		public DateTime ResevationDate { get; set; }
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string NationalCode { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
	}
}
