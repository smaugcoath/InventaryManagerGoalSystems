namespace GoalSystems.WebApi.Business.Hanlders
{
    using GoalSystems.WebApi.Business.Services.ItemService;
    using GoalSystems.WebApi.Infrastructure.Services.Mediator.Abstractions;
    using System.Threading;
    using System.Threading.Tasks;

    internal class ItemDeletedHandler : IMessageHandler<ItemGottenNotification>
    {
        public async Task Handle(ItemGottenNotification notification, CancellationToken cancellationToken)
        {
            // Syntax sugar await. Method not implemented yet.
            await Task.CompletedTask;

            // Call to another service to proccess the notification. Example: Send an email, push notification, publish a message into a Bus, etc...
        }
    }
}
