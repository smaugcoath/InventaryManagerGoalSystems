namespace GoalSystems.WebApi.Business.Hanlders
{
    using GoalSystems.WebApi.Business.Services.ItemService;
    using GoalSystems.WebApi.Infrastructure.Services.Mediator.Abstractions;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class ItemExpiredHandler : IMessageHandler<ItemExpiredNotification>
    {
        private readonly IItemService _itemService;

        internal ItemExpiredHandler(IItemService itemService)
        {
            _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
        }

        public async Task Handle(ItemExpiredNotification notification, CancellationToken cancellationToken)
        {
            // For instance, the action to be done here could be to delete the item from the inventory.
            await _itemService.DeleteAsync(notification.Name);

            // Call to another services to proccess the notification. Example: Send an email, publish a message into a Bus, etc...
        }
    }
}
