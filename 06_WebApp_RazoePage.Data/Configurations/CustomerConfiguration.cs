using _06_WebApp_RazoePage.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _06_WebApp_RazoePage.Data.Configurations
{
	public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> builder)
		{
			builder.HasMany(a => a.Tickets)
				.WithOne()
				.HasForeignKey(f => f.CustomerId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Property(a => a.NationalCode)
				.HasPrecision(10, 0);

			builder.Property(a => a.PhoneNumber)
				.HasPrecision(11, 0);
		}
	}
}
