using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Genres;
using _06_WebApp_RazoePage.WebApi.DtoModels.Genres;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Genre
{
    public class DeleteModel : DeletePageModel<
		GenreDtoModel,
		GenreItemViewModel,
		GenreDtoModel,
		GenreItemViewModel>
	{
		public DeleteModel(
			IHttpClientFactory httpClientFactory,  
			ILogger<BaseRazorPage<GenreDtoModel, GenreItemViewModel>> logger, 
			IMapper mapper) 
			: base(httpClientFactory, "genre", logger, mapper)
		{
		}

		public async Task<IActionResult> OnGetAsync(long? id)
        {
			await FindEntityToDeleteAsync(id, "دسته بندی یافت نشد");
            return DeleteEntityViewModel is not null ? Page() : RedirectToPage("/Error/Index");
        }


		public async Task<IActionResult> OnPostAsync()
		{
			GenreItemViewModel? genreViewModel = await DeleteEntityService("خطا در حذف دسته بندی فیلم");
			return genreViewModel is not null ? RedirectToPage("Index") : Page();
		}

	}
}
