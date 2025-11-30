using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Taggs;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Movies
{
	public class AddModel : BaseMovieCrudPageModel
	{
		public AddModel(
			IHttpClientFactory httpClientFactory,
			ILogger<BaseRazorPage<MovieItemDtoModel, MovieItemViewModel>> logger,
			IMapper mapper)
			: base(httpClientFactory, logger, mapper)
		{
		}

		public async Task OnGet()
		{
			CrudEntityViewModel = new CrudMovieViewModel();
			List<SelectListItem>? genreSelectList = await GetGenreSelectListAsync();
			CrudEntityViewModel.GenreSelectList = genreSelectList;
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (Request.Cookies.ContainsKey("tags"))
			{
				CrudEntityViewModel.TagsList = Request.Cookies["tags"]?.ToString() ?? string.Empty; 
			}
			return await CrudMovieAsync(WebApi.Common.ModelState.create);
		}
	}
}
