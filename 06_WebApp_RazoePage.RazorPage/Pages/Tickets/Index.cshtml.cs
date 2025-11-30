using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Tickets;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tickets;
using AutoMapper;
using Microsoft.AspNetCore.Mvc; 

namespace _06_WebApp_RazoePage.RazorPage.Pages.Tickets
{
	public class IndexModel : BaseFetchPageModel<TicketListDtoModel, TIcketListViewModel>
	{
		public IndexModel(
			IHttpClientFactory httpClientFactory, 
			ILogger<BaseRazorPage<TicketListDtoModel, TIcketListViewModel>> logger, 
			IMapper mapper) 
			: base(httpClientFactory, "ticket", logger, mapper)
		{
		}

		public async Task OnGetAsync() 
			=> await GetAllEntitesByEndpointAsync("ticketlist", "بلیتی یافت نشد");
	}
}
