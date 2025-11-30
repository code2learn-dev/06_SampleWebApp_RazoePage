using _06_WebApp_RazoePage.Data.Models;
using System.Linq.Expressions;

namespace _06_WebApp_RazoePage.Data.Contracts
{
	public interface IGenericRepository<Entity> where Entity : BaseEntity
	{
		Task<IEnumerable<Entity>> GetAllAsync(Func<IQueryable<Entity>, IOrderedQueryable<Entity>>? orderFilter = null);

		Task<IEnumerable<Entity>> FilterByPredicate(
			Expression<Func<Entity,  bool>> predicate,
			Func<IQueryable<Entity>, IOrderedQueryable<Entity>>? orderFilter);

		Task<Entity?> GetEntityWiIncludeOtherEntityAsync(long id, params string[] includes);

		Task<Entity?> GetEntityByIdAsync(long id);

		Task<Entity?> CreateEntityAsync(Entity entity);

		Task<Entity?> UpdateEntityAsync(Entity entity);

		Task<Entity?> DeleteEntityAsync(Entity entity);

		Task<int> SaveModelChagens();
	}
}
