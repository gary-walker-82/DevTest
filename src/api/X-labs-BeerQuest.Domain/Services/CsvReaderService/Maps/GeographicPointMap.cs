using CsvHelper.Configuration;
using X_labs_BeerQuest.Domain.Models;

namespace X_labs_BeerQuest.Domain.Services.CsvReaderService.Maps
{
	public sealed class GeographicPointMap : ClassMap<GeographicPoint>
	{
		public GeographicPointMap()
		{
			Map(m => m.Longitude).Name("lng");
			Map(m => m.Latitude).Name("lat");
		}
	}
}