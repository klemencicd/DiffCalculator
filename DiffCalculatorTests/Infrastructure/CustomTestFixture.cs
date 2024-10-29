using DiffCalculatorApi.Repositories.Interfaces;
using Moq;

namespace DiffCalculatorTests.Infrastructure;
public class CustomTestFixture : IDisposable
{
    public Mock<IDiffRepository> MockRepository { get; private set; }
    public HttpClient Client { get; private set; }
    private CustomServiceFactory factory;

    public CustomTestFixture()
    {
        MockRepository = new Mock<IDiffRepository>();
        factory = new(MockRepository.Object);
        Client = factory.CreateClient();
    }

    public void UseRealRepository()
    {
        factory = new();
        Client = factory.CreateClient();
    }

    public void CreateNewMock()
    {
        MockRepository = new Mock<IDiffRepository>();
        factory = new(MockRepository.Object);
        Client = factory.CreateClient();
    }

    public void Dispose()
    {
        Client.Dispose();
        factory.Dispose();
    }
}
