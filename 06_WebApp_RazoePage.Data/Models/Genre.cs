namespace _06_WebApp_RazoePage.Data.Models
{
	public class Genre : BaseEntity
	{
		public string TItle { get; set; } = string.Empty;

		public ICollection<Movie>? Movies { get; set; }
	}
}
