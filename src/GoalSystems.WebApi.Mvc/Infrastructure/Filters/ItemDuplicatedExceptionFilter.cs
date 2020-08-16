namespace GoalSystems.WebApi.Mvc.Infrastructure.Filters
{
    using GoalSystems.WebApi.Business.Services.ItemService.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ItemDuplicatedExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            if (context.Exception is ItemDuplicatedNameException exception)
            {
                context.ExceptionHandled = true;
                context.Result = new ObjectResult(exception.Message) { StatusCode = StatusCodes.Status409Conflict };
            }
        }
    }
}
