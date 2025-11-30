using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace _06_WebApp_RazoePage.RazorPage.Extensions
{
	public static class ModelStateExtensions
	{
		public static IReadOnlyList<string> MapModelStateToErrorsList(
			this ModelStateDictionary? modelState)
		{
			if (modelState is null || modelState.IsValid) return [];

			return modelState.Values.SelectMany(a => a.Errors).Select(e => e.ErrorMessage).ToList();
		}
	}
}
