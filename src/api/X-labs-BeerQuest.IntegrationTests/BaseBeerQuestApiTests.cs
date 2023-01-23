using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using X_labs_BeerQuest.Api;
using Xunit;

namespace X_labs_BeerQuest.IntegrationTests
{
	public class BaseBeerQuestApiTests : IClassFixture<BeerQuestTestWebApplicationFactory<Program>>
	{
		protected readonly HttpClient Client;
		protected readonly BeerQuestTestWebApplicationFactory<Program> Factory;
		protected readonly IServiceScope Services;

		public BaseBeerQuestApiTests(BeerQuestTestWebApplicationFactory<Program> factory)
		{
			Factory = factory;
			Client = factory.CreateClient(new WebApplicationFactoryClientOptions
			{
				AllowAutoRedirect = false
			});
			Services = Factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
		}
	}
}