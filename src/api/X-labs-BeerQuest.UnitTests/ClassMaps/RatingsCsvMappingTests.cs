using System.Text;
using CsvHelper.Configuration;
using Faker;
using X_labs_BeerQuest.Domain.Models;
using X_labs_BeerQuest.Domain.Services.CsvReaderService;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Exceptions;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Maps;

namespace X_labs_BeerQuest.UnitTests.ClassMaps
{
	public class RatingsCsvMappingTests
	{
		readonly private CsvReaderService _instance;

		public RatingsCsvMappingTests()
		{
			_instance = new CsvReaderService(new List<ClassMap> { new RatingsMap() });
		}

		[Fact]
		public async Task GivenInValidCsvByteArrayRatingsMapShouldThrowError()
		{
			//Arrange
			const string fileContents = """
			"star_beer","stars_atmosphere","star_amenities","stars_value"
			"Test","Error","Should","Break"
			""";

			var fileBytes = Encoding.Default.GetBytes(fileContents);

			//Act + Asset
			await Assert.ThrowsAsync<CsvConversionException>(() => _instance.ConvertToObject<Ratings>(fileBytes));
		}

		[Fact]
		public async Task GivenValidCsvByteArrayRatingsMapShouldCorrectlyMapAllRatingsToProperties()
		{
			//Arrange

			var beerStar = (double)RandomNumber.Next(0, 50) / 10;
			var atmosphereStar = (double)RandomNumber.Next(0, 50) / 10;
			var amenitiesStar = (double)RandomNumber.Next(0, 50) / 10;
			var valueStar = (double)RandomNumber.Next(0, 50) / 10;
			var fileContents = $"""
			"stars_beer","stars_atmosphere","stars_amenities","stars_value"
			"{beerStar}","{atmosphereStar}","{amenitiesStar}","{valueStar}"
			""";

			var fileBytes = Encoding.Default.GetBytes(fileContents);

			//Act
			var actual = await _instance.ConvertToObject<Ratings>(fileBytes);

			//Assert
			Assert.NotEmpty(actual);
			Assert.Equal(beerStar, actual[0].Beer);
			Assert.Equal(amenitiesStar, actual[0].Amenities);
			Assert.Equal(atmosphereStar, actual[0].Atmosphere);
			Assert.Equal(valueStar, actual[0].ValueForMoney);
		}
	}
}