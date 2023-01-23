using CsvHelper.Configuration;
using X_labs_BeerQuest.Domain.Models;

namespace X_labs_BeerQuest.Domain.Services.CsvReaderService.Maps
{
	public sealed class ReviewMap : ClassMap<Review>
	{
		public ReviewMap()
		{
			Map(m => m.DateVisited).Name("date");
			References<RatingsMap>(x => x.Ratings);
			References<LocationMap>(x => x.Location);
		}
	}
}