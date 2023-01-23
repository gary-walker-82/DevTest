using FluentValidation;

namespace X_labs_BeerQuest.Domain.Application.SearchReviews
{
	public class
		SearchReviewsRequestValidator : AbstractValidator<SearchReviewsRequest>
	{
		public SearchReviewsRequestValidator()
		{
			RuleFor(x => x).NotNull();
		}
	}
}