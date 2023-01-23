namespace X_labs_BeerQuest.Domain.Services.Search
{
	public interface ISearchResult<T>
	{
		List<T> Results { get; }
		int? Page { get; }
		int? PageSize { get; }
		int? TotalResults { get; }
		Dictionary<string, List<string>> Facets { get; }
	}
}