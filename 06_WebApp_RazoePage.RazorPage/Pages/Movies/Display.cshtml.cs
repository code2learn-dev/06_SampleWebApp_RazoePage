using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Movies
{
	public class DisplayModel : DeletePageModel<
		MovieItemDtoModel,
		MovieItemViewModel,
		DeleteMovieDtoModel,
		DeleteMovieViewModel>
	{
		protected string MovieCoverImageUri => "https://localhost:7249";

		public DisplayModel(
			IHttpClientFactory httpClientFactory, 
			ILogger<BaseRazorPage<MovieItemDtoModel, MovieItemViewModel>> logger, 
			IMapper mapper) 
			: base(httpClientFactory, "movie", logger, mapper)
		{
		}


		public async Task<IActionResult> OnGetAsync(long? id)
		{
			await FindEntityToDeleteAsync(id, "فیلمی یافت نشد");

			if (DeleteEntityViewModel is null) return RedirectToPage(IndexPage);

			if (!string.IsNullOrEmpty(DeleteEntityViewModel.ImageName))
				TempData["Image"] = $"{MovieCoverImageUri}/images/movies/{DeleteEntityViewModel.ImageName}";

			return DeleteEntityViewModel is not null ? Page() : RedirectToPage(IndexPage);
		}

	}
}
