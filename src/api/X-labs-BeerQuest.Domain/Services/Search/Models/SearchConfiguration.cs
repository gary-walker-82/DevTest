namespace X_labs_BeerQuest.Domain.Services.Search.Models
{
	public record SearchConfiguration(string EndPoint, string Secret, string IndexName, int PageSize)
	{
		// public string EndPoint { get; init; }

		// public string Secret { get; init; }

		// public int PageSize { get; init; }


		// public string IndexName { get; init; }
	}
}