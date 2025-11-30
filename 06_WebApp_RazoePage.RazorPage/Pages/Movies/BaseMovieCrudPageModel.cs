using _06_WebApp_RazoePage.RazorPage.Extensions;
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
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Movies
{
	public class BaseMovieCrudPageModel : BasePageModel<
		CrudMovieDtoModel,
		CrudMovieViewModel,
		MovieItemDtoModel,
		MovieItemViewModel>
	{
		public BaseMovieCrudPageModel(
			IHttpClientFactory httpClientFactory,
			ILogger<BaseRazorPage<MovieItemDtoModel, MovieItemViewModel>> logger,
			IMapper mapper)
			: base(httpClientFactory, "movie", logger, mapper)
		{
		}

		protected string MovieCoverImageUri => "https://localhost:7249";

		protected DeleteMovieViewModel DeleteEntityViewModel { get; set; } 

		protected virtual async Task<IActionResult> CrudMovieAsync(
			ModelState state = WebApi.Common.ModelState.create)
		{
			string modelStateName = state == WebApi.Common.ModelState.create
				? "ایجاد فیلم جدید" : "ویرایش فیلم";

			if (CrudEntityViewModel is null)
			{
				await SetMessage($"خطا در {modelStateName}", MessageStatus.danger);
				return RedirectToPage(IndexPage);
			}

			if (!ModelState.IsValid)
			{
				IReadOnlyList<string> modelErrors = ModelState.MapModelStateToErrorsList();
				modelErrors.MapToMessages(MessageStatus.danger);
				return await RedirectToMovieDefaultPostPage();
			} 

			using MultipartFormDataContent formContent = new MultipartFormDataContent()
			{
				{
					new StringContent(CrudEntityViewModel.Id.ToString(CultureInfo.InvariantCulture), System.Text.Encoding.UTF8, MediaTypeNames.Text.Plain),
					nameof(CrudEntityViewModel.Id)
				},
				{
					new StringContent(CrudEntityViewModel.Title, System.Text.Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.Title)
				},
				{
					new StringContent(CrudEntityViewModel.Description, System.Text.Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.Description)
				},
				{
					new StringContent(CrudEntityViewModel.ScoreString, System.Text.Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.ScoreString)
				},
				{
					new StringContent(CrudEntityViewModel.ImageName, System.Text.Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.ImageName)
				},
				{
					new StringContent(CrudEntityViewModel.StartDateViewModel, System.Text.Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.StartDateViewModel)
				},
				{
					new StringContent(CrudEntityViewModel.EndDateViewModel, System.Text.Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.EndDateViewModel)
				},
				{
					new StringContent(CrudEntityViewModel.GenreId.ToString(CultureInfo.InvariantCulture), System.Text.Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.GenreId)
				},
				{
					new StringContent(CrudEntityViewModel.TagsList, System.Text.Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.TagsList)
				}
			};

			if (CrudEntityViewModel.File is not null && CrudEntityViewModel.File.Length > 0)
			{
				using var ms = new MemoryStream();
				await CrudEntityViewModel.File.CopyToAsync(ms);
				ms.Position = 0;
				 
				string fileTypeName = Path.GetExtension(CrudEntityViewModel.File.FileName) switch
				{
					".jpg" or ".jpeg" => MediaTypeNames.Image.Jpeg,
					".png" => MediaTypeNames.Image.Png,
					_ => MediaTypeNames.Image.Jpeg
				};
				var fileContent = new ByteArrayContent(ms.ToArray());
				fileContent.Headers.ContentType = new MediaTypeHeaderValue(fileTypeName);
				formContent.Add(fileContent, nameof(CrudEntityViewModel.File), CrudEntityViewModel.File.FileName);
			}

			HttpResponseMessage response = new HttpResponseMessage();
			if (state is WebApi.Common.ModelState.create)
				response = await _client.PostAsync("api/movie/add", formContent);
			else if (state is WebApi.Common.ModelState.update)
				response = await _client.PutAsync("api/movie/edit", formContent);

			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<MovieItemDtoModel>(response);
				return await RedirectToMovieDefaultPostPage();
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<
				ApplicationServiceResult<MovieItemDtoModel>>(contentResult, serializerSetting);
			if (appResult is null)
			{
				await SetMessage($"خطا در {modelStateName}", MessageStatus.danger);
				return await RedirectToMovieDefaultPostPage();
			}

			if (appResult.IsSuccess && appResult.Result is not null)
			{
				appResult.Messages.MapToMessages(MessageStatus.success);
				
				if (Request.Cookies.ContainsKey("tags"))
					Response.Cookies.Delete("tags");

				return RedirectToPage(IndexPage);
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
			return await RedirectToMovieDefaultPostPage();
		}

		protected virtual async Task<List<SelectListItem>?> GetGenreSelectListAsync()
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

		protected virtual async Task<IActionResult> RedirectToMovieDefaultPostPage()
		{
			List<SelectListItem>? genreListItems = await GetGenreSelectListAsync();
			CrudEntityViewModel.GenreSelectList = genreListItems;
			return Page();
		}
	}
}
