namespace GoalSystems.WebApi.Infrastructure.Services.Mediator.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMediatorService
    {
        Task Publish<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : IMessage;
    }
}
