using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Tickets;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tickets;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Tickets
{
	public class DeleteModel : DeletePageModel<
		TicketItemDtoModel,
		TicketItemViewModel,
		DeleteTicketDtoItem,
		DeleteTicketViewModel>
	{
		public DeleteModel(
			IHttpClientFactory httpClientFactory,
			ILogger<BaseRazorPage<TicketItemDtoModel, TicketItemViewModel>> logger,
			IMapper mapper)
			: base(httpClientFactory, "ticket", logger, mapper)
		{
		}

		public async Task OnGetAsync(long? id)
		{
			await FindEntityToDeleteAsync(id, "بلیتی جهت حذف یافت نشد");
		}

		public async Task<IActionResult> OnPostAsync()
		{
			TicketItemViewModel? ticketItemViewModel = await DeleteEntityService("خطا در حذف بلیا");

			return ticketItemViewModel != null
				? RedirectToPage(IndexPage)
				: Page();
		}

		public override async Task FindEntityToDeleteAsync(long? id, string errorMessage = "داده ای جهت حذف یافت نشد")
		{
			HttpResponseMessage response = await _client.GetAsync($"api/ticket/find/{id}");
			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<TicketProjectionDtoModel>(response);
				return;
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<
				ApplicationServiceResult<TicketProjectionDtoModel>>(contentResult);

			if (appResult is null)
			{
				await SetMessage(errorMessage, Extensions.MessageStatus.danger);
				return;
			}

			if (appResult.IsSuccess && appResult.Result != null)
				DeleteEntityViewModel = _mapper.Map<DeleteTicketViewModel>(appResult.Result);
			else
				appResult.Errors.MapToMessages(MessageStatus.danger);
		}
	}
}
