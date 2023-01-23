using System.Text;
using CsvHelper.Configuration;
using Faker;
using X_labs_BeerQuest.Domain.Models;
using X_labs_BeerQuest.Domain.Services.CsvReaderService;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Exceptions;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Maps;

namespace X_labs_BeerQuest.UnitTests.ClassMaps
{
	public class SocialPresenceCsvMappingTests
	{
		readonly private CsvReaderService _instance;

		public SocialPresenceCsvMappingTests()
		{
			_instance = new CsvReaderService(new List<ClassMap> {new SocialPresenceMap()});
		}

		[Fact]
		public async Task GivenInValidCsvByteArraySocialPresenceMapShouldThrowError()
		{
			//Arrange
			const string fileContents = """
			"url","lng","address"
			"junk","values","address"
			""";

			var fileBytes = Encoding.Default.GetBytes(fileContents);

			//Act + Asset
			await Assert.ThrowsAsync<CsvConversionException>(() =>
				_instance.ConvertToObject<SocialPresence>(fileBytes));
		}

		[Fact]
		public async Task GivenValidCsvByteArraySocialPresenceMapShouldCorrectlyMapAllGeographicProperties()
		{
			//Arrange
			var webSite = Internet.Url();
			var twitterHandle = Lorem.GetFirstWord();
			var fileContents = $"""
			"twitter","url"
			"{twitterHandle}","{webSite}"
			""";
			var fileBytes = Encoding.Default.GetBytes(fileContents);

			//Act
			var actual = await _instance.ConvertToObject<SocialPresence>(fileBytes);

			//Assert
			Assert.NotEmpty(actual);
			Assert.NotNull(actual[0].Website);
			Assert.Equal(webSite, actual[0].Website?.ToString());
			Assert.Equal(twitterHandle, actual[0].TwitterHandle);
		}
	}
}