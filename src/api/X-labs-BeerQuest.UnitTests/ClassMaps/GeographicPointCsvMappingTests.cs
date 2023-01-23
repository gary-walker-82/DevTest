using System.Text;
using CsvHelper.Configuration;
using Faker;
using X_labs_BeerQuest.Domain.Models;
using X_labs_BeerQuest.Domain.Services.CsvReaderService;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Exceptions;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Maps;

namespace X_labs_BeerQuest.UnitTests.ClassMaps
{
	public class GeographicPointCsvMappingTests
	{
		readonly private CsvReaderService _instance;

		public GeographicPointCsvMappingTests()
		{
			_instance = new CsvReaderService(new List<ClassMap> { new GeographicPointMap() });
		}

		[Fact]
		public async Task GivenInValidCsvByteArrayGeographicPointMapShouldThrowError()
		{
			//Arrange
			const string fileContents = """
			"lat","lng"
			"junk","values"
			""";

			var fileBytes = Encoding.Default.GetBytes(fileContents);

			//Act + Asset
			await Assert.ThrowsAsync<CsvConversionException>(() => _instance.ConvertToObject<Ratings>(fileBytes));
		}

		[Fact]
		public async Task GivenValidCsvByteArrayGeographicPointMapShouldCorrectlyMapAllGeographicProperties()
		{
			//Arrange

			var lat = (double)RandomNumber.Next(-900000, 900000) / 100000;
			var lng = (double)RandomNumber.Next(-1800000, 1800000) / 18000;
			var fileContents = $"""
			"lat","lng"
			"{lat}","{lng}"
			""";
			var fileBytes = Encoding.Default.GetBytes(fileContents);

			//Act
			var actual = await _instance.ConvertToObject<GeographicPoint>(fileBytes);

			//Assert
			Assert.NotEmpty(actual);
			Assert.Equal(lat, actual[0].Latitude);
			Assert.Equal(lng, actual[0].Longitude);
		}
	}
}