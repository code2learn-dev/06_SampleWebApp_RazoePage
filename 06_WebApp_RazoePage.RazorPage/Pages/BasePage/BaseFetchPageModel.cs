using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.ViewModels;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Tickets;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels;
using AutoMapper;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace _06_WebApp_RazoePage.RazorPage.Pages.BasePage
{
	public abstract class BaseFetchPageModel<TEntityItemDtoModel, TEntityItemViewModel>

		: BaseRazorPage<TEntityItemDtoModel, TEntityItemViewModel>

		where TEntityItemDtoModel : BaseDtoModel
		where TEntityItemViewModel : BaseViewModel
	{
		protected BaseFetchPageModel(
			IHttpClientFactory httpClientFactory,
			string serviceName,
			ILogger<BaseRazorPage<TEntityItemDtoModel, TEntityItemViewModel>> logger,
			IMapper mapper)
			: base(httpClientFactory, serviceName, logger, mapper)
		{
		}

		public IReadOnlyList<TEntityItemViewModel>? EntityViewModelList { get; set; }

		public virtual async Task GetAllEntityViewModelListAsync(
			string errorMessage = "داده ای یافت نشد")
		{
			HttpResponseMessage response = await _client.GetAsync($"api/{_serviceName}");
			if (!response.IsSuccessStatusCode)
			{
				await SetMessage(errorMessage, MessageStatus.danger);
				return;
			}

			string resultContent = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<ApplicationServiceResult<
				IEnumerable<TEntityItemDtoModel>>>(resultContent, serializerSetting);

			if (appResult is null)
			{
				await SetMessage(errorMessage, MessageStatus.danger);
				return;
			}

			if (appResult.IsSuccess)
			{
				IEnumerable<TEntityItemDtoModel> entityListResult = appResult.Result ?? [];
				EntityViewModelList = _mapper.Map<IEnumerable<TEntityItemViewModel>>(entityListResult).ToList();
				return;
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
		}

		public virtual async Task GetAllEntitesByEndpointAsync(
			string endpoint,
			string errorMessage = "داده ای یافت نشد")
		{
			HttpResponseMessage response = await _client.GetAsync($"api/{_serviceName}/{endpoint}");
			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<TEntityItemDtoModel>(response);
				return;
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<
				ApplicationServiceResult<IEnumerable<TEntityItemDtoModel>>>(contentResult);

			if (appResult is null)
			{
				await SetMessage(errorMessage, MessageStatus.danger);
				return;
			}

			if (appResult.IsSuccess)
			{
				EntityViewModelList = _mapper.Map<IEnumerable<TEntityItemViewModel>>(appResult.Result).ToList();
				return;
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
		}

		public virtual async Task GetAllInXmlFormatAsync(string errorMessage = "داده ای یافت نشد")
		{
			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Xml));
			HttpResponseMessage response = await _client.GetAsync($"api/{_serviceName}");
			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<TEntityItemDtoModel>(response);
				return;
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var serialize = new DataContractSerializer(typeof(ApplicationServiceResult<IEnumerable<TEntityItemDtoModel>>));
			await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(contentResult));
			var dtoModelObject = serialize.ReadObject(stream);
			var appResult = dtoModelObject as ApplicationServiceResult<IEnumerable<TEntityItemDtoModel>>;

			if (appResult is null)
			{
				await SetMessage(errorMessage, MessageStatus.danger);
				return;
			}

			if (appResult.IsSuccess)
			{
				EntityViewModelList = _mapper.Map<IEnumerable<TEntityItemViewModel>>(appResult.Result).ToList();
				return;
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
		}

		public virtual async Task GetAllByFormatFilterAsync(
			string? format = "json", 
			string errormessage = "داده اِی یافت نشد")
		{
			RequestFormat requestFormat = (RequestFormat)(Enum.TryParse(typeof(RequestFormat), format, true, out object? formatResult) ? formatResult : RequestFormat.json);

			HttpResponseMessage response = await _client.GetAsync($"api/genre/all/{requestFormat}");
			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<TEntityItemDtoModel>(response);
				return;
			}

			var appResult = new ApplicationServiceResult<IEnumerable<TEntityItemDtoModel>>();
			string contentResult = await response.Content.ReadAsStringAsync();
			if (requestFormat is RequestFormat.xml)
			{
				var seriliser = new DataContractSerializer(typeof(ApplicationServiceResult<IEnumerable<TEntityItemDtoModel>>));
				await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(contentResult));
				var dtoModelObject = seriliser.ReadObject(stream);
				appResult = dtoModelObject as ApplicationServiceResult<IEnumerable<TEntityItemDtoModel>>;
			}
			else
			{
				appResult = JsonConvert.DeserializeObject<ApplicationServiceResult<IEnumerable<TEntityItemDtoModel>>>(contentResult);
			}

			if (appResult is null)
			{
				await SetMessage(errormessage, MessageStatus.danger);
				return;
			}

			if (appResult.IsSuccess)
			{
				EntityViewModelList = _mapper.Map<IEnumerable<TEntityItemViewModel>>(appResult.Result).ToList();
				return;
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
		}


		public virtual async Task<TEntityItemViewModel?> GetEntityViewModelByIdAsync
			(long? id, 
			string endpoint, 
			string errorMessage = "داده ای یافت نشد") 
		{
			string apiUri = !string.IsNullOrEmpty(endpoint)
				? $"api/{_serviceName}/{endpoint}/{id}"
				: $"api/{_serviceName}/{id}";
			HttpResponseMessage response = await _client.GetAsync(apiUri);

			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<TEntityItemDtoModel>(response);
				return null;
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<ApplicationServiceResult<TEntityItemDtoModel>>(contentResult);

			if (appResult is null)
			{
				await SetMessage(errorMessage, MessageStatus.danger);
				return null;
			}

			if (appResult.IsSuccess && appResult.Result != null)
			{
				var itemViewModel = _mapper.Map<TEntityItemViewModel>(appResult.Result);
				return itemViewModel;
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
			return null;
		}
	}

	public enum RequestFormat : byte
	{
		json = 1,
		xml
	}
}
