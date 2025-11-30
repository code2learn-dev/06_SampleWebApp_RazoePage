using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Movies
{
    public class IndexModel : BaseFetchPageModel<MovieItemDtoModel, MovieItemViewModel>
    {
		public IndexModel(
            IHttpClientFactory httpClientFactory,  
            ILogger<BaseRazorPage<MovieItemDtoModel, MovieItemViewModel>> logger, 
            IMapper mapper) 
            : base(httpClientFactory, "movie", logger, mapper)
		{
		}

        public async Task<IActionResult> OnGetAsync()
        {
            await GetAllEntityViewModelListAsync("دسته بندی جهت نمایش یافت نشد");
            if (EntityViewModelList is null) return RedirectToPage("/Error/Index"); 
			return Page();
        }

    }
}
