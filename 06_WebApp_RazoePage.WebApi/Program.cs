using _06_WebApp_RazoePage.Data.Contracts;
using _06_WebApp_RazoePage.Data.Repositories;
using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using FluentValidation;
using _06_WebApp_RazoePage.WebApi.DtoModels.Movies;
using Microsoft.AspNetCore.Mvc;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tags;
using _06_WebApp_RazoePage.WebApi.DtoModels.Customers;
using _06_WebApp_RazoePage.WebApi.DtoModels.Tickets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OnlineCinemaDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("cinema_db"));
});
builder.Services.AddControllers().AddNewtonsoftJson()
	.AddXmlDataContractSerializerFormatters();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile(typeof(ApiMappingProfile)));
builder.Services.AddValidatorsFromAssemblyContaining<CrudGenreDtoModel>();
builder.Services.AddCors(options =>
{
	options.AddPolicy("MoviePolicy", 
		policy =>
		{
			policy.WithOrigins("https://localhost:7081")
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
});

// handle null values and reference loop handling with asp.net core mvc jsonoptions
//builder.Services.Configure<JsonOptions>(options =>
//{
//	options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

//	options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
//});

builder.Services.Configure<MvcNewtonsoftJsonOptions>(options =>
{
	options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
	options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// configure web api to accept any format from client
builder.Services.Configure<MvcOptions>(options =>
{
	options.RespectBrowserAcceptHeader = true;
	options.ReturnHttpNotAcceptable = true;
});

// another approach for adding Fluent validation with Scrutor
//builder.Services.Scan(a =>
//		a.FromAssemblyOf<CrudGenreDtoModel>()
//		.AddClasses(c => c.AssignableTo(typeof(IValidator)))
//		.AsImplementedInterfaces()
//		.WithScopedLifetime());

builder.Services.AddScoped<IApplicationServiceResultSelector, ApplicationServiceResultSelector>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

builder.Services.AddScoped<IModelStateArgs<GenreController>, GenreModelArgs>();
builder.Services.AddScoped<IModelStateArgs<MovieController>, MovieModelStateArgs>();
builder.Services.AddScoped<IModelStateArgs<TagController>, TagModelArgs>();
builder.Services.AddScoped<IModelStateArgs<CustomerController>, CustomerModelArgs>();
builder.Services.AddScoped<IModelStateArgs<TicketController>, TicketModelArgs>();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseStatusCodePages();

	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
		options.RoutePrefix = string.Empty;
	});
}

app.UseStaticFiles(new StaticFileOptions()
{
	FileProvider = new PhysicalFileProvider(
		Path.Combine(app.Environment.ContentRootPath, "assets"))
});

app.UseCors("MoviePolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();

