namespace GoalSystems.WebApi.Mvc.Models
{
    using GoalSystems.WebApi.Mvc.Models.Abstractions;
    using System;

    /// <inheritdoc/>
    public class AddAsyncResponse : BaseResponse<AddAsyncResponse.Item>
    {
        public class Item
        {
            public string Name { get; set; }

            public DateTime ExpirationDate { get; set; }

            public byte Type { get; set; }

        }
    }
}
