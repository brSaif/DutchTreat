using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IDutchRepository repository,ILogger<ProductsController> _logger)
        {
            _repository = repository;
            this._logger = _logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Product>> Get()
        {
            // We can change IEnumerable<Products> to JsonResult
            // and wrap the _repository.GetAllProducts() in Json() and in the catch{return Json("Bad request")};
            // but that will limit our return type to json only 

            // An alternative is to use ActionResult and wrap the return data in
            // response codes ex: 200, 400, 403 and so on 

            // For better documentation we may use ActionResult<IEnumerable<Products>> 
            try
            {
                return Ok(_repository.GetAllProducts());
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get products : {e}");
                return BadRequest("Failed to get products");
            }
           
        }
    }
}
