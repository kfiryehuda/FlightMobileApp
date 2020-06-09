using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FlightMobileApp.Controllers
{
    [ApiController]
    public class CommandController : ControllerBase
    {

        private readonly ILogger<CommandController> _logger;
        private Connect connect;
        public CommandController(IMemoryCache cache, ILogger<CommandController> logger)
        {
            _logger = logger;

        }

        [HttpPost]
        [Route("api/command")]
        public ActionResult<PlaneInfo> PostCommands(PlaneInfo info)
        {
            connect = new Connect();
            connect.ConnectToFG("127.0.0.1", 5402);
            //connect.WriteAndRead("data\n");

            string aileron = connect.WriteAndRead("set /controls/flight/aileron " + info.aileron + " \r\n");

            //Console.WriteLine(aileron);
            return info;
        }

        [HttpGet]
        [Route("screenshot")]
        public async Task<IActionResult> GetPicture()
        {

            var image = System.IO.File.OpenRead("C:\\test\\random_image.jpeg");
            return File(image, "image/jpeg");
        }


    }
}
