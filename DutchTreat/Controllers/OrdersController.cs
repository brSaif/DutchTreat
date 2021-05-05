using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IDutchRepository repository,
            ILogger<OrdersController> logger,IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems = true)
        {
            try
            {
                var result = _repository.GetAllOrders(includeItems);
                if (result != null) return Ok(_mapper.Map<IEnumerable<OrderViewModel>>(result));
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed To Get Orders : {e}");
                return BadRequest("Failed To Get Orders");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Get(int id)
        {
            try
            {
                var result = (_repository.GetOrderById(id));
                if (result != null)
                {
                    return Ok(_mapper.Map<Order,OrderViewModel>(result));
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order with id : {id} \n {ex} ");
                return BadRequest($"Failed to get order with id : {id}");
            }
        }

        
        public IActionResult Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // We can't pass a DTO to a DB so we have to convert it an Order Model
                    //var newOrder = new Order()
                    //{
                    //    OrderDate = model.OrderDate,
                    //    OrderNumber = model.OrderNumber,
                    //    Id = model.OrderId
                    //};

                    var newOrder = _mapper.Map<OrderViewModel,Order>(model);

                    // Check if date specified otherwise set it.
                    if (newOrder.OrderDate == DateTime.MinValue) newOrder.OrderDate = DateTime.UtcNow;

                    _repository.AddEntity(newOrder);

                    if (_repository.SaveAll())
                    {
                        // Convert Order back to VM 
                        // An alternative of this hole approach is to use AutoMapper
                        //var vm = new OrderViewModel()
                        //{
                        //    OrderId = newOrder.Id,
                        //    OrderDate = newOrder.OrderDate,
                        //    OrderNumber = newOrder.OrderNumber
                        //};
                        return Created($"/api/orders/{newOrder.Id}", _mapper.Map<Order,OrderViewModel>(newOrder));
                    } 
                }

                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to save the order to the db {e}");
            }
            return BadRequest("Failed to save the order");

        }
    }
}
