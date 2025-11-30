
namespace _06_WebApp_RazoePage.WebApi.Common
{
	public class ApplicationServiceResultSelector
		: IApplicationServiceResultSelector
	{
		public ApplicationServiceResult<IEnumerable<TResult>> GetResultList<TResult>(IEnumerable<TResult>? result = null)
		{
			var appResult = new ApplicationServiceResult<IEnumerable<TResult>>();
			if (result is not null)
				appResult.AddResult(result);

			return appResult;
		}

		public ApplicationServiceResult<TResult> GetSingleResult<TResult>()
			=> new ApplicationServiceResult<TResult>();
	}
}
