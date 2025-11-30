namespace _06_WebApp_RazoePage.WebApi.DtoModels.Tickets
{
	public class TicketItemDtoModel : BaseDtoModel
	{
		public long CustomerId { get; set; }
		public long MovieId { get; set; }
		public string CustomerName { get; set; } = string.Empty;
		public string MovieTitle { get; set; } = string.Empty;
		public string CustomerNationalCode { get; set; } = string.Empty;
		public DateTime RegisterDate { get; set; }
		public DateTime ResevationDate { get; set; }
	}
}
