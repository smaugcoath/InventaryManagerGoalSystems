using FluentAssertions;
using GoalSystems.WebApi.Business.Models;
using GoalSystems.WebApi.Business.Services.ItemService;
using GoalSystems.WebApi.Mvc.Controllers;
using GoalSystems.WebApi.Mvc.Models;
using GoalSystems.WebApi.Mvc.Tests.Functional.SeedWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GoalSystems.WebApi.Mvc.Tests.Functional
{
    [Collection(nameof(ServerCollectionFixture))]
    [ResetDatabase]
    public class Create_should
    {
        private readonly ServerFixture _serverFixture;

        internal Create_should(ServerFixture serverFixture)
        {
            _serverFixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        public async Task Return_created_result_with_location_header()
        {
            var request = new AddAsyncRequest
            {
                Data = new AddAsyncRequest.Item
                {
                    ExpirationDate = DateTime.Now.AddMinutes(1),
                    Name = "ItemName",
                    Type = 1
                }
            };

            var exectedStatusCode = StatusCodes.Status201Created;
            var expectedLocationHeader = $"api/items/itemname";

            var response = await _serverFixture.Server.CreateHttpApiRequest<ItemsController>(c => c.AddAsync(request))
                .SendAsync(HttpMethods.Put);

            response.EnsureSuccessStatusCode();
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(exectedStatusCode);
            response.Headers.Location.Should().NotBeNull()
                .And.Should().Be(expectedLocationHeader);
        }

        [Fact]
        public async Task Return_conflict_if_name_is_duplicated()
        {
            const string ItemName = "ItemName";
            var itemService = _serverFixture.GetService<IItemService>();
            var item = new Item { Name = ItemName, Type = default, ExpirationDate = default };
            await itemService.AddAsync(item);
            var request = new AddAsyncRequest
            {
                Data = new AddAsyncRequest.Item
                {
                    ExpirationDate = DateTime.Now.AddMinutes(1),
                    Name = ItemName,
                    Type = 1
                }
            };

            var exectedStatusCode = StatusCodes.Status409Conflict;

            var response = await _serverFixture.Server.CreateHttpApiRequest<ItemsController>(c => c.AddAsync(request))
                .SendAsync(HttpMethods.Put);

            response.EnsureSuccessStatusCode();
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(exectedStatusCode);
        }
    }
}
