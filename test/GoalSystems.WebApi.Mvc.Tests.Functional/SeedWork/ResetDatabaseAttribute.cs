using System.Reflection;
using Xunit.Sdk;

namespace GoalSystems.WebApi.Mvc.Tests.Functional.SeedWork
{
    public sealed class ResetDatabaseAttribute : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            ServerFixture.ResetDatabase();
        }
    }
}
