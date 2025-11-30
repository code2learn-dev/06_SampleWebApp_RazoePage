using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Genres;
using _06_WebApp_RazoePage.WebApi.DtoModels.Genres;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Genre
{
    public class IndexModel : BaseFetchPageModel<
        GenreDtoModel,
        GenreItemViewModel>
    {
		public IndexModel(
            IHttpClientFactory httpClientFactory,  
            ILogger<BaseRazorPage<GenreDtoModel, GenreItemViewModel>> logger, 
            IMapper mapper) 
            : base(httpClientFactory, "genre", logger, mapper)
		{
		}

		public async Task<IActionResult> OnGetAsync(string? format)
        {
            await GetAllByFormatFilterAsync(format, "دسته بندی جهت نمایش یافت نشد");
            //await GetAllInXmlFormatAsync("دسته بندی جهت نمایش یافت نشد");
            //await GetAllEntityViewModelListAsync("دسته بندی جهت نمایش یافت نشد");
            if (EntityViewModelList is null) return RedirectToPage("/Error/Index");
            return Page();
        }

    }
}
