using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Customers;
using _06_WebApp_RazoePage.WebApi.DtoModels.Customers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc; 

namespace _06_WebApp_RazoePage.RazorPage.Pages.Customer
{
    public class IndexModel : BaseFetchPageModel<CustomerItemDtoModel, CustomerItemViewModel>
    {
		public IndexModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<BaseRazorPage<CustomerItemDtoModel, CustomerItemViewModel>> logger, 
            IMapper mapper) 
            : base(httpClientFactory, "customer", logger, mapper)
		{
		}

        public async Task<IActionResult> OnGetAsync()
        {
            await GetAllEntityViewModelListAsync();
            return EntityViewModelList is not null ? Page() : RedirectToPage(ErrorPage);
        }

    }
}
