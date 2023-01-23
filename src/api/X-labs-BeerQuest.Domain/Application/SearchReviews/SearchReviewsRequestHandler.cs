using MediatR;
using X_labs_BeerQuest.Domain.Models;
using X_labs_BeerQuest.Domain.Services.Search;
using X_labs_BeerQuest.Domain.Services.Search.SearchDocuments;

namespace X_labs_BeerQuest.Domain.Application.SearchReviews
{
	public class SearchReviewsRequestHandler : IRequestHandler<SearchReviewsRequest, ISearchResult<Review>>
	{
		readonly private ISearchService<Review, ReviewSearchDocument> _searchService;

		public SearchReviewsRequestHandler(ISearchService<Review, ReviewSearchDocument> searchService)
		{
			_searchService = searchService;
		}

		#region Implementation of IRequestHandler<in SearchIndexHandler,Unit>

		/// <inheritdoc />
		public async Task<ISearchResult<Review>> Handle(SearchReviewsRequest request,
			CancellationToken cancellationToken)
		{
			return await _searchService.SearchAsync(request);
		}

		#endregion
	}
}