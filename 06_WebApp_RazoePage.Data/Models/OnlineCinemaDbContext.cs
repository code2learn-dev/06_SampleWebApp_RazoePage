using _06_WebApp_RazoePage.Data.ProjectionModels;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace _06_WebApp_RazoePage.Data.Models
{
	public class OnlineCinemaDbContext : DbContext
	{
		public DbSet<Genre> Genres { get; set; }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<MovieItemWithGenreTitle> MovieItems { get; set; }
		public DbSet<Ticket> Tickets { get; set; }
		public DbSet<TicketListModel> TicketList { get; set; }
		public DbSet<TicketProjecttionModel> TicketProjection { get; set; }

		public OnlineCinemaDbContext(DbContextOptions<OnlineCinemaDbContext> options) : base(options) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=sample_aspnetcore_razorpage;Integrated Security=True;TrustServerCertificate=True;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(
				Assembly.GetExecutingAssembly(),
				(options) => options.GetInterfaces().Any(c =>
					c.IsGenericType && c.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));

			modelBuilder.Entity<MovieItemWithGenreTitle>()
				.HasNoKey()
				.ToView(null);

			modelBuilder.Entity<TicketListModel>()
				.HasNoKey()
				.ToView(null);

			modelBuilder.Entity<TicketProjecttionModel>()
				.HasNoKey()
				.ToView(null);
		}
	}
}
