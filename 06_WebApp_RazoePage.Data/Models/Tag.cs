namespace _06_WebApp_RazoePage.Data.Models
{
	public class Tag : BaseEntity
	{
		public string Name { get; set; } = string.Empty;

		public List<Movie>? Movies { get; set; }
	}
}
