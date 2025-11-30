using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.Data.Models;
using Microsoft.Extensions.Logging;

namespace _06_WebApp_RazoePage.Data.Repositories
{
	public class TagRepository : GenericRepository<Tag>, ITagRepository
	{
		public TagRepository(
			OnlineCinemaDbContext dbContext, 
			ILogger<GenericRepository<Tag>> logger) : base(dbContext, logger)
		{
		}
	}
}
