using FluentValidation;
using X_labs_BeerQuest.Domain.Services.CsvReaderService;

namespace X_labs_BeerQuest.Domain.Application.SearchIndexRefresh
{
	public class
		SearchIndexRefreshRequestValidator : AbstractValidator<SearchIndexRefreshRequest>
	{
		public SearchIndexRefreshRequestValidator(ICsvReaderService csvReaderService)
		{
			RuleFor(x => x.FileContents).NotEmpty();
			RuleFor(x => x)
				.MustAsync(async (request, cancellationToken) =>
					(await request.ConvertFileBytesToReviewsAsync(csvReaderService)).Count > 0)
				.WithName("Contents")
				.WithMessage("Failed to convert contents to list of Reviews");
		}
	}
}