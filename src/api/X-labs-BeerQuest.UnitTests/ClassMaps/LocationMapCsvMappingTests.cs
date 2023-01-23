using System.Text;
using CsvHelper.Configuration;
using Faker;
using X_labs_BeerQuest.Domain.Models;
using X_labs_BeerQuest.Domain.Services.CsvReaderService;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Exceptions;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Maps;

namespace X_labs_BeerQuest.UnitTests.ClassMaps
{
	public class LocationMapCsvMappingTests
	{
		readonly private CsvReaderService _instance;

		public LocationMapCsvMappingTests()
		{
			_instance = new CsvReaderService(new List<ClassMap> { new LocationMap() });
		}

		[Fact]
		public async Task GivenInValidCsvByteArrayLocationMapShouldThrowError()
		{
			//Arrange
			const string fileContents = """
			"missingname","category","url","excerpt","thumbnail","lat","lng","address","phone","twitter","tags"
			"name","category","url","excerpt","thumbNail","53.7994003","-1.545981","115 The Headrow, Leeds, LS1 5JW","phone","BLoungeGrp","coffee,food"
			""";

			var fileBytes = Encoding.Default.GetBytes(fileContents);

			//Act + Asset
			await Assert.ThrowsAsync<CsvConversionException>(() =>
				_instance.ConvertToObject<Location>(fileBytes));
		}

		[Theory]
		[InlineData("Closed venues", LocationCategory.Closed)]
		[InlineData("Bar reviews", LocationCategory.Bar)]
		[InlineData("Pub reviews", LocationCategory.Pub)]
		public async Task GivenValidCsvByteArrayLocationMapShouldCorrectlyMapAllGeographicProperties(string category,
			LocationCategory expectedCategory)
		{
			//Arrange
			var availableTags = new List<string> { "food", "live music", "coffee", "free wifi" };
			var name = Company.Name();
			var url = Internet.Url();
			var thumbNail = Internet.Url();
			var excerpt = Lorem.Sentence();
			var phone = Phone.Number();
			var tags = string.Join(", ", Enumerable.Range(1, RandomNumber.Next(4)).Select(x => availableTags[x - 1]));
			var fileContents = $""""
			"name","category","url","excerpt","thumbnail","lat","lng","address","phone","twitter","tags"
			"{name}","{category}","{url}","{excerpt}","{thumbNail}","53.7994003","-1.545981","115 The Headrow, Leeds, LS1 5JW","{phone}","BLoungeGrp","{tags}"
			"""";

			var fileBytes = Encoding.Default.GetBytes(fileContents);

			//Act
			var actual = await _instance.ConvertToObject<Location>(fileBytes);

			//Assert
			Assert.NotEmpty(actual);
			Assert.Equal(name, actual[0].Name);
			Assert.Equal(expectedCategory, actual[0].Category);
			Assert.Equal(excerpt, actual[0].Excerpt);
			Assert.Equal(thumbNail, actual[0].Thumbnail);
			Assert.Equal(phone, actual[0].Phone);
			Assert.Equal(tags, actual[0].TagsString);
			Assert.NotNull(actual[0].Address);
		}
	}
}