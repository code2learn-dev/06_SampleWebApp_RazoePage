using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Genres;
using _06_WebApp_RazoePage.WebApi.DtoModels.Genres;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Genre
{
    public class AddModel : BasePageModel<
		CrudGenreDtoModel,
		CrudGenreViewModel,
		GenreDtoModel,
		GenreItemViewModel>
	{
		public AddModel(
			IHttpClientFactory httpClientFactory, 
			ILogger<BaseRazorPage<GenreDtoModel, GenreItemViewModel>> logger,
			IMapper mapper)
			: base(httpClientFactory, "genre", logger, mapper)
		{
		}

		public IActionResult OnGetAsync()
        {
            CrudEntityViewModel = new CrudGenreViewModel();
            return Page();
        } 

        public async Task<IActionResult> OnPostAsync()
        {
			if (!ModelState.IsValid)
			{
				IReadOnlyList<string> modelErrors = ModelState.MapModelStateToErrorsList();
				modelErrors.MapToMessages(MessageStatus.danger);
				return RedirectToPage(nameof(IndexPage));
			}

			GenreItemViewModel? createdGenre = await CreateEntityService("خطا در تعریف دسته بندی جدید");
			return createdGenre is not null ? RedirectToPage("Index") : Page();
		}

	}
}
