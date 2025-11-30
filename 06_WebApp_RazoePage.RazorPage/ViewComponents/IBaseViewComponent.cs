using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.WebApi.DtoModels;
using AutoMapper;

namespace _06_WebApp_RazoePage.RazorPage.ViewComponents
{
	public interface IBaseViewComponent
	{
		IHttpClientFactory HttpClientFactory { get; }
		HttpClient Client { get; }
		IMapper Mapper { get; } 

		Task GetResponseErrorMessages<TEntityResponseResult>(HttpResponseMessage? response)
			where TEntityResponseResult : BaseDtoModel;

		Task SetMessage(string? message, MessageStatus messageStatus = MessageStatus.info);
	}
}
