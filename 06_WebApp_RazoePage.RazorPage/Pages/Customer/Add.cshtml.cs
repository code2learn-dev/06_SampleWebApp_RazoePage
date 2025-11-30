using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Customers;
using _06_WebApp_RazoePage.WebApi.DtoModels.Customers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Customer
{
    public class AddModel : BaseCrudCustomerPageModel
    {
		public AddModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<BaseRazorPage<CustomerItemDtoModel, CustomerItemViewModel>> logger, 
            IMapper mapper) 
            : base(httpClientFactory, logger, mapper)
		{
		}

		public void OnGet()
        {
            CrudEntityViewModel = new CrudCustomerViewModel();
        } 

		public async Task<IActionResult> OnPostAsync() => await CrudCustomer();

	}
}
