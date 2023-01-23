using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using X_labs_BeerQuest.Api.Middleware;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Exceptions;

namespace X_labs_BeerQuest.UnitTests.Middleware
{
	public class ErrorHandlingMiddlewareTest
	{
		[Fact]
		public async Task Invoke()
		{
			// Arrange
			var middleware = new ErrorHandlingMiddleware(_ =>
				Task.CompletedTask
			);

			var context = new DefaultHttpContext
			{
				Response =
				{
					Body = new MemoryStream()
				}
			};

			//Act
			await middleware.Invoke(context);

			//Assert
			Assert.Equal((int) HttpStatusCode.OK, context.Response.StatusCode);
		}


		[Fact]
		public async Task Invoke_WhenValidationErrorOccurred()
		{
			// Arrange
			var middleware =
				new ErrorHandlingMiddleware(_ => throw new ValidationException("error"));

			var context = new DefaultHttpContext
			{
				Response =
				{
					Body = new MemoryStream()
				}
			};

			//Act
			await middleware.Invoke(context);

			//Assert
			context.Response.Body.Seek(0, SeekOrigin.Begin);
			var reader = new StreamReader(context.Response.Body);
			var streamText = await reader.ReadToEndAsync();

			Assert.StartsWith("""{"error":"error"}""", streamText);
			Assert.Equal((int) HttpStatusCode.BadRequest, context.Response.StatusCode);
		}

		[Fact]
		public async Task Invoke_WhenValidationErrorOccurredWithErrorObject()
		{
			// Arrange
			var middleware = new ErrorHandlingMiddleware(_ => throw new ValidationException("error",
				new List<ValidationFailure> {new("property", "message")}));

			var context = new DefaultHttpContext
			{
				Response =
				{
					Body = new MemoryStream()
				}
			};

			//Act
			await middleware.Invoke(context);

			//Assert
			context.Response.Body.Seek(0, SeekOrigin.Begin);
			var reader = new StreamReader(context.Response.Body);
			var streamText = await reader.ReadToEndAsync();

			Assert.Contains(
				""""PropertyName":"property","ErrorMessage":"message"""",
				streamText);
			Assert.Equal((int) HttpStatusCode.BadRequest, context.Response.StatusCode);
		}


		[Fact]
		public async Task Invoke_WhenBeerQuestExceptionOccurredWithErrorObject()
		{
			// Arrange
			var middleware =
				new ErrorHandlingMiddleware(_ => throw new CsvConversionException("beer quest error message"));

			var context = new DefaultHttpContext
			{
				Response =
				{
					Body = new MemoryStream()
				}
			};

			//Act
			await middleware.Invoke(context);

			//Assert
			context.Response.Body.Seek(0, SeekOrigin.Begin);
			var reader = new StreamReader(context.Response.Body);
			var streamText = await reader.ReadToEndAsync();

			Assert.StartsWith("""{"error":"beer quest error message"}""", streamText);
			Assert.Equal((int) HttpStatusCode.BadRequest, context.Response.StatusCode);
		}

		[Fact]
		public async Task Invoke_WhenGeneralExceptionOccurredWithErrorObject()
		{
			// Arrange
			var middleware = new ErrorHandlingMiddleware(_ => throw new Exception("error message"));

			var context = new DefaultHttpContext
			{
				Response =
				{
					Body = new MemoryStream()
				}
			};

			//Act
			await middleware.Invoke(context);

			//Assert
			context.Response.Body.Seek(0, SeekOrigin.Begin);
			var reader = new StreamReader(context.Response.Body);
			var streamText = await reader.ReadToEndAsync();

			Assert.StartsWith("""{"error":"error message"}""", streamText);
			Assert.Equal((int) HttpStatusCode.InternalServerError, context.Response.StatusCode);
		}
	}
}