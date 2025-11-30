using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.ViewComponents
{
	public class MovieSidebarViewComponent : ViewComponent
	{
		private readonly IBaseViewComponent _baseViewComponent;

		private readonly string _movieBaseUri;

		public MovieSidebarViewComponent(IBaseViewComponent baseViewComponent)
		{
			_baseViewComponent = baseViewComponent;
			_movieBaseUri = Path.Combine("https://localhost:7249", "images", "movies");
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			IEnumerable<MovieProjectViewModel> latestMoviesList = new List<MovieProjectViewModel>();

			HttpResponseMessage response = await _baseViewComponent.Client.GetAsync("api/movie/sidebarlist");
			if (!response.IsSuccessStatusCode) { 
				await _baseViewComponent.GetResponseErrorMessages<MovieItemDtoModel>(response);
				return View(latestMoviesList);
			} 

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<
				ApplicationServiceResult<IEnumerable<MovieProjectDtoModel>>>(contentResult);
			if(appResult is null)
			{
				await _baseViewComponent.SetMessage("امکان نمایش جدیدترین فیلم ها وچود ندارد", Extensions.MessageStatus.danger);
				return View(latestMoviesList);
			}

			if(appResult.IsSuccess)
			{
				latestMoviesList = _baseViewComponent.Mapper.Map<IEnumerable<MovieProjectViewModel>>(appResult.Result);
				TempData["BaseCoverImageUri"] = _movieBaseUri;
				return View(latestMoviesList);
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
			return View(latestMoviesList);
		}
	}
}
