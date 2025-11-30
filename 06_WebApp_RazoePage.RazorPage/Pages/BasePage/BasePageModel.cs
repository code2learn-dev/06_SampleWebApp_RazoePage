using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.ViewModels;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.BasePage
{
	public abstract class BasePageModel<
		TCrudEntityDtoModel,
		TCrudEntityViewModel,
		TEntityItemDtoModel,
		TEntityItemViewModel>

		: BaseRazorPage<TEntityItemDtoModel, TEntityItemViewModel>

		where TCrudEntityDtoModel : BaseDtoModel
		where TCrudEntityViewModel : BaseViewModel
		where TEntityItemDtoModel : BaseDtoModel
		where TEntityItemViewModel : BaseViewModel
	{  
		protected JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
		{
			ContractResolver = new PrivateSetterContractResolver(),
			TypeNameHandling = TypeNameHandling.Auto,
			TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
		};

		[BindProperty]
		public TCrudEntityViewModel CrudEntityViewModel { get; set; }

		protected BasePageModel(
			IHttpClientFactory httpClientFactory,
			string serviceName,
			ILogger<BaseRazorPage<TEntityItemDtoModel, TEntityItemViewModel>> logger,
			IMapper mapper )
			: base(httpClientFactory, serviceName, logger, mapper)
		{ 
		}

		public async Task<TEntityItemViewModel?> CreateEntityService(string errorMessage)
		{
			if (CrudEntityViewModel is null)
			{
				await SetMessage("داده ای یافت نشد");
				return default;
			}

			if (!ModelState.IsValid)
			{
				IReadOnlyList<string> modelErrors = ModelState.MapModelStateToErrorsList();
				modelErrors.MapToMessages(MessageStatus.danger);
				return default;
			}

			if (CrudEntityViewModel.Id > 0)
				CrudEntityViewModel.Id = 0;

			TCrudEntityDtoModel entityDtoModel = _mapper.Map<TCrudEntityDtoModel>(CrudEntityViewModel);
			HttpResponseMessage response = await _client.PostAsJsonAsync($"api/{_serviceName}", entityDtoModel);

			return await ProcessResponseAsync(response, errorMessage);
		}

		public async Task<TEntityItemViewModel?> EditEntityService(string errorMessage)
		{
			TCrudEntityDtoModel entityDtoModel = _mapper.Map<TCrudEntityDtoModel>(CrudEntityViewModel);
			HttpResponseMessage response = await _client.PutAsJsonAsync($"api/{_serviceName}", entityDtoModel);

			return await ProcessResponseAsync(response, errorMessage);
		}

		public async Task FindEntityToCrudAsync(long? id)
		{
			HttpResponseMessage response = await _client.GetAsync($"api/{_serviceName}/{id}");
			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<TCrudEntityDtoModel>(response);
				return;
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<
				ApplicationServiceResult<TCrudEntityDtoModel>>(contentResult, serializerSetting);
			if (appResult is null)
			{
				await SetMessage("دسته بندی یافت نشد", MessageStatus.danger);
				return;
			}

			if (!appResult.IsSuccess || appResult.Result is null)
			{
				if (appResult.Errors is not null && appResult.Errors.Any())
					appResult.Errors.MapToMessages(MessageStatus.danger);
				else
					await SetMessage("دسته بندی یافت نشد", MessageStatus.danger);

				return;
			}

			CrudEntityViewModel = _mapper.Map<TCrudEntityViewModel>(appResult.Result);
		} 
	}
}
