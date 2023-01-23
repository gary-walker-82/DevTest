namespace X_labs_BeerQuest.Domain.Services.CsvReaderService.Exceptions
{
	public class CsvConversionException : BeerQuestException
	{
		public CsvConversionException()
		{
		}

		public CsvConversionException(string message) : base(message)
		{
		}

		public CsvConversionException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}