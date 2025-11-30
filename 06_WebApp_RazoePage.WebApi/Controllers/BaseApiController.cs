using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace _06_WebApp_RazoePage.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public abstract class BaseApiController<
		TEntity,
		TEntityDto,
		TCrudEntityDto,
		TDeleteEntityDto,
		TController>

		: ControllerBase

		where TEntity : BaseEntity
		where TEntityDto : BaseDtoModel
		where TCrudEntityDto : BaseDtoModel
		where TDeleteEntityDto : BaseDtoModel
		where TController : ControllerBase
	{
		protected readonly IGenericRepository<TEntity> _repository;
		protected readonly IMapper _mapper;
		protected readonly IApplicationServiceResultSelector _resultSelector;
		protected readonly IModelStateArgs<TController> _modelStateArgs;
		protected readonly IValidator<TCrudEntityDto> _validator;

		protected JsonSerializerSettings serializerSettins = new JsonSerializerSettings()
		{
			ContractResolver = new PrivateSetterContractResolver(),
			TypeNameHandling = TypeNameHandling.Auto,
			TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
			Formatting = Formatting.Indented
		};

		protected BaseApiController(
			IGenericRepository<TEntity> repository,
			IMapper mapper,
			IApplicationServiceResultSelector resultSelector,
			IModelStateArgs<TController> modelStateArgs,
			IValidator<TCrudEntityDto> validator)
		{
			_repository = repository;
			_mapper = mapper;
			_resultSelector = resultSelector;
			_modelStateArgs = modelStateArgs;
			_validator = validator;
		} 

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public virtual async Task<IActionResult> Get()
		{
			IEnumerable<TEntity> entites = await _repository.GetAllAsync();
			var entitesDto = _mapper.Map<IEnumerable<TEntityDto>>(entites);
			var appResult = _resultSelector.GetResultList(entitesDto);
			return Ok(appResult);
		}

		[HttpGet("all/{format}")]
		[FormatFilter]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllByFormat()
		{
			IEnumerable<TEntity> entites = await _repository.GetAllAsync();
			var entitesDto = _mapper.Map<IEnumerable<TEntityDto>>(entites);
			var appResult = _resultSelector.GetResultList(entitesDto);
			return Ok(appResult);
		}

		[HttpGet("filter")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public virtual async Task<IActionResult> Get(FilterDtoModel<TEntity>? filterModel)
		{
			IEnumerable<TEntity> entites = [];
			if (filterModel is not null)
				entites = await _repository.FilterByPredicate(filterModel.Predicate, filterModel.OrderBy);
			else
				entites = await _repository.GetAllAsync();

			var entitesDto = _mapper.Map<IEnumerable<TEntityDto>>(entites);
			var appResult = _resultSelector.GetResultList(entitesDto);
			return Ok(appResult);
		}

		[HttpGet("{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public virtual async Task<IActionResult> GetById(long? id) 
			=> await FindEntityActionResultAsync(id, Common.ModelState.create);


		[HttpGet("delete/{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public virtual async Task<IActionResult> GetDeleteEntityById(long? id) 
			=> await FindEntityActionResultAsync(id, Common.ModelState.delete);


		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public virtual async Task<IActionResult> Post([FromBody] TCrudEntityDto? model)
		{
			IReadOnlyList<string> validateErrors = await ValidateModel(model);
			if (validateErrors.Any())
				return GetApiActionResult(
					null,
					Common.ModelState.create,
					HttpStatusCode.BadRequest,
					validateErrors);

			TEntity entity = _mapper.Map<TEntity>(model);
			TEntity? createdEntity = await _repository.CreateEntityAsync(entity);
			return GetApiActionResult(createdEntity, Common.ModelState.create);
		}


		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public virtual async Task<IActionResult> Put([FromBody] TCrudEntityDto model)
		{
			IReadOnlyList<string> modelErrors = await ValidateModel(model, Common.ModelState.update);
			if (modelErrors.Any())
				return GetApiActionResult(null, Common.ModelState.update, HttpStatusCode.BadRequest, modelErrors);

			TEntity entity = _mapper.Map<TEntity>(model);
			TEntity? updatedEntity = await _repository.UpdateEntityAsync(entity);
			return GetApiActionResult(updatedEntity, Common.ModelState.update);
		}

		[HttpDelete("{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public virtual async Task<IActionResult> Delete(long? id)
		{
			if (id is null or < 0)
				return GetApiActionResult(null, Common.ModelState.delete, HttpStatusCode.BadRequest);

			TEntity? entity = await _repository.GetEntityByIdAsync(id ?? 0);
			if (entity is null)
				return GetApiActionResult(null, Common.ModelState.delete, HttpStatusCode.NotFound);

			entity = await _repository.DeleteEntityAsync(entity);

			return GetApiActionResult(entity, Common.ModelState.delete);
		} 


		protected IActionResult ReturnModelResponse<TMovieDtoModel>(
			ApplicationServiceResult<TMovieDtoModel?> appResult,
			TEntity? model,
			ModelState modelState)
			where TMovieDtoModel : BaseDtoModel
		{
			string serializedResult = string.Empty;

			if (model is null)
				return ReturnModelActionResult(
						appResult,
						modelState,
						HttpStatusCode.BadRequest);

			var movieItemDtoItem = _mapper.Map<TMovieDtoModel>(model);
			appResult.AddResult(movieItemDtoItem);

			return ReturnModelActionResult(
				appResult,
				modelState,
				HttpStatusCode.OK);
		}

		protected IActionResult ReturnModelActionResult<TMovieDtoMovel>(
			ApplicationServiceResult<TMovieDtoMovel?> appResult,
			ModelState modelState,
			HttpStatusCode statusCode,
			IReadOnlyList<string>? errorsList = null)
			where TMovieDtoMovel : BaseDtoModel
		{
			_modelStateArgs.SetModelMessage(modelState, statusCode, errorsList);
			if (statusCode is not HttpStatusCode.OK &&
				errorsList is not null &&
				errorsList.Any())
				appResult.AddErrorsList(_modelStateArgs.MessagesList, statusCode);
			else if (statusCode is not HttpStatusCode.OK &&
				errorsList is null)
				appResult.AddError(_modelStateArgs.Message, statusCode);
			else
				appResult.AddMessage(_modelStateArgs.Message);
			string serializedResult = JsonConvert.SerializeObject(appResult);
			return statusCode is HttpStatusCode.OK ? Ok(serializedResult) : BadRequest(appResult);
		}

		private IActionResult GetApiActionResult(
			TEntity? entityResult,
			ModelState modelState,
			HttpStatusCode? statusCode = null,
			IReadOnlyList<string>? modelErrors = null)
		{
			var appResult = _resultSelector.GetSingleResult<TEntityDto>();

			switch (modelState)
			{
				case Common.ModelState.create when entityResult is not null:
				case Common.ModelState.update when entityResult is not null:
				case Common.ModelState.delete when entityResult is not null:

					TEntityDto entityDto = _mapper.Map<TEntityDto>(entityResult);
					_modelStateArgs.SetModelMessage(modelState, HttpStatusCode.OK);
					appResult.AddMessage(_modelStateArgs.Message);
					appResult.AddResult(entityDto);

					var serializedResult = JsonConvert.SerializeObject(appResult, serializerSettins);
					return Ok(serializedResult);

				case Common.ModelState.create
					when entityResult is null && modelErrors is not null && modelErrors.Any():
				case Common.ModelState.update
					when entityResult is null && modelErrors is not null && modelErrors.Any():
				case Common.ModelState.delete
					when entityResult is null && modelErrors is not null && modelErrors.Any():

					appResult.AddErrorsList(modelErrors, HttpStatusCode.BadRequest);
					serializedResult = JsonConvert.SerializeObject(appResult, serializerSettins);
					return statusCode is HttpStatusCode.NotFound
						? NotFound(serializedResult) : BadRequest(serializedResult);

				case Common.ModelState.create
					when entityResult is null && modelErrors is null:
				case Common.ModelState.update
					when entityResult is null && modelErrors is null:
				case Common.ModelState.delete
					when entityResult is null && modelErrors is null:

					HttpStatusCode responseStatusCode = HttpStatusCode.BadRequest;
					if (statusCode is not null)
					{
						_modelStateArgs.SetModelMessage(modelState, (HttpStatusCode)statusCode);
						responseStatusCode = (HttpStatusCode)statusCode;
					}
					else
						_modelStateArgs.SetModelMessage(modelState, HttpStatusCode.BadRequest);

					appResult.AddError(_modelStateArgs.Message, responseStatusCode);
					serializedResult = JsonConvert.SerializeObject(appResult, serializerSettins);
					return statusCode is not null && statusCode is HttpStatusCode.NotFound
						? NotFound(serializedResult)
						: BadRequest(serializedResult);

				default:
					appResult.AddError("داده ای یافت نشد", HttpStatusCode.NotFound);
					serializedResult = JsonConvert.SerializeObject(appResult, serializerSettins);
					return NotFound(serializedResult);
			}
		}

		protected virtual async Task<IReadOnlyList<string>> ValidateModel(
			TCrudEntityDto? entity, ModelState modelState = Common.ModelState.create)
		{
			if (entity is null)
			{
				_modelStateArgs.SetModelMessage(modelState, HttpStatusCode.BadRequest);
				return [_modelStateArgs.Message];
			}

			ValidationResult validationResult = await _validator.ValidateAsync(entity);
			if (!validationResult.IsValid)
			{
				IReadOnlyList<string> modelErrors = validationResult.Errors.Select(a => a.ErrorMessage).ToList();
				return modelErrors;
			}

			return [];
		}

		protected virtual async Task<IActionResult> FindEntityActionResultAsync(
			long? id, ModelState modelState = Common.ModelState.delete)
		{
			var appResult = new ApplicationServiceResult<object?>();
			if (id is null or < 0)
			{
				_modelStateArgs.SetModelMessage(Common.ModelState.read, HttpStatusCode.BadRequest);
				appResult.AddError(_modelStateArgs.Message, HttpStatusCode.BadRequest);
				return BadRequest(appResult);
			}

			TEntity? entity = await _repository.GetEntityByIdAsync(id ?? 0);
			if (entity is null)
			{
				_modelStateArgs.SetModelMessage(Common.ModelState.read, HttpStatusCode.NotFound);
				appResult.AddError(_modelStateArgs.Message, HttpStatusCode.NotFound);
				return BadRequest(appResult);
			}

			_modelStateArgs.SetModelMessage(Common.ModelState.read, HttpStatusCode.OK);
			appResult.AddMessage(_modelStateArgs.Message);

			if (modelState is Common.ModelState.delete)
			{
				TDeleteEntityDto deleteEntityDto = _mapper.Map<TDeleteEntityDto>(entity);
				appResult.AddResult(deleteEntityDto);
			}
			else
			{
				TCrudEntityDto crudEntityDto = _mapper.Map<TCrudEntityDto>(entity);
				appResult.AddResult(crudEntityDto);
			}

			return Ok(appResult);
		}
	}
}
