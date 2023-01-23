using CsvHelper.Configuration;
using X_labs_BeerQuest.Domain.Models;

namespace X_labs_BeerQuest.Domain.Services.CsvReaderService.Maps
{
	public sealed class SocialPresenceMap : ClassMap<SocialPresence>
	{
		public SocialPresenceMap()
		{
			Map(m => m.Website).Name("url");
			Map(m => m.TwitterHandle).Name("twitter");
		}
	}
}