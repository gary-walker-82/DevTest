using CsvHelper.Configuration;
using X_labs_BeerQuest.Domain.Models;

namespace X_labs_BeerQuest.Domain.Services.CsvReaderService.Maps
{
	public sealed class RatingsMap : ClassMap<Ratings>
	{
		public RatingsMap()
		{
			Map(x => x.Amenities).Name("stars_amenities");
			Map(x => x.Atmosphere).Name("stars_atmosphere");
			Map(x => x.Beer).Name("stars_beer");
			Map(x => x.ValueForMoney).Name("stars_value");
		}
	}
}