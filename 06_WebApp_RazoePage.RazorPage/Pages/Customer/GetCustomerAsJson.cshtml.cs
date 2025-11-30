using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Customers;
using _06_WebApp_RazoePage.WebApi.DtoModels.Customers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc; 

namespace _06_WebApp_RazoePage.RazorPage.Pages.Customer
{
    public class GetCustomerAsJsonModel : BaseFetchPageModel<
        CustomerProjectDtoModel,
        CustomerProjectViewModel>
    {
		public GetCustomerAsJsonModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<BaseRazorPage<CustomerProjectDtoModel, CustomerProjectViewModel>> logger,
            IMapper mapper) 
            : base(httpClientFactory, "customer", logger, mapper)
		{
		}

        public async Task<JsonResult> OnGetAsync(long? id)
        {
			CustomerProjectViewModel? customerProjectViewModel = await GetEntityViewModelByIdAsync(id, "getproject", "مشتری یافت نشد");

            return customerProjectViewModel != null
                ? new JsonResult(customerProjectViewModel)
                : new JsonResult(string.Empty);

        }

    }
}
