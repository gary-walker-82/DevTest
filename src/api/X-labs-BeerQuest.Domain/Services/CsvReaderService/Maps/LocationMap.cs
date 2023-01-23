using CsvHelper.Configuration;
using X_labs_BeerQuest.Domain.Models;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Converters;

namespace X_labs_BeerQuest.Domain.Services.CsvReaderService.Maps
{
	public sealed class LocationMap : ClassMap<Location>
	{
		public LocationMap()
		{
			Map(m => m.Name).Name("name");
			Map(m => m.Category).Name("category").TypeConverter<LocationCategoryConverter>();
			Map(m => m.Excerpt).Name("excerpt");
			Map(m => m.Thumbnail).Name("thumbnail");
			Map(m => m.Phone).Name("phone");
			Map(m => m.TagsString).Name("tags");
			References<AddressMap>(x => x.Address);
			References<SocialPresenceMap>(x => x.SocialPresence);
		}
	}
}