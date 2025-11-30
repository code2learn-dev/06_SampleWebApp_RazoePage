using _06_WebApp_RazoePage.Data.Models;
using _06_WebApp_RazoePage.RazorPage.Extensions;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Customers;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Genres;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Moveis;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Taggs;
using _06_WebApp_RazoePage.RazorPage.ViewModels.Tickets;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.DtoModels.Customers;
using _06_WebApp_RazoePage.WebApi.DtoModels.Genres;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tags;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tickets;
using AutoMapper;
using Newtonsoft.Json;
using System.Globalization; 

namespace _06_WebApp_RazoePage.RazorPage.Common
{
	public class RazoePageMappingProfile : Profile
	{
		private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
		{
			ContractResolver = new PrivateSetterContractResolver(),
			TypeNameHandling = TypeNameHandling.Auto,
			TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
			Formatting = Formatting.Indented
		};

		public RazoePageMappingProfile()
		{
			// create genre mappers
			CreateMap<GenreDtoModel, GenreItemViewModel>();
			CreateMap<GenreItemViewModel, GenreDtoModel>();
			CreateMap<CrudGenreDtoModel, CrudGenreViewModel>();
			CreateMap<CrudGenreViewModel, CrudGenreDtoModel>();


			// create movie mappers
			CreateMap<MovieItemDtoModel, MovieItemViewModel>();
			CreateMap<CrudMovieDtoModel, CrudMovieViewModel>()
				.ForMember(dto => dto.ScoreString, 
						opt => opt.MapFrom(
						src => src.Score.ToString(CultureInfo.InvariantCulture)
						.Replace('/', '.')))
				.ForMember(dto => dto.StartDateViewModel,
							opt => opt.MapFrom(
							src => src.StateDateDispaly.MapToPersianDate()))
				.ForMember(dto => dto.EndDateViewModel,
							opt => opt.MapFrom(
							src => src.EndDateDisplay.MapToPersianDate()))
				.ForMember(dto => dto.TagsList,
							opt => opt.MapFrom(
							src => JsonConvert.SerializeObject(src.Tags, serializerSettings)));

			CreateMap<CrudMovieViewModel, CrudMovieDtoModel>();
			CreateMap<DeleteMovieViewModel, DeleteMovieDtoModel>();

			CreateMap<DeleteMovieDtoModel, DeleteMovieViewModel>()
				.ForMember(dto => dto.ScoreString,
							opt => opt.MapFrom(
							src => src.Score.ToString(CultureInfo.InvariantCulture)
							.Replace('/', '.')))
				.ForMember(dto => dto.StartDateViewModel,
							opt => opt.MapFrom(
							src => src.StateDateDispaly.MapToPersianDate()))
				.ForMember(dto => dto.EndDateViewModel,
							opt => opt.MapFrom(
							src => src.EndDateDisplay.MapToPersianDate()))
				.ForMember(dto => dto.TagsList,
							opt => opt.MapFrom(
							src => JsonConvert.SerializeObject(src.Tags, serializerSettings)));

			CreateMap<MovieItemWithGenreTitleDtoModel, MovieItemWithGenreTitleViewModel>();
			CreateMap<MovieProjectDtoModel, MovieProjectViewModel>();


			// tag mappers
			CreateMap<CrudTagViewModel, CrudTagDtoModel>();
			CreateMap<TagItemDtoModel, TagItemViewModel>();

			// customer mappers
			CreateMap<CustomerItemDtoModel, CustomerItemViewModel>();
			CreateMap<CrudCustomerDtoModel, CrudCustomerViewModel>();
			CreateMap<DeleteCustomerDtoModel, DeleteCustomerViewModel>();
			CreateMap<CustomerProjectDtoModel, CustomerProjectViewModel>();

			// ticket mappers
			CreateMap<TicketItemDtoModel, TicketItemViewModel>()
				.ForMember(dto => dto.RegisterDate,
				opt => opt.MapFrom(src => src.RegisterDate.MapToPersianDate()))

				.ForMember(dto => dto.ResevationDate,
				opt => opt.MapFrom(src => src.ResevationDate.MapToPersianDate()));


			CreateMap<CrudTicketViewModel, CrudTicketDtoItem>()
				.ForMember(dto => dto.RegisterDate,
				opt => opt.MapFrom(src => src.RegisterDate.MapToGregorianDate()))

				.ForMember(dto => dto.ResevationDate,
				opt => opt.MapFrom(src => src.ResevationDate.MapToGregorianDate()));

			CreateMap<TicketListDtoModel, TIcketListViewModel>()
				.ForMember(dto => dto.RegisterDate,
				opt => opt.MapFrom(src => src.RegisterDate.MapToPersianDate()))
				
				.ForMember(dto => dto.ResevationDate,
				opt => opt.MapFrom(src => src.ResevationDate.MapToPersianDate()));

			CreateMap<TicketProjectionDtoModel, CrudTicketViewModel>()
				.ForMember(dto => dto.RegisterDate,
				opt => opt.MapFrom(src => src.RegisterDate.MapToPersianDate()))
				.ForMember(dto => dto.ResevationDate,
				opt => opt.MapFrom(src => src.ResevationDate.MapToPersianDate()))
				.ForMember(dto => dto.CustomerName,
				opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

			CreateMap<TicketProjectionDtoModel, DeleteTicketViewModel>()
				.ForMember(dto => dto.RegisterDate,
				opt => opt.MapFrom(src => src.RegisterDate.MapToPersianDate()))
				.ForMember(dto => dto.ResevationDate,
				opt => opt.MapFrom(src => src.ResevationDate.MapToPersianDate()))
				.ForMember(dto => dto.CustomerName,
				opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

			CreateMap<DeleteTicketViewModel, DeleteTicketDtoItem>()
				.ForMember(dto => dto.RegisterDate,
				opt => opt.MapFrom(src => src.RegisterDate.MapToGregorianDate()))
				.ForMember(dto => dto.ResevationDate,
				opt => opt.MapFrom(src => src.ResevationDate.MapToGregorianDate())); 
		}
	}
}

