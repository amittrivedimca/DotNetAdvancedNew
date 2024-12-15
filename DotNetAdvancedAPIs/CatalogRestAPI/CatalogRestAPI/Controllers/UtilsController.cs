using AzureMessageBroker;
using Microsoft.AspNetCore.Mvc;

namespace CatalogRestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UtilsController : ControllerBase
    {        

        private readonly ILogger<UtilsController> _logger;

        public UtilsController(ILogger<UtilsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "SendBrokerMessage")]
        public async Task<IActionResult> Get()
        {
            AzureServiceBusProducer azureService = new AzureServiceBusProducer();
            await azureService.SendTestMessageAsync();
            return Ok();
        }
    }
}
