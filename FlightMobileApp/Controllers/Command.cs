using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        string ip = "127.0.0.1";
        int port = 5402;

        public CommandController(IMemoryCache cache, ILogger<CommandController> logger)
        {
            _logger = logger;
            connect = new Connect();
            connect.ConnectToFG(ip, port);


        }

        [HttpPost]
        [Route("api/command")]
        public ActionResult<PlaneInfo> PostCommands(PlaneInfo info)
        {

            var throttle = "/controls/engines/current-engine/throttle";
            var aileron = "/controls/flight/aileron";
            var elevator = "/controls/flight/elevator";
            var rudder = "/controls/flight/rudder";
            if (!(connect.WriteAndRead(throttle, info.throttle)
                && connect.WriteAndRead(aileron, info.aileron)
                && connect.WriteAndRead(elevator, info.elevator)
                && connect.WriteAndRead(rudder, info.rudder)))
            {
                return NotFound();
            }

            //Console.WriteLine(aileron);
            return Ok();
        }


    }
}
