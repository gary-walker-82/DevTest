using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using X_labs_BeerQuest.Domain.Models;

namespace X_labs_BeerQuest.Domain.Services.CsvReaderService.Converters
{
	public class LocationCategoryConverter : DefaultTypeConverter
	{
		public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
		{
			return text.Trim().ToLowerInvariant() switch
			{
				"closed venues" => LocationCategory.Closed,
				"bar reviews" => LocationCategory.Bar,
				"pub reviews" => LocationCategory.Pub,
				"other reviews" => LocationCategory.Other,
				"uncategorized" => LocationCategory.Uncategorised,
				_ => throw new ArgumentOutOfRangeException(nameof(text), text, null)
			};
		}

		public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
		{
			if (value.GetType().FullName != typeof(LocationCategory).FullName)
			{
				throw new Exception("unknown type");
			}

			return (LocationCategory) value switch
			{
				LocationCategory.Closed => "Closed venues",
				LocationCategory.Bar => "Bar reviews",
				LocationCategory.Pub => "Pub reviews",
				LocationCategory.Other => "Other reviews",
				LocationCategory.Uncategorised => "Uncategorized",
				_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
			};
		}
	}
}