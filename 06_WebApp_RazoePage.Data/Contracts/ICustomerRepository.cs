using _06_WebApp_RazoePage.Data.Models;
using _06_WebApp_RazoePage.Data.ProjectionModels;

namespace _06_WebApp_RazoePage.Data.Contracts
{
	public interface ICustomerRepository : IGenericRepository<Customer>
	{
		Task<Customer?> GetCustomerByIdIncludeTicketsAsync(long id);
		Task<CustomerProjectModel?> GetCustomerProjectByIdAsync(long id);
		Task<IEnumerable<CustomerProjectModel>> GetCustomersProjectModelListAsync(Func<IQueryable<Customer>, IOrderedQueryable<Customer>>? orderFilter = null);
	}
}
