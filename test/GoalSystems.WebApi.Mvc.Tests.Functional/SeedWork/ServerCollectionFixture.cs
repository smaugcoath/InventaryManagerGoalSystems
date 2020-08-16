namespace GoalSystems.WebApi.Mvc.Tests.Functional.SeedWork
{
    using Xunit;

    [CollectionDefinition(nameof(ServerCollectionFixture))]
    public class ServerCollectionFixture
        : ICollectionFixture<ServerFixture>
    {
    }
}
