using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FlightMobileApp.Controllers
{
    [ApiController]
    public class CommandController : ControllerBase
    {
        IConfiguration configuration;
        private readonly ILogger<CommandController> _logger;
        private Connect connect;
        
        private IMemoryCache cache;
        public CommandController(IMemoryCache cache, ILogger<CommandController> logger, IConfiguration configuration)
        {
            this.configuration = configuration;
            string ip = configuration.GetValue<string>("flightGearIP");
            int port = Convert.ToInt32(configuration.GetValue<string>("flightGearPort"));
            _logger = logger;
            this.cache = cache;
            if (!cache.TryGetValue("connect", out connect))
            {
                connect = new Connect();
                cache.Set("connect", connect);
                connect.ConnectToFG(ip, port);
            }
        }

        [HttpPost]
        [Route("api/command")]
        public ActionResult<PlaneInfo> PostCommands(PlaneInfo info)
        {
            var throttle = "/controls/engines/current-engine/throttle";
            var aileron = "/controls/flight/aileron";
            var elevator = "/controls/flight/elevator";
            var rudder = "/controls/flight/rudder";
            //check if the return valuses is the same
            if (!(connect.WriteAndRead(throttle, info.throttle)
                && connect.WriteAndRead(aileron, info.aileron)
                && connect.WriteAndRead(elevator, info.elevator)
                && connect.WriteAndRead(rudder, info.rudder)))
            {
                return NotFound();
            }

            return Ok();
        }


    }
}
