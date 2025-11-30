using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Customers;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Customers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Customer
{
	public class BaseCustomerFetchPageModel : BaseFetchPageModel<
		CustomerItemDtoModel,
		CustomerItemViewModel>
	{
		protected readonly IMemoryCache _memoryCache;

		public BaseCustomerFetchPageModel(
			IHttpClientFactory httpClientFactory,
			ILogger<BaseRazorPage<CustomerItemDtoModel, CustomerItemViewModel>> logger,
			IMapper mapper,
			IMemoryCache memoryCache)
			: base(httpClientFactory, "customer", logger, mapper)
		{
			_memoryCache = memoryCache;
		}

		protected virtual async Task GetAllCustomersAsync()
		{
			await GetAllEntityViewModelListAsync("مشتری یافت نشد");
		}

		protected virtual async Task<IEnumerable<CustomerProjectViewModel>?> GetCustomerProjectListAsync()
		{
			HttpResponseMessage response = await _client.GetAsync("api/customer/projectlist");
			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<CustomerItemDtoModel>(response);
				return null;
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<
				ApplicationServiceResult<IEnumerable<CustomerProjectDtoModel>>>(contentResult);

			if (appResult is null)
			{
				await SetMessage("مشتری یافت نشد", Extensions.MessageStatus.danger);
				return null;
			}

			if (appResult.IsSuccess)
			{
				var customerProjectViewModelList = _mapper.Map<IEnumerable<CustomerProjectViewModel>>(appResult.Result).ToList();
				return customerProjectViewModelList;
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
			return null;
		} 
	}
}
