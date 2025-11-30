using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Tickets;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tickets;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Tickets
{
    public class EditModel : BaseCrudTicketPageModel
    {
		public EditModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<BaseRazorPage<TicketItemDtoModel, TicketItemViewModel>> logger, 
            IMapper mapper, 
            IMemoryCache memoryCache) 
            : base(httpClientFactory, logger, mapper, memoryCache)
		{
		}

        public async Task<IActionResult> OnGetAsync(long? id)
            => await GetCrudTicketActionResultAsync(id, "یلیتی یافت نشد");

        public async Task<IActionResult> OnPostAsync()
            => await EditTicketActionResultAsync();

    }
}
