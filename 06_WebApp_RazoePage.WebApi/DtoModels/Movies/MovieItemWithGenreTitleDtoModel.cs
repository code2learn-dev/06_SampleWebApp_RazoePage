namespace _06_WebApp_RazoePage.WebApi.DtoModels.Movies
{
	public class MovieItemWithGenreTitleDtoModel : BaseDtoModel
	{
		public string GenreTitle { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal Score { get; set; }
		public string ImageName { get; set; } = string.Empty;
	}
}
