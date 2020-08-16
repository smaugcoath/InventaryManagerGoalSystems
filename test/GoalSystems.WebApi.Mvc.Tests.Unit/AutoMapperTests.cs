using AutoMapper;
using GoalSystems.WebApi.Mvc.Mappers;
using Xunit;

namespace GoalSystems.WebApi.Mvc.Unit
{
    public class AutoMapper_should
    {
        [Fact]
        public void Configure_valid_mappers()
        {
            var configuration = new MapperConfiguration(x => x.AddProfile<ItemProfile>());
            configuration.AssertConfigurationIsValid();
        }
    }
}
