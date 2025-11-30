using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Customers;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Customers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Customer
{
    public class CustomerListAsJsonModel : BaseCustomerFetchPageModel
    {
		public CustomerListAsJsonModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<BaseRazorPage<CustomerItemDtoModel, CustomerItemViewModel>> logger, 
            IMapper mapper,
            IMemoryCache memoryCache) 
            : base(httpClientFactory, logger, mapper, memoryCache)
		{
		}

        public async Task<JsonResult> OnGetAsync()
        {
			IEnumerable<CustomerProjectViewModel>? customerProjectionList = await GetCustomerProjectListAsync();

            if (customerProjectionList != null)
                return new JsonResult(customerProjectionList);
            else
                return new JsonResult(Array.Empty<string>());
        }

    }
}
