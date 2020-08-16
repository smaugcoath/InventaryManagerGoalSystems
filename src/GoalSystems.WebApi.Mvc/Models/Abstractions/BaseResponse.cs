namespace GoalSystems.WebApi.Mvc.Models.Abstractions
{
    public abstract class BaseResponse<TResponse> where TResponse : new()
    {
        public TResponse Data { get; set; } = new TResponse();
    }
}
