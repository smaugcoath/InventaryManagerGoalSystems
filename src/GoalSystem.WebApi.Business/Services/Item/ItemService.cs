namespace GoalSystems.WebApi.Business.Services.ItemService
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using GoalSystems.WebApi.Business.Models;
    using GoalSystems.WebApi.Business.Services.ItemService.Exceptions;
    using GoalSystems.WebApi.Data;
    using GoalSystems.WebApi.Infrastructure.Services.BackgroundWorkerService.Abstractions;
    using GoalSystems.WebApi.Infrastructure.Services.Mediator.Abstractions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    internal class ItemService : IItemService
    {

        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IMediatorService _mediatorService;
        private readonly IBackgroundWorkerService _backgroundWorkerService;

        public ItemService(DatabaseContext context, IMapper mapper, IMediatorService mediatorService, IBackgroundWorkerService backgroundWorkerService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediatorService = mediatorService ?? throw new ArgumentNullException(nameof(mediatorService));
            _backgroundWorkerService = backgroundWorkerService ?? throw new ArgumentNullException(nameof(backgroundWorkerService));
        }

        public async Task<bool> ExistsAsync(string name)
        {
            var exists = await _context.Items.AsNoTracking()
                .AnyAsync(x => x.Name == name);

            return exists;
        }

        public async Task<Item> AddAsync(Item item)
        {
            bool exists = await ExistsAsync(item.Name);
            if (exists)
            {
                throw new ItemDuplicatedNameException();
            }

            var itemEntity = _mapper.Map<Data.Models.ItemEntity>(item);
            _context.Items.Add(itemEntity);

            await _context.SaveChangesAsync();

            var result = _mapper.Map<Item>(itemEntity);

            await _backgroundWorkerService.Schedule(() => Expire(item.Name), new DateTimeOffset(itemEntity.ExpirationDate));

            await _mediatorService.Publish(new ItemAddedNotification(itemEntity.Id));

            return result;
        }

        public async Task<Item> GetAsync(string name)
        {
            var item = await _context.Items.AsNoTracking()
                .Where(x => x.Name == name)
                .ProjectTo<Item>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (item is null)
            {
                throw new NotFoundException();
            }

            await _mediatorService.Publish(new ItemGottenNotification(item.Id));

            var result = item;

            return result;
        }

        public async Task<Item> DeleteAsync(string name)
        {
            var item = await _context.Items
                .SingleOrDefaultAsync(x => x.Name == name);

            if (item is null)
            {
                throw new NotFoundException();
            }

            _context.Items.Remove(item);

            await _context.SaveChangesAsync();

            var result = _mapper.Map<Item>(item);

            await _mediatorService.Publish(new ItemDeletedNotification(name));

            return result;
        }

        public async Task Expire(string name)
        {
            await _mediatorService.Publish(new ItemExpiredNotification(name), default);
        }
    }
}
