using _06_WebApp_RazoePage.Data.Models;
using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.ViewModels;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.BasePage
{
	public abstract class BaseRazorPage<TEntityItemDtoModel, TEntityItemViewModel> : PageModel
		where TEntityItemDtoModel : BaseDtoModel
		where TEntityItemViewModel : BaseViewModel
	{
		protected readonly IHttpClientFactory _httpClientFactory;
		protected readonly HttpClient _client;
		protected string _serviceName;
		protected ILogger<BaseRazorPage<TEntityItemDtoModel, TEntityItemViewModel>> _logger;
		protected readonly IMapper _mapper;

		protected const string IndexPage = "Index";
		protected const string ErrorPage = "/Error/Index";

		protected readonly JsonSerializerSettings serializerSetting =
			new JsonSerializerSettings()
			{
				ContractResolver = new PrivateSetterContractResolver(),
				TypeNameHandling = TypeNameHandling.Auto,
				TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
				Formatting = Formatting.Indented
			};

		public BaseRazorPage(
			IHttpClientFactory httpClientFactory, 
			string serviceName,
			ILogger<BaseRazorPage<TEntityItemDtoModel, TEntityItemViewModel>> logger,
			IMapper mapper)
		{
			_httpClientFactory = httpClientFactory;
			_serviceName = serviceName;
			_client = _httpClientFactory.CreateClient("cafemovie");
			_logger = logger;
			_mapper = mapper;
		}

		protected async Task<TEntityItemViewModel?> ProcessResponseAsync(
			HttpResponseMessage response, 
			string errorMessage)
		{
			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<TEntityItemDtoModel>(response);
				return default;
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<ApplicationServiceResult<TEntityItemDtoModel?>>(contentResult, serializerSetting);
			if (appResult is null)
			{
				await SetMessage(errorMessage, MessageStatus.danger);
				return default;
			}

			if (appResult.IsSuccess && appResult.Result is not null)
			{
				appResult.Messages.MapToMessages(MessageStatus.success);
				TEntityItemViewModel entityResultViewModel = _mapper.Map<TEntityItemViewModel>(appResult.Result);
				return entityResultViewModel;
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
			return default;
		} 

		protected virtual async Task GetResponseErrorMessages<TEntityResponseResult>(HttpResponseMessage? response) where TEntityResponseResult : BaseDtoModel
		{
			if (response is null || response.IsSuccessStatusCode) return;

			string errorContent = await response.Content.ReadAsStringAsync();
			try
			{
				var appResult = JsonConvert.DeserializeObject<ApplicationServiceResult<TEntityResponseResult>>(errorContent, serializerSetting);

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

		protected virtual Task SetMessagesList(
			IReadOnlyList<string>? errors,
			MessageStatus messageStatus = MessageStatus.info)
		{
			if (errors is null || !errors.Any()) return Task.CompletedTask;

			errors.MapToMessages(messageStatus);

			return Task.CompletedTask;
		}

		protected virtual Task SetMessage(string? message, MessageStatus messageStatus = MessageStatus.info)
		{
			if (string.IsNullOrWhiteSpace(message)) return Task.CompletedTask;

			message.AddMessage(messageStatus);

			return Task.CompletedTask;
		}
	}
}
