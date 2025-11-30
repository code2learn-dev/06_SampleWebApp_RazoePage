using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Movies
{
    public class MoviesJsonResultListModel : BaseFetchPageModel<
        MovieItemDtoModel,
        MovieItemViewModel>
    {
		public MoviesJsonResultListModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<BaseRazorPage<MovieItemDtoModel, MovieItemViewModel>> logger, 
            IMapper mapper) 
            : base(httpClientFactory, "movie", logger, mapper)
		{
		}

        public async Task<JsonResult> OnGetAsync()
        {
			HttpResponseMessage response = await _client.GetAsync("api/movie");
            if(!response.IsSuccessStatusCode)
            {
                await GetResponseErrorMessages<MovieItemDtoModel>(response);
                return new JsonResult(Array.Empty<string>());
            }

            string contentResult = await response.Content.ReadAsStringAsync();
            var appResult = JsonConvert.DeserializeObject<
                ApplicationServiceResult<IEnumerable<MovieItemDtoModel>>>(contentResult);

            if(appResult is null)
            {
                await SetMessage("فیلمی یافت نشد", Extensions.MessageStatus.danger);
                return new JsonResult(Array.Empty<string>());
            }

            if(appResult.IsSuccess)
            {
                var movieViewModelList = _mapper.Map<IEnumerable<MovieItemViewModel>>(appResult.Result);
                return new JsonResult(movieViewModelList);
            }

            appResult.Errors.MapToMessages(MessageStatus.danger);
            return new JsonResult(Array.Empty<string>());
        }

    }
}
