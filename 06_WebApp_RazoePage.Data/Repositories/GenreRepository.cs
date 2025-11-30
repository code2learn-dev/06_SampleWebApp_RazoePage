using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.Data.Models;
using Microsoft.Extensions.Logging;

namespace _06_WebApp_RazoePage.Data.Repositories
{
	public class GenreRepository
		: GenericRepository<Genre>,
		  IGenreRepository
	{
		public GenreRepository(
			OnlineCinemaDbContext dbContext, 
			ILogger<GenericRepository<Genre>> logger) : base(dbContext, logger)
		{
		}
	}
}
