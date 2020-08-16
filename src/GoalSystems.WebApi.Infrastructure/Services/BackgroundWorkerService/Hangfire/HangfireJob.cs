namespace GoalSystems.WebApi.Infrastructure.Services.BackgroundWorkerService.Hangfire
{
    using GoalSystems.WebApi.Infrastructure.Services.BackgroundWorkerService.Abstractions;
    using System;

    internal class HangfireJob : Job
    {
        internal HangfireJob(string id) : base()
        {
            _ = id ?? throw new ArgumentNullException(nameof(id));
            if (!Guid.TryParse(id, out Guid parsedGuid))
            {
                throw new ArgumentException("The parameter must be a valid Guid.", nameof(id));
            }

            Id = parsedGuid;
        }
    }
}
