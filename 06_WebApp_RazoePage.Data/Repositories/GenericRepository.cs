using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Reflection;

namespace _06_WebApp_RazoePage.Data.Repositories
{
	public class GenericRepository<Entity> : IGenericRepository<Entity>
		where Entity : BaseEntity
	{
		protected readonly OnlineCinemaDbContext _dbContext;
		protected readonly DbSet<Entity> _dbSet;
		protected readonly ILogger<GenericRepository<Entity>> _logger;

		public GenericRepository(
			OnlineCinemaDbContext dbContext, 
			ILogger<GenericRepository<Entity>> logger)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<Entity>();
			_logger = logger;
		}

		public async virtual Task<Entity?> CreateEntityAsync(Entity entity)
		{
			_dbSet.Add(entity);
			int createResult = await SaveModelChagens();
			return createResult > 0 ? entity : null;
		}

		public async virtual Task<Entity?> DeleteEntityAsync(Entity entity)
		{
			if (_dbContext.Entry(entity).State == EntityState.Detached)
				_dbContext.Entry(entity).State = EntityState.Deleted;

			_dbSet.Remove(entity);
			int deleteResult = await SaveModelChagens();
			return deleteResult > 0 ? entity : null;
		}

		public async virtual Task<IEnumerable<Entity>> GetAllAsync(Func<IQueryable<Entity>, IOrderedQueryable<Entity>>? orderFilter = null)
		{
			IQueryable<Entity> query = _dbSet;

			if (orderFilter != null)
				query = orderFilter(query);

			return await _dbSet.AsNoTracking().ToListAsync();
		}

		public async virtual Task<IEnumerable<Entity>> FilterByPredicate(
			Expression<Func<Entity, bool>> predicate,
			Func<IQueryable<Entity>, IOrderedQueryable<Entity>>? orderFilter)
		{
			IQueryable<Entity> query = _dbSet;

			if (predicate is not null)
				query = query.Where(predicate);

			return orderFilter is not null
				? await orderFilter(query).ToListAsync()
				: await query.ToListAsync();
		}

		public async virtual Task<Entity?> GetEntityWiIncludeOtherEntityAsync(long id, params string[] includes)
		{
			IQueryable<Entity> query = _dbSet;

			if(includes is not null && includes.Count() > 0)
			{
				foreach (var item in includes)
				{
					query = query.Include(item);
				}
			}

			Entity? entity = await query.FirstOrDefaultAsync(a => a.Id == id);
			return entity;
		}

		public async virtual Task<Entity?> GetEntityByIdAsync(long id)
			=> await _dbSet.SingleOrDefaultAsync(a => a.Id == id);

		public async Task<int> SaveModelChagens()
		{
			try
			{
				return await _dbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return -1;
			}
		}

		public async virtual Task<Entity?> UpdateEntityAsync(Entity entity)
		{
			if(_dbContext.Entry(entity).State == EntityState.Detached)
			{
				Entity? entityToUpdate = await GetEntityByIdAsync(entity.Id);
				if (entityToUpdate is null) return default;

				Type sourceType = entity.GetType();
				Type targetType = entityToUpdate.GetType();

				PropertyInfo[] sourceProps = sourceType.GetProperties();
				PropertyInfo[] targetProps = targetType.GetProperties();

				foreach (var prop in targetProps)
				{
					var sourceProperty = sourceProps.SingleOrDefault(
						a => a.GetType() == prop.GetType() && 
						a.Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase));

					if (sourceProperty is null) continue;

					var sourcePropValue = sourceProperty.GetValue(entity);
					prop.SetValue(entityToUpdate, sourcePropValue);
				}

				_dbSet.Update(entityToUpdate);
			}
			else
			{
				_dbSet.Update(entity);
			}

			int updateResult = await SaveModelChagens();
			return updateResult > 0 ? entity : default;
		}
	}
}
