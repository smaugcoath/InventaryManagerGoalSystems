namespace GoalSystems.WebApi.Infrastructure.Services.BackgroundWorkerService.Hangfire
{
    using global::Hangfire;
    using GoalSystems.WebApi.Infrastructure.Services.BackgroundWorkerService.Abstractions;
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    internal class HangfireBackgroundWorkerService : IBackgroundWorkerService
    {
        public Task<IJob> Schedule(Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt)
        {
            string jobId = BackgroundJob.Schedule(methodCall, enqueueAt);

            IJob job = new HangfireJob(jobId);

            return Task.FromResult(job);
        }
    }
}
