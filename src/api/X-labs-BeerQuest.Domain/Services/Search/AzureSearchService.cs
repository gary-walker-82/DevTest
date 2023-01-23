using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using X_labs_BeerQuest.Domain.Services.Search.Models;

namespace X_labs_BeerQuest.Domain.Services.Search
{
	public class AzureSearchService<T, TSearchDocument> : ISearchService<T, TSearchDocument>
		where T : class, new()
		where TSearchDocument : class, ISearchDocument<T>, new()
	{
		readonly private SearchClient _searchClient;
		readonly private SearchIndexClient _searchIndexClient;
		readonly private string _searchIndexName;

		public AzureSearchService(
			SearchConfiguration searchConfig, string searchIndexName)
		{
			_searchIndexClient =
				new SearchIndexClient(new Uri(searchConfig.EndPoint),
					new AzureKeyCredential(searchConfig.Secret));
			_searchClient = _searchIndexClient.GetSearchClient(searchIndexName);
			_searchIndexName = searchIndexName;
		}

		#region Implementation of ISearchService

		/// <inheritdoc />
		public async Task<ISearchResult<T>> SearchAsync(ISearchSpecification searchSpecification)
		{
			var searchOption = CreateSearchOption(searchSpecification);

			var searchResult =
				await _searchClient.SearchAsync<TSearchDocument>(searchSpecification.FormattedSearchTerm,
					searchOption);

			var results = searchResult.Value.GetResults();
			var entities = results.Select(x => x.Document.ConvertTo()).ToList();
			var temp = searchResult.Value.Facets.ToDictionary(x => x.Key,
				x => x.Value.Where(x => !string.IsNullOrWhiteSpace(x.Value.ToString())).Select(x => x.Value.ToString())
					.ToList());
			return new AzureSearchResult<T>(entities, searchSpecification.Page, searchSpecification.PageSize,
				(int) (searchResult.Value.TotalCount ?? 0), temp);
		}


		public async Task UpsertDocumentsAsync(IEnumerable<T> documents)
		{
			var membersRequest = ChunkBy(documents, 5000);
			var addTasks = membersRequest.Select(membersToSync =>
					_searchClient.MergeOrUploadDocumentsAsync(
						membersToSync.Select(x => new TSearchDocument().ConvertFrom(x))))
				.Cast<Task>()
				.ToList();

			await Task.WhenAll(addTasks);
		}


		private static IEnumerable<List<T>> ChunkBy(IEnumerable<T> source, int chunkSize)
		{
			return source
				.Select((x, i) => new {Index = i, Value = x})
				.GroupBy(x => x.Index / chunkSize)
				.Select(x => x.Select(v => v.Value).ToList());
		}

		public async Task CreateAsync(string suggesterName, params string[] searchSuggesterField)
		{
			var fieldBuilder = new FieldBuilder();
			var index = new SearchIndex(_searchIndexName,
				fieldBuilder.Build(typeof(TSearchDocument)));

			var suggester = new SearchSuggester(suggesterName, searchSuggesterField);
			index.Suggesters.Add(suggester);
			await _searchIndexClient.CreateOrUpdateIndexAsync(index);
		}

		public async Task DeleteAsync()
		{
			await _searchIndexClient.DeleteIndexAsync(_searchIndexName);
		}

		private SearchOptions CreateSearchOption(ISearchSpecification parameters)
		{
			var options = new SearchOptions
			{
				Size = parameters.PageSize,
				Skip = (parameters.Page - 1) * parameters.PageSize,
				IncludeTotalCount = true,
				Filter = parameters.Filter
			};
			parameters.Facets.ForEach(options.Facets.Add);
			parameters.SortOrder.ForEach(options.OrderBy.Add);

			return options;
		}

		#endregion
	}
}