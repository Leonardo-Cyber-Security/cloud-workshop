using Microsoft.AspNetCore.Mvc;
using DotnetMSWorkshop.Db;
using DotnetMSWorkshop.Entities;
using DotnetMSWorkshop.Utils.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DotnetMSWorkshop.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TestController"/> class.
    /// </remarks>
    /// <param name="configuration">The configuration instance.</param>
    /// <param name="logger">The logger instance.</param>
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController(IConfiguration configuration
            , ILogger<TestController> logger
            , AppDbContext context
            ) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<TestController> _logger = logger;
        private readonly AppDbContext _context = context;


        /// <summary>
        /// Test
        /// </summary>
        /// <remarks>Restituisce la lista</remarks>
        /// <param name="ItemName">ItemName</param>
        /// <response code="200">Dati restituiti correttamente</response>
        /// <response code="204">Nessun dato presente</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ValidateModelState]
        [SwaggerOperation("TestGET")]
        [SwaggerResponse(statusCode: 200, type: null, description: "Dati restituiti correttamente")]
        [SwaggerResponse(statusCode: 204, type: null, description: "Nessun dato presente")]
        public async Task<IActionResult> TestGET([FromQuery(Name = "ItemName")] string ItemName)
        {
            _logger.LogInformation("TestGET START");
            _logger.LogInformation($"ItemName = {ItemName}");

            var items = new List<string> { "Item1", "Item2", "Item3", "Item4" };
            var matchedItems = items.Where(item => item.Contains(ItemName, StringComparison.OrdinalIgnoreCase)).ToList();

            if (matchedItems.Count == 0)
            {
                return NoContent();
            }

            return Ok(matchedItems);
        }

        /// <summary>
        /// Test
        /// </summary>
        /// <remarks>Restituisce la lista</remarks>
        /// <param name="search">ItemName</param>
        /// <response code="200">Dati restituiti correttamente</response>
        /// <response code="204">Nessun dato presente</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ValidateModelState]
        [SwaggerOperation("GetItems")]
        [SwaggerResponse(statusCode: 200, type: null, description: "Dati restituiti correttamente")]
        [SwaggerResponse(statusCode: 204, type: null, description: "Nessun dato presente")]
        public async Task<IActionResult> GetItems([FromQuery] string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return BadRequest("Search parameter is required.");
            }

            // Query con Contains per filtrare gli items in base al nome
            var items = await _context.Items
                .Where(i => i.Name.Contains(search))
                .ToListAsync();

            return Ok(items);
        }
    }
}
