using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _06_WebApp_RazoePage.WebApi.Common
{
	public interface IModelStateArgs<TController> where TController : ControllerBase
	{
		string Message { get; }

		IReadOnlyList<string> MessagesList { get; }	

		void SetModelMessage(
			ModelState modelState, 
			HttpStatusCode statusCode,
			IReadOnlyList<string>? messages = null);
	}

	public enum ModelState : byte
	{
		create = 1,
		read,
		update,
		delete
	}
}
