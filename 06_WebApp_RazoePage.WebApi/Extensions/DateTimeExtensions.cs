using System.Globalization;

namespace _06_WebApp_RazoePage.WebApi.Extensions
{
	public static class DateTimeExtensions
	{
		public static DateTime ToGregorianDate(this string? persianDate)
		{
			if(string.IsNullOrWhiteSpace(persianDate)) return DateTime.Now;

			PersianCalendar pc = new PersianCalendar();
			string[] persianDateSections = persianDate.Split('/', StringSplitOptions.RemoveEmptyEntries);
			if (persianDateSections.Length < 3) return DateTime.Now;

			DateTime greforianDate = new DateTime(
									int.Parse(persianDateSections[0]),
									int.Parse(persianDateSections[1]),
									int.Parse(persianDateSections[2]),
									pc);

			return greforianDate;
		}
	}
}
