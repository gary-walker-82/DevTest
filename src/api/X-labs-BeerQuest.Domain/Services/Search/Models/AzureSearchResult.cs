namespace X_labs_BeerQuest.Domain.Services.Search.Models
{
	public record AzureSearchResult<T>(List<T> Results, int? Page = null, int? PageSize = null,
		int? TotalResults = null, Dictionary<string, List<string>> Facets = null) : ISearchResult<T>
	{
	}
}