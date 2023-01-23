using MediatR;
using X_labs_BeerQuest.Domain.Models;
using X_labs_BeerQuest.Domain.Services.Search;
using X_labs_BeerQuest.Domain.Services.Search.SearchDocuments;

namespace X_labs_BeerQuest.Domain.Application.SearchIndexRefresh
{
	public class SearchIndexRefreshRequestHandler : IRequestHandler<SearchIndexRefreshRequest>
	{
		readonly private ISearchService<Review, ReviewSearchDocument> _searchService;

		public SearchIndexRefreshRequestHandler(ISearchService<Review, ReviewSearchDocument> searchService)
		{
			_searchService = searchService;
		}

		#region Implementation of IRequestHandler<in SearchIndexHandler,Unit>

		/// <inheritdoc />
		public async Task<Unit> Handle(SearchIndexRefreshRequest request, CancellationToken cancellationToken)
		{
			await _searchService.DeleteAsync();
			await _searchService.CreateAsync("suggester", nameof(ReviewSearchDocument.Name));

			await _searchService.UpsertDocumentsAsync(request.Reviews);

			return Unit.Value;
		}

		#endregion
	}
}