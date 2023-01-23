using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using X_labs_BeerQuest.Api;

namespace X_labs_BeerQuest.IntegrationTests
{
	public class AzureSearchBeerQuestTestWebApplicationFactory : BeerQuestTestWebApplicationFactory<Program>
	{
	}

	public class BeerQuestTestWebApplicationFactory<TStartup>
		: WebApplicationFactory<TStartup> where TStartup : class
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services => { });
		}
	}
}