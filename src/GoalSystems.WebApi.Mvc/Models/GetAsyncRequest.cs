namespace GoalSystems.WebApi.Mvc.Models
{
    using GoalSystems.WebApi.Mvc.Models.Abstractions;
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Not used yet because the MVC binding system and swagger are well prepared to do this easily.
    /// </summary>
    public class GetAsyncRequest : BaseRequest<GetAsyncRequest.Item>
    {
        public class Item
        {
            [Required]
            [MinLength(1)]
            [MaxLength(100)]
            public string Name { get; set; }
        }
    }


}
