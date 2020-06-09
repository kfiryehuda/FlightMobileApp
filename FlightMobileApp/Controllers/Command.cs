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
            connect.WriteAndRead("data\r\n");

        }

        [HttpPost]
        [Route("api/command")]
        public ActionResult<PlaneInfo> PostCommands(PlaneInfo info)
        {

            var throttle = "/controls/engines/current-engine/throttle";
            var aileron = "/controls/flight/aileron";
            var elevator = "/controls/flight/elevator";
            var rudder = "/controls/flight/rudder";
            connect.WriteAndRead("set " + throttle + " " + info.throttle + " \r\n");
            connect.WriteAndRead("set " + aileron + " " + info.aileron + " \r\n");
            connect.WriteAndRead("set " + elevator + " " + info.elevator + " \r\n");
            connect.WriteAndRead("set " + rudder + " " + info.rudder + " \r\n");
            //Console.WriteLine(aileron);
            return info;
        }

        /* [HttpGet]
         [Route("screenshot")]
         public async Task<ActionResult> GetPicture()
         {
             *//*            string url = "http://www.google.com";
                         RedirectResult redirectResult = new RedirectResult(url, true);
                         return redirectResult;*//*

             return Image(new { url = "http://www.example.com" });
         }
 */

    }
}
