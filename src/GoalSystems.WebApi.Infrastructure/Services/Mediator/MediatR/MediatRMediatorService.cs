namespace GoalSystems.WebApi.Infrastructure.Services.Mediator.MediatR
{
    using global::MediatR;
    using GoalSystems.WebApi.Infrastructure.Services.Mediator.Abstractions;
    using System;
    using System.Threading;
    using System.Threading.Tasks;


    internal class MediatRMediatorService : IMediatorService
    {
        private readonly IMediator _mediator;

        public MediatRMediatorService(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Publish<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage: IMessage
        {
            await _mediator.Publish(message, cancellationToken);
        }

    }
}
