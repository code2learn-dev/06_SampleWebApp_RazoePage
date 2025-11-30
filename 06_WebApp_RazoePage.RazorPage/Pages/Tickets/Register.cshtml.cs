using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Customers;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Tickets;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tickets;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Tickets
{
    public class RegisterModel : BaseCrudTicketPageModel
    {
        private readonly IMemoryCache _memoryCache;

		public RegisterModel(
			IHttpClientFactory httpClientFactory,
			ILogger<BaseRazorPage<TicketItemDtoModel, TicketItemViewModel>> logger,
			IMapper mapper,
			IMemoryCache memoryCache)
			: base(httpClientFactory, logger, mapper, memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public async Task<IActionResult> OnGetAsync(long? id)
		{ 
			CustomerProjectViewModel? customerProjectViewModel = await GetTicketCustomerByIdAsync(id);
			if (customerProjectViewModel is null) return Partial("_ErrorNotFound", "مشتری یافت نشد");

			MovieProjectViewModel? movieProjectViewModel = _memoryCache.GetMemoryCacheValue<MovieProjectViewModel>("SelectedMovie");

			if(movieProjectViewModel is null)
				return Partial("_ErrorNotFound", "مشتری یافت نشد");

			CrudEntityViewModel = new CrudTicketViewModel()
			{ 
				CustomerId = customerProjectViewModel.Id,
				CustomerName = $"{customerProjectViewModel.FirstName}" +
				$"{customerProjectViewModel.LastName}",

				NationalCode = customerProjectViewModel.NationalCode,
				MovieId = movieProjectViewModel.Id,
				RegisterDate = DateTime.Now.Date.MapToPersianDate()
			};

			return Partial("_RegisterTicket", CrudEntityViewModel);
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if(!ModelState.IsValid)
			{
				IReadOnlyList<string> modelErrors = ModelState.MapModelStateToErrorsList();
				modelErrors.MapToMessages(MessageStatus.danger);
				return RedirectToPage("Add");
			}

			TicketItemViewModel? ticketItemViewModel = await CreateEntityService("خطا در ثبت بلیت");
			if(ticketItemViewModel != null)
			{
				MovieProjectViewModel? movieProject = _memoryCache.GetMemoryCacheValue<MovieProjectViewModel>("SelectedMovie");
				if (movieProject is not null)
					_memoryCache.Remove("SelectedMovie");

				return RedirectToPage(IndexPage);
			}

			return RedirectToPage("Add");
		}

	}
}
