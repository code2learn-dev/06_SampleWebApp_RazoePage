namespace _06_WebApp_RazoePage.Data.Models
{
	public class MovieItemWithGenreTitle : BaseEntity
	{
		public string GenreTitle { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal Score { get; set; }
		public string ImageName { get; set; } = string.Empty;
	}
}
