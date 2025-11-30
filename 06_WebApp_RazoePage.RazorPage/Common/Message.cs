namespace _06_WebApp_RazoePage.RazorPage.Common
{
	public class Message
	{
		private static List<string> _messages;

		static Message() => _messages = [];

		public static IReadOnlyList<string> Messages => _messages;

		public void AddMessage(string? message)
		{
			if (!string.IsNullOrWhiteSpace(message))
				_messages.Add(message);
		}

		public static void ClearMessages() => _messages.Clear();
	}
}
