namespace _06_WebApp_RazoePage.RazorPage.ViewModels.Tickets
{
	public class TicketItemViewModel : BaseViewModel
	{
		public long CustomerId { get; set; }
		public long MovieId { get; set; }
		public string CustomerName { get; set; } = string.Empty;
		public string MovieTitle { get; set; } = string.Empty;
		public string CustomerNationalCode { get; set; } = string.Empty;
		public string RegisterDate { get; set; } = string.Empty;
		public string ResevationDate { get; set; } = string.Empty;
	}
}
