using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.Pages.BasePage;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Customers;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Customers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Mime;
using System.Text;

namespace _06_WebApp_RazoePage.RazorPage.Pages.Customer
{
	public abstract class BaseCrudCustomerPageModel : BasePageModel<
		CrudCustomerDtoModel,
		CrudCustomerViewModel,
		CustomerItemDtoModel,
		CustomerItemViewModel>
	{
		protected BaseCrudCustomerPageModel(
			IHttpClientFactory httpClientFactory,
			ILogger<BaseRazorPage<CustomerItemDtoModel, CustomerItemViewModel>> logger,
			IMapper mapper)
			: base(httpClientFactory, "customer", logger, mapper)
		{
		}

		protected string ProfileImageUri => "https://localhost:7249";

		protected virtual async Task<IActionResult> GetCustomerPageResultAsync(long? id)
		{
			HttpResponseMessage response = await _client.GetAsync($"api/customer/{id}");
			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<CrudCustomerDtoModel>(response);
				return RedirectToPage(IndexPage);
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<
				ApplicationServiceResult<CrudCustomerDtoModel>>(contentResult);

			if (appResult is null)
			{
				await SetMessage("مشتری یافت نشد", MessageStatus.danger);
				return RedirectToPage(IndexPage);
			}

			if (appResult.IsSuccess && appResult.Result is not null)
			{
				CrudEntityViewModel = _mapper.Map<CrudCustomerViewModel>(appResult.Result);

				if (!string.IsNullOrEmpty(CrudEntityViewModel.ProfileImage))
					TempData["ProfileImage"] = $"{ProfileImageUri}/images/customers/{CrudEntityViewModel.ProfileImage}";

				return Page();
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
			return RedirectToPage(IndexPage);
		}

		protected virtual async Task<IActionResult> CrudCustomer(ModelState modelState = WebApi.Common.ModelState.create)
		{
			string modelStateMessage = modelState == WebApi.Common.ModelState.create
				? "خطا در ایجاد مشتری جدید" : "خطا در ویرایش مشتری";

			if (CrudEntityViewModel is null)
			{
				await SetMessage(modelStateMessage, MessageStatus.danger);
				return RedirectToPage(IndexPage);
			}

			if (!ModelState.IsValid)
			{
				IReadOnlyList<string> modelErrors = ModelState.MapModelStateToErrorsList();
				modelErrors.MapToMessages(MessageStatus.danger);
				return Page();
			}

			MultipartFormDataContent formContent = new MultipartFormDataContent()
			{
				{
					new StringContent(CrudEntityViewModel.Id.ToString(CultureInfo.InvariantCulture), Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.Id)
				},
				{
					new StringContent(CrudEntityViewModel.FirstName, Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.FirstName)
				},
				{
					new StringContent(CrudEntityViewModel.LastName, Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.LastName)
				},
				{
					new StringContent(CrudEntityViewModel.PhoneNumber, Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.PhoneNumber)
				},
				{
					new StringContent(CrudEntityViewModel.NationalCode, Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.NationalCode)
				},
				{
					new StringContent(CrudEntityViewModel.ProfileImage, Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(CrudEntityViewModel.ProfileImage)
				}
			};

			if (CrudEntityViewModel.File is not null && CrudEntityViewModel.File.Length > 0)
			{
				using var ms = new MemoryStream();
				await CrudEntityViewModel.File.CopyToAsync(ms);
				ms.Position = 0;

				string fileTypename = Path.GetExtension(CrudEntityViewModel.File.FileName) switch
				{
					".jpg" or ".jpeg" => MediaTypeNames.Image.Jpeg,
					".png" => MediaTypeNames.Image.Png,
					_ => MediaTypeNames.Image.Jpeg
				};

				var fileContent = new ByteArrayContent(ms.ToArray());
				fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(fileTypename);
				formContent.Add(fileContent, nameof(CrudEntityViewModel.File), CrudEntityViewModel.File.FileName);
			}

			HttpResponseMessage response = new HttpResponseMessage();
			if (modelState is WebApi.Common.ModelState.create)
				response = await _client.PostAsync("api/customer/add", formContent);
			else if (modelState is WebApi.Common.ModelState.update)
				response = await _client.PutAsync("api/customer/edit", formContent);

			if (!response.IsSuccessStatusCode)
			{
				await GetResponseErrorMessages<CustomerItemDtoModel>(response);
				return Page();
			}

			string contentResult = await response.Content.ReadAsStringAsync();
			var appResult = JsonConvert.DeserializeObject<
				ApplicationServiceResult<CrudCustomerDtoModel>>(contentResult);

			if (appResult is null)
			{
				await SetMessage(modelStateMessage, MessageStatus.danger);
				return Page();
			}

			if (appResult.IsSuccess && appResult.Result is not null)
			{
				appResult.Messages.MapToMessages(MessageStatus.success);
				return RedirectToPage(IndexPage);
			}

			appResult.Errors.MapToMessages(MessageStatus.danger);
			return Page();
		}
	}
}
