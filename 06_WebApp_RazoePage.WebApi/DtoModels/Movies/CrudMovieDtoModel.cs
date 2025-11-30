using _06_WebApp_RazoePage.WebApi.DtoModels.Tags;

namespace _06_WebApp_RazoePage.WebApi.DtoModels.Movies
{
	public class CrudMovieDtoModel : BaseDtoModel
	{
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal Score { get; set; } = decimal.Zero;
		public string ScoreString { get; set; } = string.Empty;
		public string ImageName { get; set; } = string.Empty;
		public DateTime StateDateDispaly { get; set; }
		public DateTime EndDateDisplay { get; set; } 

		public long GenreId { get; set; } 

		public IFormFile? File { get; set; }

		public List<TagItemDtoModel>? Tags { get; set; }

		public string TagsList { get; set; } = string.Empty;
	}
}
