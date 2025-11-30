using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.Controllers;
using System.Net;

namespace _06_WebApp_RazoePage.WebApi.DtoModels.Tickets
{
	public class TicketModelArgs : IModelStateArgs<TicketController>
	{
		private string _message = string.Empty;

		private List<string> _messagesList = [];

		public string Message => _message;

		public IReadOnlyList<string> MessagesList => _messagesList;

		public void SetModelMessage(
			ModelState modelState,
			HttpStatusCode statusCode,
			IReadOnlyList<string>? messages = null)
		{
			if (messages is not null &&
				messages.Count > 0 &&
				statusCode is HttpStatusCode.BadRequest)
			{
				_messagesList.AddRange(messages);
				return;
			}
			switch (modelState)
			{
				case ModelState.create when statusCode is HttpStatusCode.OK:
					_message = "بلیت جدید با موفقیت ذخیره گردید";
					break;

				case ModelState.create when statusCode is HttpStatusCode.BadRequest:
					_message = "خطا در ثبت بلیت جدید";
					break;

				case ModelState.read
					when statusCode is HttpStatusCode.BadRequest ||
						 statusCode is HttpStatusCode.NotFound:
					_message = "بلیتی یافت نشد";
					break;

				case ModelState.read when statusCode is HttpStatusCode.OK:
					_message = "بلیت مورد با موفقیت واکشی شد";
					break;

				case ModelState.update when statusCode is HttpStatusCode.OK:
					_message = "بلیت مورد نظر با موفقیت ویرایش گردید";
					break;

				case ModelState.update when statusCode is HttpStatusCode.BadRequest:
					_message = "خطا در ویرایش بلیت";
					break;

				case ModelState.update when statusCode is HttpStatusCode.NotFound:
					_message = "بلیتی یافت نشد";
					break;

				case ModelState.delete when statusCode is HttpStatusCode.OK:
					_message = "بلیت با موفقیت حذف گردید";
					break;

				case ModelState.delete when statusCode is HttpStatusCode.BadRequest:
					_message = "خطا در حذف بلیت";
					break;

				case ModelState.delete when statusCode is HttpStatusCode.NotFound:
					_message = "بلیتی یافت نشد";
					break;

				default:
					_message = "داده ای یافت نشد";
					break;

			}
		}
	}
}
