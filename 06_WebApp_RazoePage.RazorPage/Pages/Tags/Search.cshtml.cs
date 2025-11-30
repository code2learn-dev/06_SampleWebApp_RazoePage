using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Taggs;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tags;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Tags
{
    public class SearchModel : BaseFetchPageModel<TagItemDtoModel, TagItemViewModel>
    {
		public SearchModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<BaseRazorPage<TagItemDtoModel, TagItemViewModel>> logger, 
            IMapper mapper) 
            : base(httpClientFactory, "tag", logger, mapper)
		{
		}

        public async Task<JsonResult> OnGetAsync(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                await SetMessage("تگی یافت نشد", MessageStatus.danger);
                return new JsonResult(Array.Empty<string>());
            }
			HttpResponseMessage response = await _client.GetAsync($"api/tag/filter/{name}");
            if(!response.IsSuccessStatusCode)
            {
                await GetResponseErrorMessages<TagItemDtoModel>(response);
                return new JsonResult(Array.Empty<string>());
            }
            
            string contentResult = await response.Content.ReadAsStringAsync();
            var appResult = JsonConvert.DeserializeObject<ApplicationServiceResult
                <IEnumerable<TagItemDtoModel>>>(contentResult);
            if (appResult is null) return new JsonResult(Array.Empty<string>());

            if(appResult.IsSuccess)
            {
                var tagListViewModel = _mapper.Map<IEnumerable<TagItemViewModel>>(appResult.Result);
                return new JsonResult(tagListViewModel);
            }

            appResult.Errors.MapToMessages(MessageStatus.danger);
            return new JsonResult(Array.Empty<string>());
        }

    }
}
