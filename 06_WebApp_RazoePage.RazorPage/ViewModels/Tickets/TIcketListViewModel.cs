namespace _06_WebApp_RazoePage.RazorPage.ViewModels.Tickets
{
	public class TIcketListViewModel : BaseViewModel
	{ 
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string NationalCode { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public long TicketId { get; set; }
		public string RegisterDate { get; set; } = string.Empty;
		public string ResevationDate { get; set; } = string.Empty;
	}
}
