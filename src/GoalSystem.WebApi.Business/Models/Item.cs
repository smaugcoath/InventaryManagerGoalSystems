namespace GoalSystems.WebApi.Business.Models
{
    using System;

    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime ExpirationDate { get; set; }

        public byte Type { get; set; }

    }
}
