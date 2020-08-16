namespace GoalSystems.WebApi.Business.Mappers
{
    using AutoMapper;

    internal class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Data.Models.ItemEntity, Models.Item>().ReverseMap();
        }
    }
}
