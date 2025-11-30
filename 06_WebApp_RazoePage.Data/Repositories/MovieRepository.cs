using _06_WebApp_RazoePage.Data.Configs;
using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.Data.Extensions;
using _06_WebApp_RazoePage.Data.Models;
using _06_WebApp_RazoePage.Data.ProjectionModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace _06_WebApp_RazoePage.Data.Repositories
{
	public class MovieRepository
		: GenericRepository<Movie>,
		  IMovieRepository
	{
		public MovieRepository(
			OnlineCinemaDbContext dbContext,
			ILogger<GenericRepository<Movie>> logger) : base(dbContext, logger)
		{
		}

		public override async Task<Movie?> CreateEntityAsync(Movie entity)
		{
			entity.Tags = await MapMovieTags(entity);

			_dbSet.Add(entity);
			int createResult = await SaveModelChagens();

			return createResult > 0 ? entity : default;
		}

		private async Task<List<Tag>> MapMovieTags(Movie entity)
		{
			List<Tag> movieTags = [];
			if (entity.Tags is not null && entity.Tags.Any())
			{
				List<long> tagIds = entity.Tags.Select(a => a.Id).ToList();
				movieTags = await _dbContext.Tags.Where(t => tagIds.Contains(t.Id)).ToListAsync();
			}

			return movieTags;
		}

		public override async Task<Movie?> UpdateEntityAsync(Movie entity)
		{
			Movie? movieToUpdated = await _dbSet.Include(t => t.Tags)
				.FirstOrDefaultAsync(a => a.Id == entity.Id);
			if (movieToUpdated is null) return default;

			movieToUpdated.Title = entity.Title;
			movieToUpdated.Description = entity.Description;
			movieToUpdated.Score = entity.Score;
			movieToUpdated.ImageName = entity.ImageName;
			movieToUpdated.StateDateDispaly = entity.StateDateDispaly;
			movieToUpdated.EndDateDisplay = entity.EndDateDisplay;
			movieToUpdated.GenreId = entity.GenreId;

			if (movieToUpdated.Tags is not null && movieToUpdated.Tags.Any())
				movieToUpdated.Tags.SyncWith(entity.Tags ?? [], new EntityIdComparer<Tag>(a => a.Id));
			else
				movieToUpdated.Tags = await MapMovieTags(entity);

			int updatedMovies = await SaveModelChagens();
			return updatedMovies > 0 ? entity : default;
		}

		public async Task<IEnumerable<MovieItemWithGenreTitle>> GetMovieItemsWithGenreTitle()
		{
			List<MovieItemWithGenreTitle> movieItems = await _dbContext.Set<MovieItemWithGenreTitle>().FromSqlRaw("EXEC dbo.GetMovieItemWithGenreTitleList").ToListAsync();

			return movieItems;
		}

		public async Task<Movie?> GetMovieByIdWithTagsListAsync(long id)
			=> await _dbSet.Include(a => a.Tags).SingleOrDefaultAsync(a => a.Id == id);

		public async Task<IEnumerable<MovieProjectModel>> GetMoviesSidebarListAsync(Func<IQueryable<Movie>, IOrderedQueryable<Movie>>? orderFilter = null)
		{
			IQueryable<Movie> query = _dbSet;

			query = orderFilter is not null
				? orderFilter(query)
				: query.OrderBy(a => a.Id);

			var queryMovieSelector = query.Select(a => new MovieProjectModel()
			{
				Id = a.Id,
				Title = a.Title,
				ImageName = a.ImageName,
				Score = a.Score
			});

			return await queryMovieSelector.ToListAsync();
		}

		public async Task<MovieProjectModel?> GetMovieProjectModelAsync(long? id)
		{
			IQueryable<Movie> query = _dbSet;
			Movie? movie = await query.Where(a => a.Id == id).FirstOrDefaultAsync();

			return movie is not null
				? new MovieProjectModel()
				{
					Id = movie.Id,
					Title = movie.Title,
					Score = movie.Score,
					ImageName = movie.ImageName
				}
				: default;
		}
	}
}
