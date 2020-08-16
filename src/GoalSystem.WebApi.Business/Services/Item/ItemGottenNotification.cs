namespace GoalSystems.WebApi.Business.Services.ItemService
{
    using Infrastructure.Services.Mediator.Abstractions;

    internal sealed class ItemGottenNotification : IMessage
    {
        public int Id { get; }

        internal ItemGottenNotification(int id)
        {
            Id = id;
        }

    }
}
