using _06_WebApp_RazoePage.Data.ProjectionModels;
using _06_WebApp_RazoePage.WebApi.DtoModels.Customers;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tags;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tickets;
using AutoMapper;

namespace _06_WebApp_RazoePage.WebApi.Common
{
	public class ApiMappingProfile : Profile
	{
		public ApiMappingProfile()
		{
			// genre mappings profiles
			CreateMap<Genre, GenreDtoModel>();
			CreateMap<Genre, CrudGenreDtoModel>();
			CreateMap<CrudGenreDtoModel, Genre>();

			// create movie mapping profiles
			CreateMap<Movie, CrudMovieDtoModel>();
			CreateMap<MovieProjectModel, MovieProjectDtoModel>();
			CreateMap<CrudMovieDtoModel, Movie>();
			CreateMap<CrudMovieDtoModel, DeleteMovieDtoModel>();
			CreateMap<Movie, MovieItemDtoModel>();
			CreateMap<Movie, DeleteMovieDtoModel>();
			CreateMap<MovieItemWithGenreTitle, MovieItemWithGenreTitleDtoModel>();

			// create tag mapping
			CreateMap<CrudTagDtoModel, Tag>();
			CreateMap<Tag, TagItemDtoModel>();
			CreateMap<TagItemDtoModel, Tag>();

			// create customer mappers
			CreateMap<CrudCustomerDtoModel, Customer>();
			CreateMap<Customer, CustomerItemDtoModel>();
			CreateMap<Customer, CrudCustomerDtoModel>();
			CreateMap<DeleteCustomerDtoModel, Customer>();
			CreateMap<Customer, DeleteCustomerDtoModel>();
			CreateMap<CustomerProjectModel, CustomerProjectDtoModel>();

			// create ticket mappers
			CreateMap<Ticket, TicketItemDtoModel>();
			CreateMap<CrudTicketDtoItem, Ticket>();
			CreateMap<TicketListModel, TicketListDtoModel>();
			CreateMap<TicketProjecttionModel, TicketProjectionDtoModel>();
			CreateMap<DeleteTicketDtoItem, Ticket>();
		}
	}
}
