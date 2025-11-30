using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.Controllers;
using System.Net;

namespace _06_WebApp_RazoePage.WebApi.DtoModels.Tags
{
	public class TagModelArgs : IModelStateArgs<TagController>
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
					_message = "تگ جدید با موفقیت ذخیره گردید";
					break;

				case ModelState.create when statusCode is HttpStatusCode.BadRequest:
					_message = "خطا در ثبت تگ جدید";
					break;

				case ModelState.read
					when statusCode is HttpStatusCode.BadRequest ||
						 statusCode is HttpStatusCode.NotFound:
					_message = "تگی یافت نشد";
					break;

				case ModelState.read when statusCode is HttpStatusCode.OK:
					_message = "تگ مورد با موفقیت واکشی شد";
					break;

				case ModelState.update when statusCode is HttpStatusCode.OK:
					_message = "تگ مورد نظر با موفقیت ویرایش گردید";
					break;

				case ModelState.update when statusCode is HttpStatusCode.BadRequest:
					_message = "خطا در ویرایش تگ";
					break;

				case ModelState.update when statusCode is HttpStatusCode.NotFound:
					_message = "تگی یافت نشد";
					break;

				case ModelState.delete when statusCode is HttpStatusCode.OK:
					_message = "تگ فیلم با موفقیت حذف گردید";
					break;

				case ModelState.delete when statusCode is HttpStatusCode.BadRequest:
					_message = "خطا در حذف تگ";
					break;

				case ModelState.delete when statusCode is HttpStatusCode.NotFound:
					_message = "تگی یافت نشد";
					break;

				default:
					_message = "داده ای یافت نشد";
					break;

			}
		}
	}
}
