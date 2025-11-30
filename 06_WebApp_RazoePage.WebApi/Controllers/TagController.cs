using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tags;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace _06_WebApp_RazoePage.WebApi.Controllers
{
	public class TagController : BaseApiController<
		Tag,
		TagItemDtoModel,
		CrudTagDtoModel,
		TagItemDtoModel,
		TagController>
	{
		public TagController(
			ITagRepository repository, 
			IMapper mapper, 
			IApplicationServiceResultSelector resultSelector, 
			IModelStateArgs<TagController> modelStateArgs, 
			IValidator<CrudTagDtoModel> validator) 
			: base(repository, mapper, resultSelector, modelStateArgs, validator)
		{
		}

		[HttpGet("filter/{tagName?}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationServiceResult<IEnumerable<TagItemDtoModel>>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> FilterTagsByName(string? tagName)
		{
			var appResult = _resultSelector.GetResultList<TagItemDtoModel>();
			if(string.IsNullOrWhiteSpace(tagName))
			{
				appResult.AddError("تگی یافت نشد", System.Net.HttpStatusCode.BadRequest);
				return BadRequest(appResult);
			}

			Expression<Func<Tag, bool>> predicate = t => t.Name.Contains(tagName);

			IEnumerable<Tag> filteredTags = await _repository.FilterByPredicate(predicate, q => q.OrderBy(a => a.Name));

			var tagListDtoModel = _mapper.Map<IEnumerable<TagItemDtoModel>>(filteredTags);
			appResult.AddResult(tagListDtoModel);

			return Ok(appResult);
		}
	}
}
