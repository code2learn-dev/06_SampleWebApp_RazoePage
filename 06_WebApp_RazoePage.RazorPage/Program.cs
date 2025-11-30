using _06_WebApp_RazoePage.RazorPage.Common;
using _06_WebApp_RazoePage.RazorPage.Filters;
using _06_WebApp_RazoePage.RazorPage.ViewComponents;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
	options.Conventions.ConfigureFilter(new SetActivePageAttribute());
});
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient("cafemovie", options =>
{
	options.BaseAddress = new Uri("https://localhost:7249/");
});
builder.Services.AddAutoMapper(options => options.AddProfile<RazoePageMappingProfile>());

builder.Services.AddScoped<IBaseViewComponent, BaseViewComponents>();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.MapRazorPages().Add(c => ((RouteEndpointBuilder)c).Order=1);
app.MapControllerRoute(
	name: "Default",
	pattern: "{controller}/{action}/{id?}",
	defaults: new { controller = "Home", action = "Index" });

app.Run();
