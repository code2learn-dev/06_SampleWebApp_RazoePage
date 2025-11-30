using _06_WebApp_RazoePage.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _06_WebApp_RazoePage.Data.Configurations
{
	public class GenreConfiguration : IEntityTypeConfiguration<Genre>
	{
		public void Configure(EntityTypeBuilder<Genre> builder)
		{
			builder.HasMany(a => a.Movies)
				.WithOne()
				.HasForeignKey(f => f.GenreId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
