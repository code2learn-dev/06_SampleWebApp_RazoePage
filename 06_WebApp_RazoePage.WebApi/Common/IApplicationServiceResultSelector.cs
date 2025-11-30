namespace _06_WebApp_RazoePage.WebApi.Common
{
	public interface IApplicationServiceResultSelector
	{
		ApplicationServiceResult<IEnumerable<TResult>> GetResultList<TResult>(
			IEnumerable<TResult>? result = null);

		ApplicationServiceResult<TResult> GetSingleResult<TResult>();
	}
}
