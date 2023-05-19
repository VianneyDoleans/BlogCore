using Xunit;

namespace BlogCoreAPI.FunctionalTests.Models
{
    /// <summary>
    /// https://xunit.net/docs/shared-context#collection-fixture
    /// </summary>
    [CollectionDefinition("WebApplicationFactory")]
    public class TestWebApplicationFactoryCollection : ICollectionFixture<TestWebApplicationFactory>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces. 
    }
}
