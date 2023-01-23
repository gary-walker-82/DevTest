namespace X_labs_BeerQuest.Domain.Services.Search
{
	public interface ISearchSpecification
	{
		string FormattedSearchTerm { get; }
		List<string> SortOrder { get; }
		string Filter { get; }
		int? PageSize { get; }
		int? Page { get; }
		List<string> Facets { get; }
	}
}