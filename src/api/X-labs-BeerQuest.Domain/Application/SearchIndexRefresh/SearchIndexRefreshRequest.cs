using MediatR;
using X_labs_BeerQuest.Domain.Models;
using X_labs_BeerQuest.Domain.Services.CsvReaderService;

namespace X_labs_BeerQuest.Domain.Application.SearchIndexRefresh
{
	public record SearchIndexRefreshRequest : IRequest
	{
		public SearchIndexRefreshRequest(byte[] fileContents)
		{
			FileContents = fileContents;
		}

		public byte[] FileContents { get; init; }

		public List<Review> Reviews { get; private set; } = new();

		public async Task<List<Review>> ConvertFileBytesToReviewsAsync(ICsvReaderService csvReaderService)
		{
			Reviews = await csvReaderService.ConvertToObject<Review>(FileContents);
			return Reviews;
		}
	}
}