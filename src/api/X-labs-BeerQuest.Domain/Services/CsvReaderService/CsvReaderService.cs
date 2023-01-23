using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using X_labs_BeerQuest.Domain.Services.CsvReaderService.Exceptions;

namespace X_labs_BeerQuest.Domain.Services.CsvReaderService
{
	public class CsvReaderService : ICsvReaderService
	{
		readonly private List<ClassMap> _maps;

		public CsvReaderService(IEnumerable<ClassMap> maps)
		{
			_maps = maps.ToList();
		}

		#region Implementation of ICsvReader

		/// <summary>
		///     Takes Csv File Bytes and converts it into a list of T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="fileBytes"></param>
		/// <returns></returns>
		/// <exception cref="CsvConversionException"></exception>
		public Task<List<T>> ConvertToObject<T>(byte[] fileBytes)
		{
			try
			{
				using var memoryStream = new MemoryStream(fileBytes);
				using var reader = new StreamReader(memoryStream);
				using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

				_maps.ForEach(x => csv.Context.RegisterClassMap(x));
				var result = csv.GetRecords<T>().ToList();

				return Task.FromResult(result);
			}
			catch (Exception ex)
			{
				throw new CsvConversionException(ex.Message, ex);
			}
		}

		#endregion
	}
}