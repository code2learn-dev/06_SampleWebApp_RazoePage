using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.Data.Models;
using _06_WebApp_RazoePage.Data.ProjectionModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace _06_WebApp_RazoePage.Data.Repositories
{
	public class CustomerRepository
		: GenericRepository<Customer>,
		  ICustomerRepository
	{
		public CustomerRepository(
			OnlineCinemaDbContext dbContext, 
			ILogger<GenericRepository<Customer>> logger) : base(dbContext, logger)
		{
		}

		public async Task<IEnumerable<CustomerProjectModel>> GetCustomersProjectModelListAsync(
			Func<IQueryable<Customer>, IOrderedQueryable<Customer>>? orderFilter = null)
		{
			IQueryable<Customer> query = _dbSet;

			query = orderFilter != null
				? orderFilter(query)
				: query.OrderBy(a => a.Id);

			IQueryable<CustomerProjectModel> customerProjectModels = query.Select(a => new CustomerProjectModel()
			{
				Id = a.Id,
				FirstName = a.FirstName,
				LastName = a.LastName,
				NationalCode = a.NationalCode
			});

			return await customerProjectModels.ToListAsync();
		}

		public async Task<CustomerProjectModel?> GetCustomerProjectByIdAsync(long id)
		{
			Customer? customer = await _dbSet.Where(a => a.Id == id).FirstOrDefaultAsync();
			return customer != null
				? new CustomerProjectModel()
				{
					Id = customer.Id,
					FirstName = customer.FirstName,
					LastName = customer.LastName,
					NationalCode = customer.NationalCode
				}
				: default;
		}

		public async Task<Customer?> GetCustomerByIdIncludeTicketsAsync(long id) 
			=> await _dbSet.Include(a => a.Tickets).FirstOrDefaultAsync(a => a.Id == id); 
	}
}
