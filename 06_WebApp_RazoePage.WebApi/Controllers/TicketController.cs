using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.Data.ProjectionModels;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tickets;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.WebApi.Controllers
{
	public class TicketController : BaseApiController<
		Ticket,
		TicketItemDtoModel,
		CrudTicketDtoItem,
		DeleteTicketDtoItem,
		TicketController>
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly ITicketRepository _ticketRepository;

		public TicketController(
			ITicketRepository repository,
			IMapper mapper,
			IApplicationServiceResultSelector resultSelector,
			IModelStateArgs<TicketController> modelStateArgs,
			IValidator<CrudTicketDtoItem> validator,
			ICustomerRepository customerRepository)
			: base(repository, mapper, resultSelector, modelStateArgs, validator)
		{
			_customerRepository = customerRepository;
			_ticketRepository = repository;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationServiceResult<TicketItemDtoModel>))]

		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationServiceResult<TicketItemDtoModel?>))]
		public override async Task<IActionResult> Post([FromBody] CrudTicketDtoItem? model)
		{
			var appResult = _resultSelector.GetSingleResult<TicketItemDtoModel?>();

			IReadOnlyList<string> modelErrors = await ValidateModel(model);
			if (modelErrors.Any())
				return ReturnModelActionResult(
					appResult,
					Common.ModelState.create,
					System.Net.HttpStatusCode.BadRequest,
					modelErrors); 

			Ticket ticket = _mapper.Map<Ticket>(model); 
			Ticket? createdTicket = await _repository.CreateEntityAsync(ticket);

			return ReturnModelResponse(
				appResult,
				createdTicket,
				Common.ModelState.create);
		}

		[HttpGet("ticketlist")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationServiceResult<IEnumerable<TicketListDtoModel>>))]
		public async Task<IActionResult> GetTicketList()
		{
			IEnumerable<TicketListModel> ticketListModel = await _ticketRepository.GetTicketListModelAsync();

			var ticketListDtoModel =  _mapper.Map<IEnumerable<TicketListDtoModel>>(ticketListModel);
			var appResult = _resultSelector.GetResultList(ticketListDtoModel);
			
			return Ok(appResult);
		}

		/// <summary>
		/// Find Ticket by id
		/// </summary>
		/// <param name="id">Ticket Id</param>
		/// <returns>
		/// return ApplicationServiceResult<TicketProjectionDtoModel>
		/// return BadRequest if id is null or equal 0
		/// return notfound if return null
		/// return ok if returned TicketProjectionModel is not null
		/// </returns>
		[HttpGet("find/{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK)] 
		[ProducesResponseType(StatusCodes.Status400BadRequest)] 
		[ProducesResponseType(StatusCodes.Status404NotFound)] 
		public async Task<IActionResult> FindTicketById(long? id)
		{
			var appResult = _resultSelector.GetSingleResult<TicketProjectionDtoModel?>();

			if (id is null or <= 0)
				return ReturnModelActionResult(
					appResult,
					Common.ModelState.read,
					System.Net.HttpStatusCode.BadRequest);

			TicketProjecttionModel? ticketProjecttionModel = await _ticketRepository.GetTicketDetailsByIdAsync(id ?? 0);

			if (ticketProjecttionModel is null)
				return ReturnModelActionResult(
					appResult,
					Common.ModelState.read,
					System.Net.HttpStatusCode.NotFound);

			var ticketProjectDtoModel = _mapper.Map<TicketProjectionDtoModel>(ticketProjecttionModel);
			appResult.AddResult(ticketProjectDtoModel);

			var serializedResult = JsonConvert.SerializeObject(appResult, serializerSettins);
			return Ok(serializedResult);
		}

	}
}
