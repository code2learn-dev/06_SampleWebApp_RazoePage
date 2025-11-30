using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Genres;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Genres;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Movies
{
    public class DeleteModel : DeletePageModel<
        MovieItemDtoModel,
        MovieItemViewModel,
        DeleteMovieDtoModel,
        DeleteMovieViewModel>
    {
		public DeleteModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<BaseRazorPage<MovieItemDtoModel, MovieItemViewModel>> logger, 
            IMapper mapper) : 
            base(httpClientFactory, "movie", logger, mapper)
		{
		}

		protected string MovieCoverImageUri => "https://localhost:7249";

		public async Task<IActionResult> OnGetAsync(long? id)
		{
			await FindEntityToDeleteAsync(id, "فایلی یافت نشد");

			if (DeleteEntityViewModel is null) return RedirectToPage(IndexPage);

			List<SelectListItem>? genreListModel = await GetGenreSelectListAsync();
			DeleteEntityViewModel.GenreSelectList = genreListModel;

			if (!string.IsNullOrEmpty(DeleteEntityViewModel.ImageName))
				TempData["Image"] = $"{MovieCoverImageUri}/images/movies/{DeleteEntityViewModel.ImageName}";

			if (!string.IsNullOrEmpty(DeleteEntityViewModel.TagsList))
				Response.Cookies.Append("tags", DeleteEntityViewModel.TagsList);

			return Page();
		}


		public async Task<IActionResult> OnPostAsync()
		{
			MovieItemViewModel? movieItemViewModel = await DeleteEntityService("خطا در حذف فیلم");
			return movieItemViewModel is not null ? RedirectToPage(IndexPage) : Page();
		}

		private async Task<List<SelectListItem>?> GetGenreSelectListAsync()
		{
			HttpResponseMessage response = await _client.GetAsync("api/genre");
			if (!response.IsSuccessStatusCode)
			{
				await SetMessage("دسته بندی فیلم ها یافت نشد", Extensions.MessageStatus.danger);
				return null;
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<
				ApplicationServiceResult<IEnumerable<GenreDtoModel>?>>(contentResult);
			if (appResult is null)
			{
				await SetMessage("دسته بندی فیلم ها یافت نشد", Extensions.MessageStatus.danger);
				return null;
			}

			if (appResult.IsSuccess)
			{
				IEnumerable<GenreDtoModel> genreDtoModelList = appResult.Result ?? [];
				var genreViewModelList = _mapper.Map<IEnumerable<GenreItemViewModel>>(genreDtoModelList);
				return genreViewModelList.Select(a => new SelectListItem(a.Title, a.Id.ToString())).ToList();
			}

			await SetMessage("دسته بندی فیلم ها یافت نشد", Extensions.MessageStatus.danger);
			return null;
		}
	}
}
