namespace _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis
{
	public class MovieProjectViewModel : BaseViewModel
	{
		public string Title { get; set; } = string.Empty;
		public decimal Score { get; set; }
		public string ImageName { get; set; } = string.Empty;
	}
}
