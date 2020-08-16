namespace GoalSystems.WebApi.Infrastructure.Services.BackgroundWorkerService.Abstractions
{
    using System;

    public interface IJob
    {
        Guid Id { get; }
    }
}