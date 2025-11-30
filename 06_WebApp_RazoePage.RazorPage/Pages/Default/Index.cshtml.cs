using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.Pages.Movies;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Default
{
	public class IndexModel : BaseMovieFetchPageModel
	{
		public IndexModel(
			IHttpClientFactory httpClientFactory, 
			ILogger<BaseRazorPage<MovieItemDtoModel, MovieItemViewModel>> logger, 
			IMapper mapper,
			IMemoryCache memoryCache) 
			: base(httpClientFactory, logger, mapper, memoryCache)
		{
		}

		public string MovieCoverImageUri => "https://localhost:7249";

		public async Task<IActionResult> OnGetAsync()
		{
			await GetMoviesItemListAsync("فیلمی ثبت نشده است");
			return MovieThumbnailItemList is not null ? Page() : RedirectToPage("/Error/Index");
		}

	}
}
