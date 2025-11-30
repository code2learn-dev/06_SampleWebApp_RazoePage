using _06_WebApp_RazoePage.RazorPage.Common;

namespace _06_WebApp_RazoePage.RazorPage.Extensions
{
	public static class MessageExtensions
	{
		public static void MapToMessages(
			this IReadOnlyList<string>? messages,
			MessageStatus messageStatus = MessageStatus.info)
		{
			if (messages is null || !messages.Any()) return;

			Message message = new();
			string iconCssClass = messageStatus switch
			{
				MessageStatus.danger => "bi-x-octagon",
				MessageStatus.info => "bi-info-circle",
				MessageStatus.success => "bi-check2-circle",
				MessageStatus.warning => "bi-info-circle",
				_ => "bi-info-circle"
			};
			foreach (var messageItem in messages)
			{
				string messageFormat = $"<p class=\"alert bg-{messageStatus} text-white\">" +
					$"<i style=\"margin-left: .5rem;\" class=\"bi {iconCssClass}\"></i>" +
					$"{messageItem}</p>";
				message.AddMessage(messageFormat);
			}
		}

		public static void AddMessage(this string? messageText, MessageStatus messageStatus = MessageStatus.info)
		{
			if (string.IsNullOrWhiteSpace(messageText)) return;

			Message message = new();
			string iconCssClass = messageStatus switch
			{
				MessageStatus.danger => "bi-x-octagon",
				MessageStatus.info => "bi-info-circle",
				MessageStatus.success => "bi-check2-circle",
				MessageStatus.warning => "bi-info-circle",
				_ => "bi-info-circle"
			};
			string messageFormat = $"<p class=\"alert bg-{messageStatus} text-white\" id=\"alert-text\">" +
				$"<i style=\"margin-left: .5rem;\" class=\"bi {iconCssClass}\"></i>" +
				$"{messageText}</p>";
			message.AddMessage(messageFormat);
		}
	}

	public enum MessageStatus : byte
	{
		info = 1,
		success,
		warning,
		danger
	}
}
