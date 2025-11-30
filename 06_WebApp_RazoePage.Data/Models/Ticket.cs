namespace _06_WebApp_RazoePage.Data.Models
{
	public class Ticket : BaseEntity
	{
		public long CustomerId { get; set; }
		public long MovieId { get; set; }
		public DateTime RegisterDate { get; set; }
		public DateTime ResevationDate { get; set; }
	}
}
