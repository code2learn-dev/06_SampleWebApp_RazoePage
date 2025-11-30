using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Taggs;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tags;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Tags
{
	public class DeleteModel : DeletePageModel<
			TagItemDtoModel,
			TagItemViewModel,
			TagItemDtoModel,
			TagItemViewModel>
	{
		public DeleteModel(
			IHttpClientFactory httpClientFactory,
			ILogger<BaseRazorPage<TagItemDtoModel, TagItemViewModel>> logger, 
			IMapper mapper) 
			: base(httpClientFactory, "tag", logger, mapper)
		{
		}

		public async Task<IActionResult> OnGetAsync()
		{
			string errorMessage = "Œÿ« œ— Õ–›  ê";
			CrudTagViewModel? crudTagViewModel = TempData.MapToTargetData<CrudTagViewModel>("TagModel");
			if(crudTagViewModel is null)
			{
				await SetMessage(errorMessage, MessageStatus.danger);
				return RedirectToPage(IndexPage);
			}

			if(crudTagViewModel.ActionMethod is not ActionMethod.delete)
			{
				await SetMessage(errorMessage, MessageStatus.danger);
				return RedirectToPage(IndexPage);
			}

			DeleteEntityViewModel = new TagItemViewModel()
			{
				Id = crudTagViewModel.Id,
				Name = crudTagViewModel.Name
			};

			await DeleteEntityService(errorMessage);
			return RedirectToPage(IndexPage);
		}

	}
}
