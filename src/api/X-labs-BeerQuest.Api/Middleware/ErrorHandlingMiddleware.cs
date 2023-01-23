using System.Net;
using System.Text;
using FluentValidation;
using Newtonsoft.Json;
using X_labs_BeerQuest.Domain.Services;

namespace X_labs_BeerQuest.Api.Middleware
{
	public class ErrorHandlingMiddleware
	{
		readonly private RequestDelegate _next;

		public ErrorHandlingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (ValidationException ex)
			{
				await ValidationException(context, ex);
			}
			catch (BeerQuestException ex)
			{
				await BeerQuestException(context, ex);
			}
			catch (Exception ex)
			{
				await GeneralException(context, ex);
			}
		}

		private static async Task ValidationException(HttpContext context, ValidationException ex)
		{
			var result = ex.Errors.Any()
				? JsonConvert.SerializeObject(new {error = ex.Errors})
				: SerializeObject(ex);
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
			await context.Response.WriteAsync(result);
		}

		private static async Task GeneralException(HttpContext context, Exception ex)
		{
			var result = SerializeObject(ex);
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
			await context.Response.WriteAsync(result);
		}

		private static async Task BeerQuestException(HttpContext context, Exception ex)
		{
			var result = SerializeObject(ex);
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
			await context.Response.WriteAsync(result);
		}


		private static string SerializeObject(Exception? ex)
		{
			if (ex == null)
			{
				return string.Empty;
			}

			var resultBuilder = new StringBuilder();
			var stackTrace = JsonConvert.SerializeObject(new {stack = ex.StackTrace});
			do
			{
				resultBuilder.AppendLine(JsonConvert.SerializeObject(new {error = ex.Message}));
				ex = ex.InnerException;
			} while (ex != null);

			resultBuilder.AppendLine(stackTrace);

			return resultBuilder.ToString();
		}
	}
}