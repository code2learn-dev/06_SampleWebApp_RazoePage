using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Movies
{
	public abstract class BaseMovieFetchPageModel : BaseFetchPageModel<
		MovieItemDtoModel,
		MovieItemViewModel>
	{
		protected readonly IMemoryCache _memoryCache;

		protected BaseMovieFetchPageModel(
			IHttpClientFactory httpClientFactory,
			ILogger<BaseRazorPage<MovieItemDtoModel, MovieItemViewModel>> logger,
			IMapper mapper,
			IMemoryCache memporyCache)
			: base(httpClientFactory, "movie", logger, mapper)
		{
			_memoryCache = memporyCache;
		}

		public IEnumerable<MovieItemWithGenreTitleViewModel> MovieThumbnailItemList { get; private set; }

		protected virtual async Task<IActionResult> GetMoviesItemListAsync(string errorMessage = "داده ای یافت نشد")
		{
			HttpResponseMessage response = await _client.GetAsync("api/movie/thumbnail/list");
			if (!response.IsSuccessStatusCode)
			{
				await SetMessage(errorMessage, MessageStatus.danger);
				return RedirectToPage("/Error/Index");
			}

			string contentListResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<ApplicationServiceResult<
				IEnumerable<MovieItemWithGenreTitleDtoModel>>>(contentListResult);
			if (appResult is null)
			{
				await SetMessage(errorMessage, MessageStatus.danger);
				return RedirectToPage("/Error/Index");
			}

			MovieThumbnailItemList = _mapper.Map<IEnumerable<MovieItemWithGenreTitleViewModel>>(appResult.Result);
			return Page();
		}

		protected virtual async Task GetMovieProjectByIdAsync(long? id)
		{
			HttpResponseMessage response = await _client.GetAsync($"api/movie/projectmovie/{id}");
			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<MovieProjectDtoModel>(response);
				return;
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<
				ApplicationServiceResult<MovieProjectDtoModel>>(contentResult);

			if(appResult is null)
			{
				await SetMessage("فیلمی یافت نشد", MessageStatus.danger);
				return;
			}

			if(appResult.IsSuccess && appResult.Result is not null)
			{
				var movieProjectViewModel = _mapper.Map<MovieProjectViewModel>(appResult.Result);
				_memoryCache.SetMemoryCache("SelectedMovie", movieProjectViewModel);
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
		}
	}
}
