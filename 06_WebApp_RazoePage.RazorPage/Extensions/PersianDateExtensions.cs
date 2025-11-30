using System.Globalization;
using System.Numerics;

namespace _06_WebApp_RazoePage.RazorPage.Extensions
{
	public static class PersianDateExtensions
	{
		public static string MapToPersianDate(this DateTime date)
		{ 
			PersianCalendar pc = new PersianCalendar();

			return $"{pc.GetYear(date)}/{pc.GetMonth(date)}/{pc.GetDayOfMonth(date)}";
		}

		public static DateTime MapToGregorianDate(this string? persianDate)
		{
			if (string.IsNullOrEmpty(persianDate)) return DateTime.Now;

			PersianCalendar pc = new PersianCalendar();
			string[] persianDateSsections = persianDate.Split('/', StringSplitOptions.RemoveEmptyEntries);
			if (persianDateSsections.Length < 3) return DateTime.Now;

			int year = int.TryParse(persianDateSsections[0], out int y) ? y : 0;
			int month = int.TryParse(persianDateSsections[1], out int m) ? m : 0;	
			int day = int.TryParse(persianDateSsections[2], out int d) ? d : 0;

			return new DateTime(year, month, day, pc).Date;
		}
	}
}
