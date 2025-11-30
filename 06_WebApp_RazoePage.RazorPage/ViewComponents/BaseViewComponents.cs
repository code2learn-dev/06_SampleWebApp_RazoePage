using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.ViewModels;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels;
using AutoMapper;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.ViewComponents
{
	public class BaseViewComponents : IBaseViewComponent 
	{
		protected readonly IHttpClientFactory _httpClientFactory;
		protected readonly HttpClient _client; 
		protected readonly ILogger<BaseViewComponents> _logger;
		private readonly IMapper _mapper;

		private JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
		{
			ContractResolver = new PrivateSetterContractResolver(),
			TypeNameHandling = TypeNameHandling.Auto,
			TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
		};

		public BaseViewComponents(
			IHttpClientFactory httpClientFactory, 
			ILogger<BaseViewComponents> logger,
			IMapper mapper)
		{
			_httpClientFactory = httpClientFactory;
			_client = _httpClientFactory.CreateClient("cafemovie");
			_logger = logger;
			_mapper = mapper;
		}

		public IMapper Mapper => _mapper;

		public IHttpClientFactory HttpClientFactory => _httpClientFactory;

		public HttpClient Client => _client;  

		public virtual async Task GetResponseErrorMessages<TEntityResponseResult>(HttpResponseMessage? response) where TEntityResponseResult : BaseDtoModel
		{
			if (response is null || response.IsSuccessStatusCode) return;

			string errorContent = await response.Content.ReadAsStringAsync();
			try
			{
				var appResult = JsonConvert.DeserializeObject<ApplicationServiceResult<TEntityResponseResult>>(errorContent, serializerSettings);

				if (appResult is not null &&
					appResult.Errors is not null &&
					appResult.Errors.Any())
					appResult.Errors.MapToMessages(MessageStatus.danger);
			}
			catch
			{
				await SetMessage("در حال حاضر سیستم قادر به پاسخ گویی نمی باشد", MessageStatus.danger);
				_logger.LogError(errorContent);
			}
		}

		public virtual Task SetMessage(string? message, MessageStatus messageStatus = MessageStatus.info)
		{
			if (string.IsNullOrWhiteSpace(message)) return Task.CompletedTask;

			message.AddMessage(messageStatus);

			return Task.CompletedTask;
		}
	}
}
