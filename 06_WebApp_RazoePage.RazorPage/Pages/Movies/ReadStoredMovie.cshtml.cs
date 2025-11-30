using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Movies
{
    public class ReadStoredMovieModel : BaseMovieFetchPageModel
    {
		public ReadStoredMovieModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<BaseRazorPage<MovieItemDtoModel, MovieItemViewModel>> logger, 
            IMapper mapper, 
            IMemoryCache memporyCache) 
            : base(httpClientFactory, logger, mapper, memporyCache)
		{
		}

		public JsonResult OnGet(long? id)
        {
			var movieProjectViewModel = _memoryCache.GetMemoryCacheValue<MovieProjectViewModel>("SelectedMovie");

			return movieProjectViewModel != null ? new JsonResult(movieProjectViewModel) : new JsonResult(Array.Empty<string>());
		}
    }
}
