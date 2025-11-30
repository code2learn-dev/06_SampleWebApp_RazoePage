using _06_WebApp_RazoePage.WebApi.Common;
using _06_WebApp_RazoePage.WebApi.Controllers;
using Microsoft.OpenApi.Extensions;
using System.Net;

namespace _06_WebApp_RazoePage.WebApi.DtoModels.Genres
{
	public class GenreModelArgs : IModelStateArgs<GenreController>
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
					_message = "دسته جدید با موفقیت ذخیره گردید";
					break;

				case ModelState.create when statusCode is HttpStatusCode.BadRequest:
					_message = "خطا در ثبت دسته جدید";
					break;

				case ModelState.read
					when statusCode is HttpStatusCode.BadRequest ||
						 statusCode is HttpStatusCode.NotFound:
					_message = "دسته بندی یافت نشد";
					break;

				case ModelState.read when statusCode is HttpStatusCode.OK:
					_message = "دسته بندی مورد با موفقیت واکشی شد";
					break;

				case ModelState.update when statusCode is HttpStatusCode.OK:
					_message = "دسته بندی مورد نظر با موفقیت ویرایش گردید";
					break;

				case ModelState.update when statusCode is HttpStatusCode.BadRequest:
					_message = "خطا در ویرایش دسته بندی";
					break;

				case ModelState.update when statusCode is HttpStatusCode.NotFound:
					_message = "دسته بندی یافت نشد";
					break;

				case ModelState.delete when statusCode is HttpStatusCode.OK:
					_message = "دسته بندی فیلم با موفقیت حذف گردید";
					break;

				case ModelState.delete when statusCode is HttpStatusCode.BadRequest:
					_message = "خطا در حذف دسته بندی";
					break;

				case ModelState.delete when statusCode is HttpStatusCode.NotFound:
					_message = "دسته بندی یافت نشد";
					break;

				default:
					_message = "داده ای یافت نشد";
					break;

			}
		}
	}
}
