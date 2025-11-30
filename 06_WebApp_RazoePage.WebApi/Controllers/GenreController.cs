using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.WebApi.Common;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace _06_WebApp_RazoePage.WebApi.Controllers
{
	public class GenreController
		: BaseApiController<
			Genre,
			GenreDtoModel,
			CrudGenreDtoModel,
			GenreDtoModel,
			GenreController>
	{
		public GenreController(
			IGenreRepository repository, 
			IMapper mapper, 
			IApplicationServiceResultSelector resultSelector, 
			IModelStateArgs<GenreController> modelStateArgs, 
			IValidator<CrudGenreDtoModel> validator) 
			: base(repository, mapper, resultSelector, modelStateArgs, validator)
		{
		}

		[HttpGet("find/json/{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationServiceResult<GenreDtoModel>))]
		[Produces(MediaTypeNames.Application.Json)]
		public async Task<IActionResult> GetByIdInJsonFormat(long? id) => await GetById(id);

		[HttpGet("find/xml/{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationServiceResult<GenreDtoModel>))]
		[Produces(MediaTypeNames.Application.Xml)]
		public async Task<IActionResult> GetByIdInXmlFormat(long? id)
		{
			var appResult = new ApplicationServiceResult<CrudGenreDtoModel?>();
			if (id is null or < 0)
			{
				_modelStateArgs.SetModelMessage(Common.ModelState.read, HttpStatusCode.BadRequest);
				appResult.AddError(_modelStateArgs.Message, HttpStatusCode.BadRequest);
				return BadRequest(appResult);
			}

			Genre? genre = await _repository.GetEntityByIdAsync(id ?? 0);
			if (genre is null)
			{
				_modelStateArgs.SetModelMessage(Common.ModelState.read, HttpStatusCode.NotFound);
				appResult.AddError(_modelStateArgs.Message, HttpStatusCode.NotFound);
				return BadRequest(appResult);
			}

			CrudGenreDtoModel crudGenreDtoModel = _mapper.Map<CrudGenreDtoModel>(genre);
			appResult.AddResult(crudGenreDtoModel);
			return Ok(appResult);
		}
	}
}
