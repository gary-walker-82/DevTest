using X_labs_BeerQuest.Domain.Models;
using X_labs_BeerQuest.Domain.Services.Search.Models;
using X_labs_BeerQuest.Domain.Services.Search.SearchDocuments;

namespace X_labs_BeerQuest.Domain.Services.Search
{
	public class ReviewAzureSearchService : AzureSearchService<Review, ReviewSearchDocument>
	{
		/// <inheritdoc />
		public ReviewAzureSearchService(SearchConfiguration searchConfig) : base(searchConfig,
			searchConfig.IndexName
		)
		{
		}
	}
}