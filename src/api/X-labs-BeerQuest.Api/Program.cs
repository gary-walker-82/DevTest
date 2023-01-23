using System.Reflection;
using CsvHelper.Configuration;
using FluentValidation;
using MediatR;
using X_labs_BeerQuest.Api.Middleware;
using X_labs_BeerQuest.Domain;
using X_labs_BeerQuest.Domain.Application.SearchIndexRefresh;
using X_labs_BeerQuest.Domain.Application.SearchReviews;
using X_labs_BeerQuest.Domain.Models;
using X_labs_BeerQuest.Domain.Services.CsvReaderService;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Maps;
using X_labs_BeerQuest.Domain.Services.Search;
using X_labs_BeerQuest.Domain.Services.Search.Models;
using X_labs_BeerQuest.Domain.Services.Search.SearchDocuments;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddValidatorsFromAssemblyContaining<SearchIndexRefreshRequestValidator>(ServiceLifetime.Transient);
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(SearchIndexRefreshRequestHandler))!);
builder.Services.Decorate(typeof(IRequestHandler<,>), typeof(ValidateRequestHandlerDecorator<,>));
builder.Services.AddScoped<ICsvReaderService, CsvReaderService>();
builder.Services.Scan(x =>
	x.FromAssemblyOf<ReviewMap>().AddClasses(classes => classes.AssignableTo(typeof(ClassMap)))
		.As<ClassMap>().WithSingletonLifetime());

builder.Services
	.AddScoped<ISearchService<Review, ReviewSearchDocument>, ReviewAzureSearchService>();
var searchSettings = builder.Configuration.GetSection("Search").Get<SearchConfiguration>();
builder.Services.AddSingleton(searchSettings!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseHttpsRedirection();
app.UseMiddleware(typeof(ErrorHandlingMiddleware));


app.MapPost("/Admin/SearchIndex", async (HttpRequest request, IMediator mediator) =>
{
	var file = request.Form.Files.FirstOrDefault();
	using var fileContentsStream = new MemoryStream();
	file?.CopyToAsync(fileContentsStream);
	await mediator.Send(
		new SearchIndexRefreshRequest(
			fileContentsStream.ToArray()));
	return Results.Accepted();
}).Accepts<IFormFile>("multipart/form-data");

app.MapPost("/Search",
		async (SearchReviewsRequest request, IMediator mediator) =>
			Results.Ok(await mediator.Send(request)))
	.Accepts<SearchReviewsRequest>("application/json");


app.Run();

namespace X_labs_BeerQuest.Api
{
	public class Program
	{
		protected Program()
		{
		}
	}
}