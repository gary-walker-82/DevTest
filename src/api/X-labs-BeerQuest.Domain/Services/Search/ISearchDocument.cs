namespace X_labs_BeerQuest.Domain.Services.Search
{
	public interface ISearchDocument<T> where T : class, new()
	{
		object ConvertFrom(T entity);
		T ConvertTo();
	}
}