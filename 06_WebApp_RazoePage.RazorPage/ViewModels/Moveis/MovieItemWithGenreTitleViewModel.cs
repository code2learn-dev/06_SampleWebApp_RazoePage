namespace _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis
{
	public class MovieItemWithGenreTitleViewModel : BaseViewModel
	{
		public string GenreTitle { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal Score { get; set; }
		public string ImageName { get; set; } = string.Empty;
	}
}
