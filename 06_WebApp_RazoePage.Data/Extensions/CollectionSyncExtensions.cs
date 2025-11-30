namespace _06_WebApp_RazoePage.Data.Extensions
{
	public static class CollectionSyncExtensions
	{
		public static void SyncWith<TEntity>(
			this ICollection<TEntity> existing,
			IEnumerable<TEntity> updated,
			IEqualityComparer<TEntity>? comparer = null)
		{
			var toRemove = existing.Except(updated, comparer).ToList();
			var toAdd = updated.Except(existing, comparer).ToList();

			foreach (var item in toRemove) existing.Remove(item);

			foreach (var item in toAdd) existing.Add(item);
		}
	}
}
