namespace GoalSystems.WebApi.Infrastructure.Services.Mediator.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMessageHandler<TNotification> where TNotification : IMessage
    {
        Task Handle(TNotification notification, CancellationToken cancellationToken);
    }
}
