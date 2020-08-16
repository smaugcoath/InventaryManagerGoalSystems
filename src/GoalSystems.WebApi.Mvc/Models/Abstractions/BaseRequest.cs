namespace GoalSystems.WebApi.Mvc.Models.Abstractions
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Api base request based on json.api specs. <see href="https://jsonapi.org/"/>
    /// </summary>
    public abstract class BaseRequest<TRequest> where TRequest : new()
    {
        [Required]
        public TRequest Data { get; set; } = new TRequest();
    }
}
