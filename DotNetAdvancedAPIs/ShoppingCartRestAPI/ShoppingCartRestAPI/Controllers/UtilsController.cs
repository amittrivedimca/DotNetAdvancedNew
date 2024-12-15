using AzureMessageBroker;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCartRestAPI.Controllers
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

        [HttpGet(Name = "ReceiveBrokerMessage")]
        public async Task<IActionResult> Get()
        {
            AzureServiceBusReceiver azureService = new AzureServiceBusReceiver();
            await azureService.ReceiveTestMessageAsync();
            return Ok();
        }
       
    }
}
