namespace GoalSystems.WebApi.Infrastructure.Services.BackgroundWorkerService.Abstractions
{
    using System;

    internal abstract class Job : IJob
    {
        public Guid Id { get; protected set; }
    }
}
