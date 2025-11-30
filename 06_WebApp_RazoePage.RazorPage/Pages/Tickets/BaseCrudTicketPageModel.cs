using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.Pages.Customer;
using _06_WebApp_RazoePage.RazorPage.ViewModels;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Customers;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Tickets;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels;
using _06_WebApp_RazoePage.WebApi.DtoModels.Customers;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tickets;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Tickets
{
	public class BaseCrudTicketPageModel : BasePageModel<
		CrudTicketDtoItem,
		CrudTicketViewModel,
		TicketItemDtoModel,
		TicketItemViewModel>
	{
		protected readonly IMemoryCache _memoryCache; 

		public BaseCrudTicketPageModel(
			IHttpClientFactory httpClientFactory, 
			ILogger<BaseRazorPage<TicketItemDtoModel, TicketItemViewModel>> logger, 
			IMapper mapper,
			IMemoryCache memoryCache) 
			: base(httpClientFactory, "ticket", logger, mapper)
		{
			_memoryCache = memoryCache; 
		} 

		protected async Task<CustomerProjectViewModel?> GetTicketCustomerByIdAsync(long? id)
		{
			HttpResponseMessage response = await _client.GetAsync($"api/customer/getproject/{id}");
			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<CustomerItemDtoModel>(response);
				return null;
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<ApplicationServiceResult
				<CustomerProjectDtoModel>>(contentResult);

			if(appResult is null)
			{
				await SetMessage("مشتری یافت نشد", Extensions.MessageStatus.danger);
				return null;
			}

			if(appResult.IsSuccess && appResult.Result != null)
			{
				var customerProjectViewModel = _mapper.Map<CustomerProjectViewModel>(appResult.Result);
				return customerProjectViewModel;
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
			return null;
		}

		protected virtual async Task<IActionResult> GetCrudTicketActionResultAsync(
			long? id, string errorMessage = "داده ای یافت نشد")
		{
			HttpResponseMessage response = await _client.GetAsync($"api/ticket/find/{id}");
			var crudTicketViewModel = await ProcessHttpResponse<TicketProjectionDtoModel, CrudTicketViewModel>(response, errorMessage);

			if (crudTicketViewModel != null)
			{
				CrudEntityViewModel = crudTicketViewModel;
				return Page();
			}

			return RedirectToPage(IndexPage);
		} 

		public async Task<IActionResult> EditTicketActionResultAsync()
		{
			if(CrudEntityViewModel is null)
			{
				await SetMessage("خطا در ویرایش بلیت", MessageStatus.danger);
				_logger.LogError("CrudEntityViewModel is null");
				return Page();
			}

			if(!ModelState.IsValid)
			{
				IReadOnlyList<string> modelErrors = ModelState.MapModelStateToErrorsList();
				modelErrors.MapToMessages(MessageStatus.danger);
				return Page();
			}

			TicketItemViewModel? editedTicket = await EditEntityService("خطا در ویرایش اطلاعات بلیت");

			return editedTicket != null
				? RedirectToPage(IndexPage)
				: Page();
		}

		protected async Task<TTicketViewModel?> ProcessHttpResponse<TTicketDtoModel, TTicketViewModel>(HttpResponseMessage response, string errorMessage)
			where TTicketDtoModel : BaseDtoModel
			where TTicketViewModel : BaseViewModel
		{
			if(!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<TTicketDtoModel>(response);
				return null;
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult =  JsonConvert.DeserializeObject<
				ApplicationServiceResult<TTicketDtoModel>>(contentResult, serializerSetting);

			if(appResult is null)
			{
				await SetMessage(errorMessage, MessageStatus.danger);
				return null;
			}

			if(appResult.IsSuccess && appResult.Result != null)
			{
				var ticketViewModel = _mapper.Map<TTicketViewModel>(appResult.Result);
				return ticketViewModel;
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
			return null;
		}
	}
}
