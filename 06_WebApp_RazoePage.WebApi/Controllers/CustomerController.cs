using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.Data.ProjectionModels;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Customers;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tickets;
using _06_WebApp_RazoePage.WebApi.Extensions;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;

namespace _06_WebApp_RazoePage.WebApi.Controllers
{ 
	public class CustomerController : BaseApiController<
		Customer,
		CustomerItemDtoModel,
		CrudCustomerDtoModel,
		DeleteCustomerDtoModel,
		CustomerController>
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly IWebHostEnvironment _webHost;
		private readonly string _customerImageDirPath;

		public CustomerController(
			ICustomerRepository repository,
			IMapper mapper,
			IApplicationServiceResultSelector resultSelector,
			IModelStateArgs<CustomerController> modelStateArgs,
			IValidator<CrudCustomerDtoModel> validator,
			IWebHostEnvironment webHost)
			: base(repository, mapper, resultSelector, modelStateArgs, validator)
		{
			_webHost = webHost;
			_customerRepository = repository;
			_customerImageDirPath = Path.Combine(webHost.ContentRootPath, "assets", "images", "customers");
		}

		[HttpGet("projectlist")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetCustomerProjectionList()
		{
			IEnumerable<CustomerProjectModel> customerProjectDataModel = await _customerRepository.GetCustomersProjectModelListAsync();

			var customerProjectListDtoModel = _mapper.Map<IEnumerable<CustomerProjectDtoModel>>(customerProjectDataModel);

			var appResult = _resultSelector.GetResultList<CustomerProjectDtoModel>(customerProjectListDtoModel);

			return Ok(appResult);
		}


		[HttpPost("add")] 
		public async Task<IActionResult> CreateCustomer()
		{
			var appResult = _resultSelector.GetSingleResult<CustomerItemDtoModel?>();
			
			CrudCustomerDtoModel? customerDtoModel = GetCustomerDtoModelFromRequest();
			if (customerDtoModel is null)
				return ReturnModelActionResult<CustomerItemDtoModel>(
					appResult,
					Common.ModelState.create,
					HttpStatusCode.BadRequest);

			IReadOnlyList<string> modelErrors = await ValidateModel(customerDtoModel);
			if (modelErrors.Count > 0)
				return ReturnModelActionResult<CustomerItemDtoModel>(
					appResult,
					Common.ModelState.create,
					HttpStatusCode.BadRequest,
					modelErrors);

			if (customerDtoModel.File is not null &&
				customerDtoModel.File.Length > 0)
				customerDtoModel.ProfileImage = await customerDtoModel.File.UploadImageAsync($"{customerDtoModel.FirstName} {customerDtoModel.LastName}", _webHost, _customerImageDirPath) ??  string.Empty;

			Customer customer = _mapper.Map<Customer>(customerDtoModel);
			Customer? createdCustomer = await _customerRepository.CreateEntityAsync(customer);

			return ReturnModelResponse(appResult, createdCustomer, Common.ModelState.create);
		}

		[HttpPut("edit")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)] 
		public async Task<IActionResult> PutCustomer()
		{
			var appResult = _resultSelector.GetSingleResult<CustomerItemDtoModel?>();
			CrudCustomerDtoModel? customerDtoModel = GetCustomerDtoModelFromRequest();

			if (customerDtoModel is null)
				return ReturnModelActionResult(
					appResult,
					Common.ModelState.update,
					HttpStatusCode.BadRequest);

			IReadOnlyList<string> modelErrors = await ValidateModel(customerDtoModel);
			if (modelErrors.Count > 0)
				return ReturnModelActionResult(
					appResult,
					Common.ModelState.update,
					HttpStatusCode.BadRequest);

			if(customerDtoModel.File is not null &&
				customerDtoModel.File.Length > 0)
			{
				string? newFileName = await customerDtoModel.File.EditImageAsync(
					customerDtoModel.ProfileImage,
					$"{customerDtoModel.FirstName} {customerDtoModel.LastName}",
					_webHost,
					_customerImageDirPath);

				customerDtoModel.ProfileImage = !string.IsNullOrEmpty(newFileName)
					? newFileName : customerDtoModel.ProfileImage;
			}

			Customer customer = _mapper.Map<Customer>(customerDtoModel);
			Customer? updatedCustomer = await _customerRepository.UpdateEntityAsync(customer);
			return ReturnModelResponse(
				appResult,
				updatedCustomer,
				Common.ModelState.update);
		}

		[HttpDelete("{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public override async Task<IActionResult> Delete(long? id)
		{
			var appResult = _resultSelector.GetSingleResult<CustomerItemDtoModel?>();
			if (id is null or <= 0)
				return ReturnModelActionResult(
					appResult,
					Common.ModelState.delete,
					HttpStatusCode.BadRequest);

			Customer? customer = await _customerRepository.GetEntityByIdAsync(id ?? 0);
			if (customer is null)
				return ReturnModelActionResult(
					appResult,
					Common.ModelState.delete,
					HttpStatusCode.NotFound);

			if (!string.IsNullOrEmpty(customer.ProfileImage))
				customer.ProfileImage.DeleteImage(_webHost, "customers");

			customer = await _customerRepository.DeleteEntityAsync(customer);
			return ReturnModelResponse(
				appResult,
				customer,
				Common.ModelState.delete);
		}


		[HttpGet("getproject/{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationServiceResult<CustomerProjectDtoModel>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetMovieProjectById(long? id)
		{
			var appResult = _resultSelector.GetSingleResult<CustomerProjectDtoModel?>();
			if (id is null or <= 0)
				return ReturnModelActionResult(
					appResult,
					Common.ModelState.read,
					HttpStatusCode.BadRequest);

			CustomerProjectModel? customerProjectModel = await _customerRepository.GetCustomerProjectByIdAsync(id ?? 0);

			if (customerProjectModel is null)
				return ReturnModelActionResult(
					appResult,
					Common.ModelState.read,
					HttpStatusCode.NotFound);

			var customerProjectDtoModel = _mapper.Map<CustomerProjectDtoModel>(customerProjectModel);
			appResult.AddResult(customerProjectDtoModel);
			return Ok(appResult);
		} 

		private CrudCustomerDtoModel? GetCustomerDtoModelFromRequest()
		{
			if (Request.Form is null ||
				Request.Form.Keys.Count == 0 ||
				!Request.HasFormContentType)
				return default;

			string[] formKeys = Request.Form.Keys?.ToArray() ?? [];

			CrudCustomerDtoModel customerModel = new CrudCustomerDtoModel();
			string idString = Request.Form?[nameof(customerModel.Id)].ToString() ?? string.Empty;
			customerModel.Id = long.TryParse(idString, CultureInfo.InvariantCulture, out long id) ? id : 0;

			customerModel.FirstName = Request.Form?[nameof(customerModel.FirstName)].ToString() ?? string.Empty;

			customerModel.LastName = Request.Form?[nameof(customerModel.LastName)].ToString() ?? string.Empty;

			customerModel.ProfileImage = Request.Form?[nameof(customerModel.ProfileImage)].ToString() ?? string.Empty;

			customerModel.NationalCode = Request.Form?[nameof(customerModel.NationalCode)].ToString() ?? string.Empty;

			customerModel.PhoneNumber = Request.Form?[nameof(customerModel.PhoneNumber)].ToString() ?? string.Empty; 

			if (Request.Form?.Files.Count > 0 &&
				Request.Form?.Files[0].Length > 0)
				customerModel.File = Request.Form?.Files[0];

			return customerModel;
		}
	}
}
