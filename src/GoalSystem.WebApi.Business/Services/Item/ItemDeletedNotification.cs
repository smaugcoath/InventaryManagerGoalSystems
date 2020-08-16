namespace GoalSystems.WebApi.Business.Services.ItemService
{
    using Infrastructure.Services.Mediator.Abstractions;

    internal sealed class ItemDeletedNotification : IMessage
    {
        public string Name { get; }

        internal ItemDeletedNotification(string name)
        {
            Name = name;
        }

    }
}
