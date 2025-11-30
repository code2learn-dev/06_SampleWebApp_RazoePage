using System.Linq.Expressions;

namespace _06_WebApp_RazoePage.WebApi.DtoModels
{
	public class FilterDtoModel<TEntity>
	{
		public Expression<Func<TEntity, bool>>? Predicate { get; set; }
		public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy { get; set; }
	}
}
