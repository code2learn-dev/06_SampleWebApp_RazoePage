using System.Net;
using System.Runtime.Serialization;

namespace _06_WebApp_RazoePage.WebApi.Common
{
	[DataContract]
	public abstract class ApplicationServiceResult
	{
		[DataMember]
		private List<string> _errors = [];

		[DataMember]
		private List<string> _messages = [];

		[DataMember]
		private HttpStatusCode _statusCode = HttpStatusCode.OK;

		public IReadOnlyList<string> Errors => _errors;

		public IReadOnlyList<string> Messages => _messages;

		public HttpStatusCode StatusCode => _statusCode;

		public bool IsFailure => _errors.Count > 0;

		public bool IsSuccess => !IsFailure;

		public void AddError(
			string? error, 
			HttpStatusCode statusCode = HttpStatusCode.BadRequest)
		{
			if (!string.IsNullOrWhiteSpace(error))
			{
				_statusCode = statusCode;
				_errors.Add(error);
			}
		}

		public void AddErrorsList(
			IReadOnlyList<string> errors,
			HttpStatusCode statusCode = HttpStatusCode.BadRequest)
		{
			_statusCode = statusCode;
			_errors.AddRange(errors);
		}

		public void AddMessage(string message)
		{
			_statusCode = HttpStatusCode.OK;
			if (!string.IsNullOrWhiteSpace(message))
				_messages.Add(message);
		}

		public void AddMessagesList(IReadOnlyList<string> messages)
		{
			_statusCode = HttpStatusCode.OK;
			_messages.AddRange(messages);
		}

		public void ClearErrors() => _errors.Clear();

		public void ClearMessages() => _messages.Clear();
	}

	[DataContract]
	public class ApplicationServiceResult<TResult> : ApplicationServiceResult
	{
		[DataMember]
		public TResult? Result { get; private set; }

		public void AddResult(TResult result) => Result = result;
	}
}
