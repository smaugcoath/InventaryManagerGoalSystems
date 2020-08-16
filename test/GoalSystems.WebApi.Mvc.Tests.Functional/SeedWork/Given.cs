using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoalSystems.WebApi.Mvc.Tests.Functional.SeedWork
{
    internal class Given
    {
        private readonly ServerFixture _fixture;

        internal Given(ServerFixture fixture)
        {
            _fixture = fixture;
        }

        internal async Task<IEnumerable<TEntity>> This<TEntity>(params TEntity[] values)
            where TEntity : class
        {
            //var context = _fixture.GetService<DatabaseContext>();

            //context.Set<TEntity>().AddRange(values);
            //await context.SaveChangesAsync();

            return values;
        }
    }
}
