namespace GoalSystems.WebApi.Business.Services.ItemService
{
    using GoalSystems.WebApi.Business.Models;
    using System.Threading.Tasks;

    public interface IItemService
    {
        Task<bool> ExistsAsync(string name);
        Task<Item> AddAsync(Item item);
        Task<Item> DeleteAsync(string name);
        Task<Item> GetAsync(string name);
    }
}
