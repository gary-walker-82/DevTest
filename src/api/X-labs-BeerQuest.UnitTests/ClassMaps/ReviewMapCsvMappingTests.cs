using System.Text;
using CsvHelper.Configuration;
using Faker;
using X_labs_BeerQuest.Domain.Models;
using X_labs_BeerQuest.Domain.Services.CsvReaderService;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Exceptions;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Maps;

namespace X_labs_BeerQuest.UnitTests.ClassMaps
{
	public class ReviewMapCsvMappingTests
	{
		readonly private CsvReaderService _instance;

		public ReviewMapCsvMappingTests()
		{
			_instance = new CsvReaderService(new List<ClassMap> { new ReviewMap() });
		}

		[Fact]
		public async Task GivenInValidCsvByteArrayReviewMapShouldThrowError()
		{
			//Arrange
			const string fileContents = """
			"missingname","category","url","date","excerpt","thumbnail","lat","lng","address","phone","twitter","stars_beer","stars_atmosphere","stars_amenities","stars_value","tags"
			"115 The Headrow","Pub reviews","http://leedsbeer.info/?p=2753","2014-10-18T15:48:51+00:00","A bar that lives up to its name.","http://leedsbeer.info/wp-content/uploads/2014/10/115.jpg","53.7994003","-1.545981","115 The Headrow, Leeds, LS1 5JW","","BLoungeGrp","1.5","3","2.5","2","coffee,food"
			""";

			var fileBytes = Encoding.Default.GetBytes(fileContents);

			//Act + Asset
			await Assert.ThrowsAsync<CsvConversionException>(() =>
				_instance.ConvertToObject<Review>(fileBytes));
		}

		[Fact]
		public async Task GivenValidCsvByteArrayReviewMapShouldCorrectlyMapAllGeographicProperties()
		{
			//Arrange
			var dateOfReview = DateTime.UtcNow.AddHours(RandomNumber.Next(-100000, 100000));
			var fileContents = $"""
			"name","category","url","date","excerpt","thumbnail","lat","lng","address","phone","twitter","stars_beer","stars_atmosphere","stars_amenities","stars_value","tags"
			"115 The Headrow","Pub reviews","http://leedsbeer.info/?p=2753","{dateOfReview:yyyy-MM-ddTHH:mm:sszzz}","A bar that lives up to its name.","http://leedsbeer.info/wp-content/uploads/2014/10/115.jpg","53.7994003","-1.545981","115 The Headrow, Leeds, LS1 5JW","","BLoungeGrp","1.5","3","2.5","2","coffee,food"
			""";

			var fileBytes = Encoding.Default.GetBytes(fileContents);

			//Act
			var actual = await _instance.ConvertToObject<Review>(fileBytes);

			//Assert
			Assert.NotEmpty(actual);
			Assert.Equal(dateOfReview.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"),
				actual[0].DateVisited.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"));
			Assert.NotNull(actual[0].Location);
			Assert.NotNull(actual[0].Ratings);
		}
	}
}