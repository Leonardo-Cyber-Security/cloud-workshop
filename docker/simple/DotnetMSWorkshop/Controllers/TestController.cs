using Microsoft.AspNetCore.Mvc;
using DotnetMSWorkshop.Utils.Attributes;
using Swashbuckle.AspNetCore.Annotations;

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
            ) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<TestController> _logger = logger;


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

    }
}
