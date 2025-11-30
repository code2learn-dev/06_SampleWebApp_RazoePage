using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Movies
{
    public class StoreMovieModel : BaseMovieFetchPageModel
    {
		public StoreMovieModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<BaseRazorPage<MovieItemDtoModel, MovieItemViewModel>> logger, 
            IMapper mapper, 
            IMemoryCache memporyCache) 
            : base(httpClientFactory, logger, mapper, memporyCache)
		{
		}

		public async Task OnGetAsync(long? id)
        {
            await GetMovieProjectByIdAsync(id);
        }
    }
}
