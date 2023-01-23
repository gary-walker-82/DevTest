namespace X_labs_BeerQuest.Domain.Services.Search
{
	public interface ISearchService<T, TSearchDocument>
		where T : class, new()
		where TSearchDocument : class, ISearchDocument<T>, new()
	{
		Task<ISearchResult<T>> SearchAsync(ISearchSpecification searchSpecification);

		Task UpsertDocumentsAsync(IEnumerable<T> documents);
		Task CreateAsync(string suggesterName, params string[] searchSuggesterField);
		Task DeleteAsync();
	}
}