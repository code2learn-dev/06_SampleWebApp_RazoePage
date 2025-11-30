using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Taggs;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tags;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Tags
{
    public class IndexModel : BaseFetchPageModel<TagItemDtoModel, TagItemViewModel>
    {
		public IndexModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<BaseRazorPage<TagItemDtoModel, TagItemViewModel>> logger, 
            IMapper mapper) 
            : base(httpClientFactory, "tag", logger, mapper)
		{
		}

        [BindProperty]
		public CrudTagViewModel CrudTagViewModel { get; set; }

		public async Task<IActionResult> OnGetAsync()
        {
            CrudTagViewModel = new CrudTagViewModel();
            await GetAllEntityViewModelListAsync("تگی یافت نشد");
            return EntityViewModelList is not null ? Page() : RedirectToPage("/Error/Index");
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
				IReadOnlyList<string> modelErrors = ModelState.MapModelStateToErrorsList();
                modelErrors.MapToMessages(MessageStatus.danger);
                return Page();
            }

            TempData.MapToTempData<CrudTagViewModel>(CrudTagViewModel, "TagModel");
            
            return CrudTagViewModel.ActionMethod switch {
                ActionMethod.add => RedirectToPage("Add"),
                ActionMethod.edit => RedirectToPage("Edit"),
                ActionMethod.delete => RedirectToPage("Delete"),
                _ => RedirectToPage(IndexPage)
            };
        }
    }
}
