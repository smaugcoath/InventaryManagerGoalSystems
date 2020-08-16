namespace GoalSystems.WebApi.Mvc.Models
{
    using GoalSystems.WebApi.Mvc.Models.Abstractions;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddAsyncRequest : BaseRequest<AddAsyncRequest.Item>
    {

        public class Item
        {
            [Required]
            [MinLength(1)]
            [MaxLength(100)]
            public string Name { get; set; }

            public DateTime ExpirationDate { get; set; }

            public byte Type { get; set; }

        }
    }
}
