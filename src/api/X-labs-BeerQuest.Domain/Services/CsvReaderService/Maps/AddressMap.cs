using CsvHelper.Configuration;
using X_labs_BeerQuest.Domain.Models;

namespace X_labs_BeerQuest.Domain.Services.CsvReaderService.Maps
{
	public sealed class AddressMap : ClassMap<Address>
	{
		public AddressMap()
		{
			Map(x => x.AddressString).Name("address");
			References<GeographicPointMap>(x => x.GeographicPoint);
		}
	}
}