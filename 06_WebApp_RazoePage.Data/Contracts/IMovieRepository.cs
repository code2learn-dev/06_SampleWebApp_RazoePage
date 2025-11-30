using _06_WebApp_RazoePage.Data.Models;
using _06_WebApp_RazoePage.Data.ProjectionModels;

namespace _06_WebApp_RazoePage.Data.Contracts
{
	public interface IMovieRepository : IGenericRepository<Movie>
	{
		Task<Movie?> GetMovieByIdWithTagsListAsync(long id); 
		Task<IEnumerable<MovieItemWithGenreTitle>> GetMovieItemsWithGenreTitle();
		Task<MovieProjectModel?> GetMovieProjectModelAsync(long? id);
		Task<IEnumerable<MovieProjectModel>> GetMoviesSidebarListAsync(Func<IQueryable<Movie>, IOrderedQueryable<Movie>>? orderFilter = null);
	}
}
