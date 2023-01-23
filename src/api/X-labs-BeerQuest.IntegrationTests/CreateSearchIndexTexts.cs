using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using X_labs_BeerQuest.Api;
using X_labs_BeerQuest.Domain.Application.SearchReviews;
using X_labs_BeerQuest.Domain.Models;
using X_labs_BeerQuest.Domain.Services.CsvReaderService;
using X_labs_BeerQuest.Domain.Services.Search;
using X_labs_BeerQuest.Domain.Services.Search.Models;
using X_labs_BeerQuest.Domain.Services.Search.SearchDocuments;
using Xunit;

namespace X_labs_BeerQuest.IntegrationTests
{
	public class AzureSearchFixture : IClassFixture<AzureSearchBeerQuestTestWebApplicationFactory>
	{
		protected readonly HttpClient Client;
		protected readonly BeerQuestTestWebApplicationFactory<Program> Factory;
		protected readonly IServiceScope Services;

		public AzureSearchFixture(AzureSearchBeerQuestTestWebApplicationFactory factory)
		{
			Factory = factory;
			Client = factory.CreateClient(new WebApplicationFactoryClientOptions
			{
				AllowAutoRedirect = false
			});
			Services = Factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
			const string fileContents = """
			"name","category","url","date","excerpt","thumbnail","lat","lng","address","phone","twitter","stars_beer","stars_atmosphere","stars_amenities","stars_value","tags"
			"115 The Headrow","Pub reviews","http://leedsbeer.info/?p=2753","2014-10-18T15:48:51+00:00","A bar that lives up to its name.","http://leedsbeer.info/wp-content/uploads/2014/10/115.jpg","53.7994003","-1.545981","115 The Headrow, Leeds, LS1 5JW","","BLoungeGrp","1","2","4","5","coffee,food"
			"1871 Bar & Lounge","Bar reviews","http://leedsbeer.info/?p=1455","2013-05-16T16:10:30+00:00","Not much going on here beer-wise, but they do serve food on a Sunday evening — quite a rarity amongst city centre bars. ","http://leedsbeer.info/wp-content/uploads/2013/05/20130506_205930.jpg","53.7958908","-1.5433109","7-9 Boar Lane, Leeds LS1 5DD","0845 200 1871","1871BarLeeds","5","4","3","2","coffee,food,free wifi"
			""";
			var fileBytes = Encoding.Default.GetBytes(fileContents);
			var reviews = Services.ServiceProvider.GetRequiredService<ICsvReaderService>()
				.ConvertToObject<Review>(fileBytes).GetAwaiter().GetResult();
			var searchService =
				Services.ServiceProvider.GetRequiredService<ISearchService<Review, ReviewSearchDocument>>();
			searchService.DeleteAsync().GetAwaiter().GetResult();
			searchService.CreateAsync("suggester", "Name").GetAwaiter().GetResult();
			searchService.UpsertDocumentsAsync(reviews).GetAwaiter().GetResult();
		}
	}

	public class SearchReviewsRequestTexts : AzureSearchFixture
	{
		/// <inheritdoc />
		public SearchReviewsRequestTexts(
			AzureSearchBeerQuestTestWebApplicationFactory factory) : base(factory)
		{
		}

		[Fact]
		public async Task GivenANullRequestShouldReturn500Error()
		{
			//Arrange

			//Act
			var response = await Client.PostAsync("/Search", null);

			//Assert

			Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
		}

		[Fact]
		public async Task GivenARequestWithEmptySearchTermShouldReturnFirstPageOfResults()
		{
			//Arrange

			//Act
			var response = await Client.PostAsJsonAsync("/Search", new {Page = 1, PageSize = 10, SearhTerm = ""});

			//Assert
			var actual = await response.Content.ReadFromJsonAsync<AzureSearchResult<Review>>();
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public async Task GivenARequestWithSortSetToBeerShouldReturnFirstPageOfResultsWithHighestBeerRatingFirst()
		{
			//Arrange

			//Act
			var response = await Client.PostAsJsonAsync("/Search",
				new {Page = 1, PageSize = 10, SearhTerm = "", Sort = SortOrderEnum.BeerRating, SortAscending = false});

			//Assert
			var actual = await response.Content.ReadFromJsonAsync<AzureSearchResult<Review>>();
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal("1871 Bar & Lounge", actual.Results[0].Location.Name);
		}

		[Fact]
		public async Task
			GivenARequestWithSortSetToValueRatingShouldReturnFirstPageOfResultsWithHighestBeerRatingFirst()
		{
			//Arrange

			//Act
			var response = await Client.PostAsJsonAsync("/Search",
				new {Page = 1, PageSize = 10, SearhTerm = "", Sort = SortOrderEnum.ValueRating, SortAscending = false});

			//Assert
			var actual = await response.Content.ReadFromJsonAsync<AzureSearchResult<Review>>();
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal("115 The Headrow", actual.Results[0].Location.Name);
		}
	}

	public class CreateSearchIndexTexts : BaseBeerQuestApiTests
	{
		/// <inheritdoc />
		public CreateSearchIndexTexts(BeerQuestTestWebApplicationFactory<Program> factory) : base(factory)
		{
		}

		[Fact]
		public async Task GivenEmptyFileSearchIndexShouldReturn400Error()
		{
			//Arrange
			using var content1 = new StreamContent(new MemoryStream(new byte[] { }));
			using var formData = new MultipartFormDataContent();
			formData.Add(content1, "files", "text.csv");

			//Act
			var response = await Client.PostAsync("/Admin/SearchIndex", formData);

			//Assert
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task GivenMalformedFileSearchIndexShouldReturn400Error()
		{
			//Arrange
			var fileContents = "some random text with, commas,";
			var fileBytes = Encoding.Default.GetBytes(fileContents);

			using var content1 = new StreamContent(new MemoryStream(fileBytes));
			using var formData = new MultipartFormDataContent();
			formData.Add(content1, "files", "text.csv");

			//Act
			var response = await Client.PostAsync("/Admin/SearchIndex", formData);

			//Assert
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task GivenFileWith2CorrectlyFormattedRowsSearchIndexShouldBeReCreated()
		{
			//Arrange
			const string fileContents = """
			"name","category","url","date","excerpt","thumbnail","lat","lng","address","phone","twitter","stars_beer","stars_atmosphere","stars_amenities","stars_value","tags"
			"115 The Headrow","Pub reviews","http://leedsbeer.info/?p=2753","2014-10-18T15:48:51+00:00","A bar that lives up to its name.","http://leedsbeer.info/wp-content/uploads/2014/10/115.jpg","53.7994003","-1.545981","115 The Headrow, Leeds, LS1 5JW","","BLoungeGrp","1.5","3","2.5","2","coffee,food"
			"1871 Bar & Lounge","Bar reviews","http://leedsbeer.info/?p=1455","2013-05-16T16:10:30+00:00","Not much going on here beer-wise, but they do serve food on a Sunday evening — quite a rarity amongst city centre bars. ","http://leedsbeer.info/wp-content/uploads/2013/05/20130506_205930.jpg","53.7958908","-1.5433109","7-9 Boar Lane, Leeds LS1 5DD","0845 200 1871","1871BarLeeds","1.5","3","2.5","1.5","coffee,food,free wifi"
			""";
			var fileBytes = Encoding.Default.GetBytes(fileContents);

			using var content1 = new StreamContent(new MemoryStream(fileBytes));
			using var formData = new MultipartFormDataContent();
			formData.Add(content1, "files", "text.csv");

			//Act
			var response = await Client.PostAsync("/Admin/SearchIndex", formData);

			//Assert
			Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
		}
	}
}