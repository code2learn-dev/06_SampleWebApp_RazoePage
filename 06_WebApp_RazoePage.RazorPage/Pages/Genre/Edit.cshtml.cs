using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Genres;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Genres;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Genre
{
    public class EditModel : BasePageModel<
        CrudGenreDtoModel,
        CrudGenreViewModel,
        GenreDtoModel,
        GenreItemViewModel>
    {
		public EditModel(
            IHttpClientFactory httpClientFactory,  
            ILogger<BaseRazorPage<GenreDtoModel, GenreItemViewModel>> logger, 
            IMapper mapper) 
            : base(httpClientFactory, "genre", logger, mapper)
		{
		}

        public async Task<IActionResult> OnGetAsync(long? id, string? format)
        {
			//await FindEntityToCrudAsync(id);
			RequestFormat requestFormat = (RequestFormat)(Enum.TryParse(typeof(RequestFormat), format, true, out object? formatResult) ? formatResult : RequestFormat.json);
            string apiRequestUri = $"api/genre/find/{requestFormat}/{id}";
			HttpResponseMessage response = await _client.GetAsync(apiRequestUri);
            if(!response.IsSuccessStatusCode)
            {
                await GetResponseErrorMessages<GenreDtoModel>(response);
                return RedirectToPage(IndexPage);
            }

            string contentResult = await response.Content.ReadAsStringAsync();
            var appResult = new ApplicationServiceResult<CrudGenreDtoModel>();
            if (requestFormat is RequestFormat.xml)
            {
                var serializer = new DataContractSerializer(typeof(ApplicationServiceResult<CrudGenreDtoModel>));
                await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(contentResult));
                var dtoModelObject = serializer.ReadObject(stream);
                appResult = dtoModelObject as ApplicationServiceResult<CrudGenreDtoModel>;
            }
            else if (requestFormat is RequestFormat.json)
            {
                appResult = JsonConvert.DeserializeObject<
                    ApplicationServiceResult<CrudGenreDtoModel>>(contentResult);
            }
            if(appResult is null || appResult.Result is null)
            {
                await SetMessage("دسته بندی یافت نشد", MessageStatus.danger);
                return RedirectToPage(IndexPage);
            }

            CrudEntityViewModel = _mapper.Map<CrudGenreViewModel>(appResult.Result);
            return CrudEntityViewModel is not null ? Page() : RedirectToPage("Index");
        }


        public async Task<IActionResult> OnPostAsync()
        {
			if (!ModelState.IsValid)
			{
				IReadOnlyList<string> modelErrors = ModelState.MapModelStateToErrorsList();
				modelErrors.MapToMessages(MessageStatus.danger);
				return RedirectToPage(nameof(IndexPage));
			}

			GenreItemViewModel? editedGenre = await EditEntityService("خطا در ویرایش دسته بندی");
            return editedGenre is null ? Page() : RedirectToPage("Index");
        }
    }
}
