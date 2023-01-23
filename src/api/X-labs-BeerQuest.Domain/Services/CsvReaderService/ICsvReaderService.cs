namespace X_labs_BeerQuest.Domain.Services.CsvReaderService
{
	public interface ICsvReaderService
	{
		Task<List<T>> ConvertToObject<T>(byte[] fileBytes);
	}
}