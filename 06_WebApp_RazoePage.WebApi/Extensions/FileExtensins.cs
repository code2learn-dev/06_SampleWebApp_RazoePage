using System.Data;
using System.Text;

namespace _06_WebApp_RazoePage.WebApi.Extensions
{
	public static class FileExtensins
	{
		private static char[] escapeChars = ['@', ';', ':', '|', '?', '/', '<', '>', '$','^', '=', '#', '~', '`', '+', '&', '*', '\'', '\"', '×', '÷', '!'];

		public static async Task<string?> UploadImageAsync(
			this IFormFile? File,
			string? fileTitle,
			IWebHostEnvironment webHost,
			string imageDirPath)
		{
			if(File is null || string.IsNullOrWhiteSpace(fileTitle)) return default;

			if (File is not null && File.Length == 0) return default;

			string extension = Path.GetExtension(File.FileName);
			string safeTitle = CleanFileNameFromEscapeCharactes(fileTitle);
			string fileName = $"{safeTitle}_{Guid.NewGuid()}{extension}";
			 
			if (!Directory.Exists(imageDirPath))
				Directory.CreateDirectory(imageDirPath);

			string fullFilePath = Path.Combine(imageDirPath, fileName);
			await using Stream stream = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 102400, useAsync: true);
			await File.CopyToAsync(stream);

			return fileName;
		}

		public static async Task<string?> EditImageAsync(
			this IFormFile? File,
			string? oldFileName,
			string? newFileTitle,
			IWebHostEnvironment webHost,
			string imageDirPath)
		{
			if (File is null ||
				string.IsNullOrWhiteSpace(oldFileName) ||
				string.IsNullOrWhiteSpace(newFileTitle))
				return default;

			if (File is not null && File.Length == 0) return default;
			 
			string oldFilePath = Path.Combine(imageDirPath, oldFileName);
			if (System.IO.File.Exists(oldFilePath))
				System.IO.File.Delete(oldFilePath);

			string fileExtension = Path.GetExtension(File.FileName);
			string safeTitle = CleanFileNameFromEscapeCharactes(newFileTitle);
			string newFileName = $"{safeTitle}_{Guid.NewGuid()}{fileExtension}";

			if (!Directory.Exists(imageDirPath))
				Directory.CreateDirectory(imageDirPath);

			string filePath = Path.Combine(imageDirPath, newFileName);
			await using Stream target = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 102400, useAsync: true);
			await File.CopyToAsync(target);

			return newFileName;
		}

		public static void DeleteImage(
			this string? imageName, 
			IWebHostEnvironment webHost,
			string imageDirPath)
		{
			if (string.IsNullOrEmpty(imageName)) return;

			string baseImageDirPath = Path.Combine(webHost.ContentRootPath, "assets", "images", imageDirPath);
			string filePath = Path.Combine(baseImageDirPath, imageName);

			if (File.Exists(filePath))
				File.Delete(filePath);
		}

		private static string CleanFileNameFromEscapeCharactes(string? fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName)) return string.Empty;

			StringBuilder sb = new StringBuilder(fileName.Length); 
			foreach (var ch in fileName)
			{
				if (ch is ' ')
					sb.Append("_");

				if (!escapeChars.Contains(ch))
					sb.Append(ch);
			}

			return sb.ToString();
		}
	}
}
