using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Movies
{
	public class EditModel : BaseMovieCrudPageModel
	{
		public EditModel(
			IHttpClientFactory httpClientFactory,
			ILogger<BaseRazorPage<MovieItemDtoModel, MovieItemViewModel>> logger,
			IMapper mapper)
			: base(httpClientFactory, logger, mapper)
		{
		}

		public async Task<IActionResult> OnGetAsync(long? id)
		{
			HttpResponseMessage response = await _client.GetAsync($"api/movie/findmovie/{id}");
			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<CrudMovieDtoModel>(response);
				return RedirectToPage(IndexPage);
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<ApplicationServiceResult<CrudMovieDtoModel>>(contentResult);
			if (appResult is null)
			{
				await SetMessage("فیلمی یافت نشد", MessageStatus.danger);
				return RedirectToPage(IndexPage);
			}

			if (appResult.IsSuccess && appResult.Result is not null)
			{
				CrudMovieDtoModel movieDtoModel = appResult.Result;
				CrudEntityViewModel = _mapper.Map<CrudMovieViewModel>(movieDtoModel);
				List<SelectListItem>? genreList = await GetGenreSelectListAsync();
				CrudEntityViewModel.GenreSelectList = genreList; 

				if (!string.IsNullOrEmpty(CrudEntityViewModel.ImageName))
					TempData["Image"] = $"{MovieCoverImageUri}/images/movies/{CrudEntityViewModel.ImageName}";

				if (!string.IsNullOrEmpty(CrudEntityViewModel.TagsList))
					Response.Cookies.Append("tags", CrudEntityViewModel.TagsList);

				return Page();
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
			return RedirectToPage(IndexPage);
		}


		public async Task<IActionResult> OnPostAsync()
		{
			if (Request.Cookies.ContainsKey("tags"))
				CrudEntityViewModel.TagsList = Request.Cookies["tags"]?.ToString() ?? string.Empty;
			return await CrudMovieAsync(WebApi.Common.ModelState.update);
		}

	}
}
