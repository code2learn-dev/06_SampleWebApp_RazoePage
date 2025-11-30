using _06_WebApp_RazoePage.Data.Models;
using _06_WebApp_RazoePage.Data.ProjectionModels;

namespace _06_WebApp_RazoePage.Data.Contracts
{
	public interface ITicketRepository : IGenericRepository<Ticket>
	{
		Task<TicketProjecttionModel?> GetTicketDetailsByIdAsync(long id);
		Task<IEnumerable<TicketListModel>> GetTicketListModelAsync();
	}
}
