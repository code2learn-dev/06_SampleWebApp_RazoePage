using _06_WebApp_RazoePage.Data.Models;
using System.Diagnostics.CodeAnalysis;

namespace _06_WebApp_RazoePage.Data.Configs
{
	public class EntityIdComparer<TEntity> : IEqualityComparer<TEntity>
		where TEntity : BaseEntity
	{
		private readonly Func<TEntity, object> _keySelector;

		public EntityIdComparer(Func<TEntity, object> keySelector)
		{
			_keySelector = keySelector;
		}

		public bool Equals(TEntity? x, TEntity? y) 
			=> _keySelector(x!).Equals(_keySelector(y!));

		public int GetHashCode([DisallowNull] TEntity obj)
			=> _keySelector(obj).GetHashCode();
	}
}
