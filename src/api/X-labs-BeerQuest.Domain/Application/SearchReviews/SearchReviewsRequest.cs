using MediatR;
using X_labs_BeerQuest.Domain.Models;
using X_labs_BeerQuest.Domain.Services.Search;
using X_labs_BeerQuest.Domain.Services.Search.SearchDocuments;

namespace X_labs_BeerQuest.Domain.Application.SearchReviews
{
	public enum SortOrderEnum
	{
		Distance,
		BeerRating,
		ValueRating,
		AmenitiesRating,
		Atmosphere
	}

	public record LocationSearch(double? Longitude, double? Latitude, double? MaxDistanceInMiles)
	{
		readonly private bool _useLocationSearch =
			Longitude.HasValue && Latitude.HasValue && MaxDistanceInMiles.HasValue;

		public string Filter => _useLocationSearch
			? $"geo.distance({nameof(ReviewSearchDocument.Location)}, geography'POINT({Longitude} {Latitude})') le {MaxDistanceInMiles}"
			: string.Empty;
	}

	public record MinRatingsSearch(double? Atmosphere, double? Amenities, double? Beer, double? ValueForMoney)
	{
		public string Filter => string.Join(" and ", new List<string>
		{
			CreateFilterFor(nameof(ReviewSearchDocument.Atmosphere), Atmosphere),
			CreateFilterFor(nameof(ReviewSearchDocument.Amenities), Amenities),
			CreateFilterFor(nameof(ReviewSearchDocument.Beer), Beer),
			CreateFilterFor(nameof(ReviewSearchDocument.ValueForMoney), ValueForMoney)
		}.Where(x => !string.IsNullOrWhiteSpace(x)));

		private static string CreateFilterFor(string ratingsFilterName, double? minRating)
		{
			return minRating is > 0 ? $"{ratingsFilterName} ge {minRating}" : string.Empty;
		}
	}

	public record SearchSort(SortOrderEnum SortField = SortOrderEnum.Distance, bool SortAscending = true)
	{
		public string DirectionString => SortAscending ? "asc" : "desc";
	}

	public record SearchPaging(int? Page = 1, int? PageSize = 10)
	{
	}


	public record SearchReviewsRequest(string SearchTerm, LocationSearch? Location, MinRatingsSearch? MinRatings,
		List<string> Tags,
		SearchSort? Sort, SearchPaging? Paging) :
		IRequest<ISearchResult<Review>>,
		ISearchSpecification
	{
		readonly private string TagsFilter = Tags != null && Tags.Any()
			? $"({string.Join(" and ", Tags.Select(x => $"{nameof(ReviewSearchDocument.Tags)}/any(tag: tag eq '{x}')"))})" //" {nameof(ReviewSearchDocument.Tags)}/any(tag:  $"tag eq '{x}'"))})"//{string.Join(" and ", Tags.Select(x => $"tag eq '{x}'"))}) //tag eq 'x' search.in(tag, '{string.Join(", ", Tags.Select(x => $"{x}"))}',',')
			: "";

		private string UserSortString =>
			Sort.SortField switch
			{
				SortOrderEnum.Distance =>
					$"geo.distance({nameof(ReviewSearchDocument.Location)}, geography'POINT({Location.Longitude} {Location.Latitude})') {Sort.DirectionString}",
				SortOrderEnum.BeerRating =>
					$"{nameof(ReviewSearchDocument.Beer)} {Sort.DirectionString}",
				SortOrderEnum.ValueRating =>
					$"{nameof(ReviewSearchDocument.ValueForMoney)} {Sort.DirectionString}",
				SortOrderEnum.AmenitiesRating =>
					$"{nameof(ReviewSearchDocument.Amenities)} {Sort.DirectionString}",
				SortOrderEnum.Atmosphere =>
					$"{nameof(ReviewSearchDocument.Atmosphere)} {Sort.DirectionString}",
				_ => throw new ArgumentOutOfRangeException(nameof(Sort), Sort, "unknown sort type")
			};

		public int? PageSize => Paging.PageSize;
		public int? Page => Paging.Page;

		/// <inheritdoc />
		public List<string> Facets => new() {nameof(ReviewSearchDocument.Tags)};

		public string FormattedSearchTerm => string.IsNullOrWhiteSpace(SearchTerm)
			? "*"
			: $""" "{SearchTerm}" or *{SearchTerm} or {SearchTerm}*""";

		public string Filter => string.Join(" and ",
			new List<string> {Location?.Filter, MinRatings?.Filter, TagsFilter}.Where(
				x => !string.IsNullOrWhiteSpace(x)));

		public List<string> SortOrder => new() {UserSortString, "search.score() desc"};
	}
}