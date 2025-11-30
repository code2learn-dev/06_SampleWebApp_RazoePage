using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.Data.Models;
using _06_WebApp_RazoePage.Data.ProjectionModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace _06_WebApp_RazoePage.Data.Repositories
{
	public class TicketRepository
		: GenericRepository<Ticket>,
		  ITicketRepository
	{
		public TicketRepository(
			OnlineCinemaDbContext dbContext, 
			ILogger<GenericRepository<Ticket>> logger) : base(dbContext, logger)
		{
		}

		public async Task<IEnumerable<TicketListModel>> GetTicketListModelAsync()
		{
			List<TicketListModel> ticketList = await _dbContext.Set<TicketListModel>()
				.FromSqlRaw("EXEC dbo.TicketsList").ToListAsync();

			return ticketList;
		}

		public async Task<TicketProjecttionModel?> GetTicketDetailsByIdAsync(long id)
		{
			SqlParameter idParam = new SqlParameter("@id", id); 
			var tickets = await _dbContext.Set<TicketProjecttionModel>()
				.FromSqlRaw($"EXEC dbo.GetTicketById @id", idParam)
				.AsNoTracking()
				.ToListAsync();

			return tickets.FirstOrDefault();
		} 
	}
}
