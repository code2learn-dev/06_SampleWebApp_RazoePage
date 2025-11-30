using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Taggs;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tags;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Tags
{
	public class AddModel : BasePageModel<
		CrudTagDtoModel,
		CrudTagViewModel,
		TagItemDtoModel,
		TagItemViewModel>
	{
		public AddModel(
			IHttpClientFactory httpClientFactory,
			ILogger<BaseRazorPage<TagItemDtoModel, TagItemViewModel>> logger,
			IMapper mapper)
			: base(httpClientFactory, "tag", logger, mapper)
		{
		} 

		public async Task<IActionResult> OnGetAsync()
		{
			if (!TempData.TryGetValue("TagModel", out object? value))
			{
				await SetMessage("خطا در ذخیره تگ جدید", MessageStatus.danger);
				return RedirectToPage(IndexPage);
			}
			string tagModelString = value?.ToString() ?? string.Empty;
			CrudTagViewModel? tagViewModel = JsonConvert.DeserializeObject<CrudTagViewModel>(tagModelString);
			if (tagViewModel is null)
			{
				await SetMessage("خطا در ذخیره تگ جدید", MessageStatus.danger);
				return RedirectToPage(IndexPage);
			}

			CrudEntityViewModel = tagViewModel;
			TagItemViewModel? tagItemViewModel = await CreateEntityService("خطا در ذخیره تگ جدید");
			return RedirectToPage(IndexPage);
		}

	}
}
