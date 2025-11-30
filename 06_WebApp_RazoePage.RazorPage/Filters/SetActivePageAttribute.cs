using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace _06_WebApp_RazoePage.RazorPage.Filters
{
	public class SetActivePageAttribute : IAsyncPageFilter
	{
		public async Task OnPageHandlerExecutionAsync(
			PageHandlerExecutingContext context, 
			PageHandlerExecutionDelegate next)
		{
			var pagePath = context.ActionDescriptor.ViewEnginePath; 

			if(pagePath != null)
			{
				var pagePathSegments = pagePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
				var pageName = pagePathSegments.Length > 0 ? pagePathSegments[0] : string.Empty;
				Console.WriteLine($"Page Name: {pageName}");
				context.HttpContext.Items["ActivePage"] = pageName;
			}

			await next();
		}

		public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
		{
			return Task.CompletedTask;
		}
	}
}
