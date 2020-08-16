namespace GoalSystems.WebApi.Infrastructure.Services.BackgroundWorkerService.Abstractions
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IBackgroundWorkerService
    {
        Task<IJob> Schedule(Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt);
    }
}
