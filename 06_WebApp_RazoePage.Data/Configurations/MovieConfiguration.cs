using _06_WebApp_RazoePage.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _06_WebApp_RazoePage.Data.Configurations
{
	public class MovieConfiguration : IEntityTypeConfiguration<Movie>
	{
		public void Configure(EntityTypeBuilder<Movie> builder)
		{
			builder.HasMany(m => m.Tags)
				.WithMany(t => t.Movies)
				.UsingEntity<MovieTag>(
					t => t.HasOne(d => d.Tag)
					.WithMany()
					.HasForeignKey(f => f.TagId)
					.OnDelete(DeleteBehavior.NoAction),

					a => a.HasOne(d => d.Movie)
					.WithMany()
					.HasForeignKey(f => f.MovieId)
					.OnDelete(DeleteBehavior.Cascade),

					p => p.HasKey(k => new { k.MovieId, k.TagId })
					
				);

			builder.HasMany(m => m.Tickets)
				.WithOne()
				.HasForeignKey(f => f.MovieId)
				.OnDelete(DeleteBehavior.NoAction);

			builder.Property(a => a.Score)
				.HasPrecision(2, 1);
		}
	}
}
