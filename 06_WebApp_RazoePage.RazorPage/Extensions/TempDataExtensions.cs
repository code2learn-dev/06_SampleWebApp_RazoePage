using _06_WebApp_RazoePage.RazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace _06_WebApp_RazoePage.RazorPage.Extensions
{
	public static class TempDataExtensions
	{
		public static void MapToTempData<TData>(
			this ITempDataDictionary? tempData, 
			TData? data,
			string key)
		{
			if (tempData is null || data is null) return;

			string serializedData = JsonConvert.SerializeObject(data);
			tempData[key] = serializedData;
		}

		public static TData? MapToTargetData<TData>(
			this ITempDataDictionary? tempData,
			string key)
			where TData : BaseViewModel
		{
			if (tempData is null) return null;

			string? tempValue = tempData.TryGetValue(key, out object? temp) ? temp.ToString() : string.Empty;
			if (string.IsNullOrEmpty(tempValue)) return null;

			return JsonConvert.DeserializeObject<TData>(tempValue);
		}
	}
}
