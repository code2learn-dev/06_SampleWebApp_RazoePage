namespace _06_WebApp_RazoePage.WebApi.DtoModels.Tickets
{
	public class CrudTicketDtoItem : BaseDtoModel
	{
		public long CustomerId { get; set; }
		public long MovieId { get; set; }
		public DateTime RegisterDate { get; set; }
		public DateTime ResevationDate { get; set; }
	}
}
