using System.Text;
using CsvHelper.Configuration;
using Faker;
using X_labs_BeerQuest.Domain.Services.CsvReaderService;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Exceptions;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Maps;
using Address = X_labs_BeerQuest.Domain.Models.Address;

namespace X_labs_BeerQuest.UnitTests.ClassMaps
{
	public class AddressCsvMappingTests
	{
		readonly private CsvReaderService _instance;

		public AddressCsvMappingTests()
		{
			_instance = new CsvReaderService(new List<ClassMap> {new AddressMap()});
		}

		[Fact]
		public async Task GivenInValidCsvByteArrayAddressMapShouldThrowError()
		{
			//Arrange
			var fileContents = """
			"lat","lng","address"
			"junk","values","address"
			""";

			var fileBytes = Encoding.Default.GetBytes(fileContents);

			//Act + Asset
			await Assert.ThrowsAsync<CsvConversionException>(() => _instance.ConvertToObject<Address>(fileBytes));
		}

		[Fact]
		public async Task GivenValidCsvByteArrayAddressMapShouldCorrectlyMapAllGeographicProperties()
		{
			//Arrange
			var address = Faker.Address.StreetAddress();
			var lat = (double) RandomNumber.Next(-900000, 900000) / 100000;
			var lng = (double) RandomNumber.Next(-1800000, 1800000) / 18000;
			var fileContents = $"""
			"lat","lng","address"
			"{lat}","{lng}","{address}"
			""";
			var fileBytes = Encoding.Default.GetBytes(fileContents);

			//Act
			var actual = await _instance.ConvertToObject<Address>(fileBytes);

			//Assert
			Assert.NotEmpty(actual);
			Assert.Equal(address, actual[0].AddressString);
			Assert.NotNull(actual[0].GeographicPoint);
			Assert.Equal(lat, actual[0].GeographicPoint?.Latitude);
			Assert.Equal(lng, actual[0].GeographicPoint?.Longitude);
		}
	}
}