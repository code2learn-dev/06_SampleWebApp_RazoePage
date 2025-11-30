using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.Data.ProjectionModels;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels;
using _06_WebApp_RazoePage.WebApi.DtoModels.Customers;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tags;
using _06_WebApp_RazoePage.WebApi.Extensions;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;

namespace _06_WebApp_RazoePage.WebApi.Controllers
{
	public class MovieController
		: BaseApiController<
			Movie,
			MovieItemDtoModel,
			CrudMovieDtoModel,
			DeleteMovieDtoModel,
			MovieController>
	{
		private readonly IMovieRepository _movieRepository;
		private readonly IWebHostEnvironment _webHost;
		private readonly string _moviesImageDirPath;

		public MovieController(
			IMovieRepository repository,
			IMapper mapper,
			IApplicationServiceResultSelector resultSelector,
			IModelStateArgs<MovieController> modelStateArgs,
			IValidator<CrudMovieDtoModel> validator,
			IWebHostEnvironment webHost)
			: base(repository, mapper, resultSelector, modelStateArgs, validator)
		{
			this._movieRepository = repository;
			_webHost = webHost;
			_moviesImageDirPath = Path.Combine(webHost.ContentRootPath, "assets", "images", "movies");
		}

		/// <summary>
		/// Add new movie with save movie image cover
		/// </summary>
		/// <returns>
		/// Return Ok/BadRequest 
		/// if response is Ok then return an instance of
		/// ApplicationServiceResult<MovieItemDtoModel>
		/// </returns>
		[HttpPost("add")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> CreateMovie()
		{
			var appResult = _resultSelector.GetSingleResult<MovieItemDtoModel?>();
			string serializedResult = string.Empty;

			CrudMovieDtoModel? movieDtoModel = GetModelFromRequestAsync();
			if (movieDtoModel is null)
				return ReturnMovieActionResult(
					appResult,
					Common.ModelState.create,
					HttpStatusCode.BadRequest);

			IReadOnlyList<string> modelErrors = await ValidateModel(movieDtoModel, Common.ModelState.create);
			if (modelErrors is not null && modelErrors.Any())
				return ReturnMovieActionResult(
					appResult,
					Common.ModelState.create,
					HttpStatusCode.BadRequest,
					modelErrors);

			if (movieDtoModel.File is null || movieDtoModel.File.Length == 0)
			{
				appResult.AddError("فایلی برای کاور تصویر فیلم انتخاب نشده است", HttpStatusCode.BadRequest);
				serializedResult = JsonConvert.SerializeObject(appResult, serializerSettins);
				return BadRequest(serializedResult);
			}

			string? imageFileName = await movieDtoModel.File.UploadImageAsync(movieDtoModel.Title, _webHost, _moviesImageDirPath);
			if (string.IsNullOrEmpty(imageFileName))
			{
				appResult.AddError("تصویری برای کاور فیلم انتخاب نشده است", HttpStatusCode.BadRequest);
				serializedResult = JsonConvert.SerializeObject(appResult, serializerSettins);
				return BadRequest(serializedResult);
			}

			movieDtoModel.ImageName = imageFileName;
			Movie movie = _mapper.Map<Movie>(movieDtoModel);
			Movie? createdMovie = await _repository.CreateEntityAsync(movie);

			return ReturnMovieResponse(
				appResult,
				createdMovie,
				Common.ModelState.create);
		}

		/// <summary>
		/// Edit movie and update movie tags and image
		/// </summary>
		/// <returns>MovieItemDtoModel</returns>
		[HttpPut("edit")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Put()
		{
			var appResult = _resultSelector.GetSingleResult<MovieItemDtoModel?>();
			CrudMovieDtoModel? movieDtoModel = GetModelFromRequestAsync();

			if (movieDtoModel is null)
				return ReturnMovieActionResult(
					appResult,
					Common.ModelState.update,
					HttpStatusCode.BadRequest);

			IReadOnlyList<string> modelErrors = await ValidateModel(movieDtoModel);
			if (modelErrors.Any() && modelErrors.Count > 0)
				return ReturnMovieActionResult(
					appResult,
					Common.ModelState.update,
					HttpStatusCode.BadRequest,
					modelErrors);

			// change image in edit movie
			if (movieDtoModel.Id > 0 &&
				movieDtoModel.File is not null &&
				movieDtoModel.File.Length > 0)
			{
				string? newFileName = await movieDtoModel.File.EditImageAsync(movieDtoModel.ImageName, movieDtoModel.Title, _webHost, _moviesImageDirPath);
				movieDtoModel.ImageName = !string.IsNullOrEmpty(newFileName) ? newFileName : movieDtoModel.ImageName;
			}

			Movie movie = _mapper.Map<Movie>(movieDtoModel);
			Movie? updatedMovie = await _repository.UpdateEntityAsync(movie);

			return ReturnMovieResponse(appResult, movie, Common.ModelState.update);
		}

		/// <summary>
		///  Override the BaseApiController delete method
		/// </summary>
		/// <param name="id">Movie Id</param>
		/// <returns>
		///		Return Ok/NotFound/BadRequest response codes
		///		if movie has been deleted successfully return 
		///		an instance of ApplicationServiceResult<MovieItemDtoModel>
		///		else return instance of ApplicationServiceResult<MovieItemDtoModel>
		///		with null result
		/// </returns>
		[HttpDelete("{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationServiceResult<MovieItemDtoModel>))]

		[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApplicationServiceResult<MovieItemDtoModel>))]

		[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApplicationServiceResult<MovieItemDtoModel>))]
		public override async Task<IActionResult> Delete(long? id)
		{
			var appResult = _resultSelector.GetSingleResult<MovieItemDtoModel?>();

			Movie? movie = await _movieRepository.GetMovieByIdWithTagsListAsync(id ?? 0);
			if (movie is null)
				return ReturnMovieActionResult(
					appResult,
					Common.ModelState.delete,
					HttpStatusCode.NotFound);

			if (!string.IsNullOrEmpty(movie.ImageName))
				movie.ImageName.DeleteImage(_webHost, "movies");

			Movie? deletedMovie = await _repository.DeleteEntityAsync(movie);
			return ReturnMovieResponse(appResult, deletedMovie, Common.ModelState.delete);
		}


		[HttpGet("thumbnail/list")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationServiceResult<IEnumerable<MovieItemWithGenreTitle>>))]
		public async Task<IActionResult> GetMovieThumbnailList()
		{
			IEnumerable<MovieItemWithGenreTitle> movieItemsList = await _movieRepository.GetMovieItemsWithGenreTitle();
			var movieItemDtoModelList = _mapper.Map<IEnumerable<MovieItemWithGenreTitleDtoModel>>(movieItemsList);

			var appResult = _resultSelector.GetResultList(movieItemDtoModelList);

			return Ok(appResult);
		}

		/// <summary>
		/// Find movie by id
		/// </summary>
		/// <param name="id">Movie id and must send long parameter as id</param>
		/// <returns>Returns Movies List with genre title for every movie</returns>
		[HttpGet("findmovie/{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationServiceResult<CrudMovieDtoModel?>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetMovieByIdWithTagsListForCrud(long? id)
		{
			var appResult = _resultSelector.GetSingleResult<CrudMovieDtoModel?>();
			if (id is null or < 0)
				return ReturnMovieActionResult(
					appResult,
					Common.ModelState.read,
					HttpStatusCode.BadRequest);

			Movie? movie = await _movieRepository.GetMovieByIdWithTagsListAsync(id ?? 0);
			if (movie is null)
				return ReturnMovieActionResult(
					appResult,
					Common.ModelState.read,
					HttpStatusCode.NotFound);

			return ReturnMovieResponse(
				appResult,
				movie,
				Common.ModelState.read);
		}


		[HttpGet("displaymovie/{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationServiceResult<DeleteMovieDtoModel>))]
		public async Task<IActionResult> GetMovieByIdWithTagsListForDisplay(long? id)
		{
			var appResult = _resultSelector.GetSingleResult<DeleteMovieDtoModel?>();
			if (id is null or <= 0)
				return ReturnMovieActionResult(
					appResult,
					Common.ModelState.read,
					HttpStatusCode.BadRequest);

			Movie? movie = await _movieRepository.GetMovieByIdWithTagsListAsync(id ?? 0);
			if (movie is null)
				return ReturnMovieActionResult(
					appResult,
					Common.ModelState.read,
					HttpStatusCode.NotFound);

			return ReturnMovieResponse(
				appResult,
				movie,
				Common.ModelState.read);
		}


		/// <summary>
		/// Reading movie list items for display in landing page sidebar
		/// </summary>
		/// <returns>Return Movie list</returns>
		[HttpGet("sidebarlist")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationServiceResult<IEnumerable<MovieItemDtoModel>>))]
		public async Task<IActionResult> GetMoviesListAsSideBar()
		{
			IEnumerable<MovieProjectModel> movies = await _movieRepository.GetMoviesSidebarListAsync(a => a.OrderByDescending(d => d.Id));

			var moviesDtoModelList = _mapper.Map<IEnumerable<MovieProjectDtoModel>>(movies);
			var appResult = _resultSelector.GetResultList(moviesDtoModelList);
			return Ok(appResult);
		}

		/// <summary>
		/// Get Movie Project model for getting summerization version of the complete movie model
		/// </summary>
		/// <param name="id">Movie Id to find</param>
		/// <returns>Return MovieProjectDtoModel instance</returns>
		[HttpGet("projectmovie/{id?}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationServiceResult<MovieProjectDtoModel>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetById(long? id)
		{
			var appResult = _resultSelector.GetSingleResult<MovieProjectDtoModel?>();
			if (id is null or <= 0)
				return ReturnModelActionResult<MovieProjectDtoModel>(
					appResult,
					Common.ModelState.read,
					HttpStatusCode.BadRequest);

			MovieProjectModel? movieProjectModel = await _movieRepository.GetMovieProjectModelAsync(id ?? 0); 

			if(movieProjectModel is null)
				return ReturnModelActionResult<MovieProjectDtoModel>(
					appResult,
					Common.ModelState.read,
					HttpStatusCode.NotFound);

			var movieProjectDtoModel =  _mapper.Map<MovieProjectDtoModel>(movieProjectModel);
			appResult.AddResult(movieProjectDtoModel);
			return Ok(appResult);
		}


		private IActionResult ReturnMovieResponse<TMovieDtoModel>(
			ApplicationServiceResult<TMovieDtoModel?> appResult,
			Movie? movie,
			ModelState modelState)
			where TMovieDtoModel : BaseDtoModel
		{
			string serializedResult = string.Empty;

			if (movie is null)
				return ReturnMovieActionResult(
						appResult,
						modelState,
						HttpStatusCode.BadRequest);

			var movieItemDtoItem = _mapper.Map<TMovieDtoModel>(movie);
			appResult.AddResult(movieItemDtoItem);

			return ReturnMovieActionResult(
				appResult,
				modelState,
				HttpStatusCode.OK);
		}

		private IActionResult ReturnMovieActionResult<TMovieDtoMovel>(
			ApplicationServiceResult<TMovieDtoMovel?> appResult,
			ModelState modelState,
			HttpStatusCode statusCode,
			IReadOnlyList<string>? errorsList = null)
			where TMovieDtoMovel : BaseDtoModel
		{
			_modelStateArgs.SetModelMessage(modelState, statusCode, errorsList);
			if (statusCode is not HttpStatusCode.OK)
				appResult.AddError(_modelStateArgs.Message, statusCode);
			else
				appResult.AddMessage(_modelStateArgs.Message);
			string serializedResult = JsonConvert.SerializeObject(appResult);
			return statusCode is HttpStatusCode.OK ? Ok(serializedResult) : BadRequest(appResult);
		}

		private CrudMovieDtoModel? GetModelFromRequestAsync()
		{
			if (Request.Form is null ||
				Request.Form.Keys.Count == 0 ||
				!Request.HasFormContentType)
				return default;

			CrudMovieDtoModel movieDtoModel = new();

			// assign movie id
			string stringId = Request.Form?[nameof(movieDtoModel.Id)].ToString() ?? string.Empty;
			movieDtoModel.Id = long.TryParse(stringId, CultureInfo.InvariantCulture, out long id) ? id : 0;

			// assign movie title
			movieDtoModel.Title = Request.Form?[nameof(movieDtoModel.Title)].ToString() ?? string.Empty;

			// assign movie descriotion
			movieDtoModel.Description = Request.Form?[nameof(movieDtoModel.Description)].ToString() ?? string.Empty;

			// assign movie image name
			movieDtoModel.ImageName = Request.Form?[nameof(movieDtoModel.ImageName)].ToString() ?? string.Empty;

			// assing movie score
			movieDtoModel.ScoreString = Request.Form?[nameof(movieDtoModel.ScoreString)].ToString() ?? string.Empty;
			movieDtoModel.Score = decimal.TryParse(movieDtoModel.ScoreString, CultureInfo.InvariantCulture, out decimal score) ? score : 0;

			// assign movie genreID
			string genreIdString = Request.Form?[nameof(movieDtoModel.GenreId)].ToString() ?? string.Empty;
			movieDtoModel.GenreId = long.TryParse(genreIdString, CultureInfo.InvariantCulture, out long genreId) ? genreId : 0;

			// assign movie tags list
			movieDtoModel.TagsList = Request.Form?[nameof(movieDtoModel.TagsList)].ToString() ?? string.Empty;
			if(!string.IsNullOrEmpty(movieDtoModel.TagsList))
				movieDtoModel.Tags = JsonConvert.DeserializeObject<List<TagItemDtoModel>>(movieDtoModel.TagsList); 


			string startDate = Request.Form?["StartDateViewModel"].ToString() ?? string.Empty;
			string endDate = Request.Form?["EndDateViewModel"].ToString() ?? string.Empty;

			// assign movie start/end date display
			movieDtoModel.StateDateDispaly = startDate.ToGregorianDate();
			movieDtoModel.EndDateDisplay = endDate.ToGregorianDate();

			if (Request.Form?.Files?.Count > 0)
				movieDtoModel.File = Request.Form?.Files[0];

			return movieDtoModel;
		}
	}
}
