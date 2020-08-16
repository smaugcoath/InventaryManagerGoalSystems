namespace GoalSystems.WebApi.Business.Services.ItemService
{
    using Infrastructure.Services.Mediator.Abstractions;

    internal sealed class ItemExpiredNotification : IMessage
    {
        public string Name { get; }

        internal ItemExpiredNotification(string name)
        {
            Name = name;
        }

    }
}
