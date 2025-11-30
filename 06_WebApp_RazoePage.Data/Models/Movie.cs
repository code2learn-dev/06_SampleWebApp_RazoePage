using System.Collections;

namespace _06_WebApp_RazoePage.Data.Models
{
	public class Movie : BaseEntity
	{
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal Score { get; set; }
		public string ImageName { get; set; } = string.Empty;
		public DateTime StateDateDispaly { get; set; }
		public DateTime EndDateDisplay { get; set; }

		public long GenreId { get; set; }

		public List<Tag>? Tags { get; set; }

		public ICollection<Ticket>? Tickets { get; set; } 
	}
}
