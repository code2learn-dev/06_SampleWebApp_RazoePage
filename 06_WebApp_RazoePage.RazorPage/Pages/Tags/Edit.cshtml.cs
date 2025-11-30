using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Taggs;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tags;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Tags
{
    public class EditModel : BasePageModel<
        CrudTagDtoModel,
        CrudTagViewModel,
        TagItemDtoModel,
        TagItemViewModel>
    {
		public EditModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<BaseRazorPage<TagItemDtoModel, TagItemViewModel>> logger, 
            IMapper mapper) 
            : base(httpClientFactory, "tag", logger, mapper)
		{
		}
         
		public CrudTagViewModel? CrudTagViewModel { get; set; }

		public async Task<IActionResult> OnGetAsync()
        {
            CrudTagViewModel = TempData.MapToTargetData<CrudTagViewModel>("TagModel");
            if (CrudTagViewModel is null || CrudTagViewModel.Id == 0)
			{
                await SetMessage("خطا در ویرایش تگ", Extensions.MessageStatus.danger);
				return RedirectToPage(IndexPage);
			}

            if(CrudTagViewModel.ActionMethod is not ActionMethod.edit)
            {
                await SetMessage("خطا در ویرایش تگ", Extensions.MessageStatus.danger);
				return RedirectToPage(IndexPage);
            }

            CrudEntityViewModel = CrudTagViewModel;
			TagItemViewModel? tagItemViewModel = await EditEntityService("خطا در ویرایش تگ");
            return RedirectToPage(IndexPage);
		}
    }
}
