using DiffCalculatorApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiffCalculatorTests.Infrastructure;
public class CustomServiceFactory(IDiffRepository? _repositoryOverride = null) : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddHttpClient();

            if (_repositoryOverride != null)
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDiffRepository));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddTransient(_ => _repositoryOverride);
            }
        });

        return base.CreateHost(builder);
    }
}
