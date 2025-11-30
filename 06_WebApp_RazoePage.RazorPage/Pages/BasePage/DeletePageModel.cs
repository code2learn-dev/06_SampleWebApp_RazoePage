using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.ViewModels;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.BasePage
{
	public abstract class DeletePageModel<
		TEntityItemDtoModel,
		TEntityItemViewModel,
		TDeleteEntityDtoModel,
		TDeleteEntityViewModel>

		: BaseRazorPage<TEntityItemDtoModel, TEntityItemViewModel>

		where TEntityItemDtoModel : BaseDtoModel
		where TEntityItemViewModel : BaseViewModel
		where TDeleteEntityDtoModel : BaseDtoModel
		where TDeleteEntityViewModel : BaseViewModel
	{
		[BindProperty]
		public TDeleteEntityViewModel DeleteEntityViewModel { get; set; }

		protected DeletePageModel(
			IHttpClientFactory httpClientFactory,  
			string serviceName, 
			ILogger<BaseRazorPage<TEntityItemDtoModel, TEntityItemViewModel>> logger, 
			IMapper mapper) 
			: base(httpClientFactory, serviceName, logger, mapper)
		{
		}

		public virtual async Task<TEntityItemViewModel?> DeleteEntityService(string errorMessage)
		{
			if (DeleteEntityViewModel is null) return null;

			HttpResponseMessage response = await _client.DeleteAsync($"api/{_serviceName}/{DeleteEntityViewModel.Id}");

			return await ProcessResponseAsync(response, errorMessage);
		}

		public virtual async Task FindEntityToDeleteAsync(
			long? id, 
			string errorMessage = "داده ای جهت حذف یافت نشد")
		{
			HttpResponseMessage response = await _client.GetAsync($"api/{_serviceName}/delete/{id}");
			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<TDeleteEntityDtoModel>(response);
				return;
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<
				ApplicationServiceResult<TDeleteEntityDtoModel>>(contentResult, serializerSetting);
			if (appResult is null)
			{
				await SetMessage(errorMessage, MessageStatus.danger);
				return;
			}

			if (!appResult.IsSuccess || appResult.Result is null)
			{
				if (appResult.Errors is not null && appResult.Errors.Any())
					appResult.Errors.MapToMessages(MessageStatus.danger);
				else
					await SetMessage(errorMessage, MessageStatus.danger);

				return;
			}

			DeleteEntityViewModel = _mapper.Map<TDeleteEntityViewModel>(appResult.Result);
		}
	}
}
