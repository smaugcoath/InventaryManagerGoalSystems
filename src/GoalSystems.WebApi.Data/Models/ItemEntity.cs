
namespace GoalSystems.WebApi.Data.Models
{
    using System;

    public class ItemEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public byte Type { get; set; }
    }
}
