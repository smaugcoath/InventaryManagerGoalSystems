namespace GoalSystems.WebApi.Mvc.Models
{
    using GoalSystems.WebApi.Mvc.Models.Abstractions;
    using System;

    /// <inheritdoc/>
    public class GetAsyncResponse : BaseResponse<GetAsyncResponse.Item>
    {
        public class Item
        {
            public string Name { get; set; }

            public DateTime ExpirationDate { get; set; }

            public byte Type { get; set; }

        }
    }
}
