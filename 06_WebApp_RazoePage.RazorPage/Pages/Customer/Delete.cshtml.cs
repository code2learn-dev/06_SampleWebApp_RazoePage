using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Customers;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Customers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Customer
{
    public class DeleteModel : DeletePageModel<
        CustomerItemDtoModel,
        CustomerItemViewModel,
        DeleteCustomerDtoModel,
        DeleteCustomerViewModel>
    {
		public DeleteModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<BaseRazorPage<CustomerItemDtoModel, CustomerItemViewModel>> logger, 
            IMapper mapper) 
            : base(httpClientFactory, "customer", logger, mapper)
		{
		}

		protected string ProfileImageUri => "https://localhost:7249";

		public async Task<IActionResult> OnGetAsync(long? id)
        {
            await FindEntityToDeleteAsync(id, "مشتری یافت نشد");

            if (DeleteEntityViewModel is null)
                return RedirectToPage(IndexPage);

            if (!string.IsNullOrEmpty(DeleteEntityViewModel.ProfileImage))
                TempData["ProfileImage"] = $"{ProfileImageUri}/images/customers/{DeleteEntityViewModel.ProfileImage}";
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
			CustomerItemViewModel? deletedCustomer = await DeleteEntityService("خطا در حذف مشتری");
           
            return deletedCustomer is not null ? RedirectToPage(IndexPage) : Page();
        } 
    }
}
