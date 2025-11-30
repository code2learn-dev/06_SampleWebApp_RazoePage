using _06_WebApp_RazoePage.WebApi.DtoModels.Tags;

namespace _06_WebApp_RazoePage.WebApi.DtoModels.Movies
{
	public class DeleteMovieDtoModel : BaseDtoModel
	{
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal Score { get; set; }
		public string ImageName { get; set; } = string.Empty;
		public DateTime StateDateDispaly { get; set; }
		public DateTime EndDateDisplay { get; set; }

		public List<TagItemDtoModel>? Tags { get; set; }

		public long GenreId { get; set; }
	}
}
