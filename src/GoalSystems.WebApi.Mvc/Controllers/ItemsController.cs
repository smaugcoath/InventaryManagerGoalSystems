namespace GoalSystems.WebApi.Mvc.Controllers
{
    using AutoMapper;
    using GoalSystems.WebApi.Business.Models;
    using GoalSystems.WebApi.Business.Services.ItemService;
    using GoalSystems.WebApi.Mvc.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;


    [ApiController]
    //[Authorize] Authorization is not implemented yet.
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(
            IItemService itemService,
            IMapper mapper,
            ILogger<ItemsController> logger)
        {
            _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

       
        [HttpGet("{name}")]
        //[Authorize("ReadPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetAsyncResponse>> GetAsync(string name)
        {
            var item = await _itemService.GetAsync(name);

            var result = _mapper.Map<GetAsyncResponse>(item);

            return Ok(result);
        }

        //This uses Put verb because is idenpotent. If you call the endpooint twice with the sames parameters the system status do not change, it only reponse with a Conflict.
        [HttpPut]
        //[Authorize("CreatePolicy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AddAsyncResponse>> AddAsync(AddAsyncRequest request)
        {
            var item = _mapper.Map<Item>(request);
            var itemAdded = await _itemService.AddAsync(item);
            var response = _mapper.Map<AddAsyncResponse>(itemAdded);

            var locationHeader = Url.ActionLink(nameof(GetAsync), values: new { name = item.Name });
            return Created(locationHeader, response);
           // It seems to be some unexpected behaviour here. CreatedAtAction and CreatedAtRoute do not work and throw a RouteNotFound exception even if you setup MVC to not take into account Async suffixes.
           // Anyway, this should be the correct way to return the location header and content.
           // return CreatedAtAction(nameof(GetAsync), new { name = item.Name }, response);
        }

        [HttpDelete("{name:required:maxlength(100)}")]
        //[Authorize("DeletePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DeleteAsyncResponse>> DeleteAsync(string name)
        {
            var item = await _itemService.DeleteAsync(name);
            var response = _mapper.Map<DeleteAsyncResponse>(item);

            return Ok(response);
        }
    }
}
