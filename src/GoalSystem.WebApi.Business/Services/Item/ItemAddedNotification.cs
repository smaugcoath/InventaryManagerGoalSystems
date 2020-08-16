namespace GoalSystems.WebApi.Business.Services.ItemService
{
    using Infrastructure.Services.Mediator.Abstractions;

    internal sealed class ItemAddedNotification : IMessage
    {
        public int Id { get; }

        internal ItemAddedNotification(int id)
        {
            Id = id;
        }

    }
}
