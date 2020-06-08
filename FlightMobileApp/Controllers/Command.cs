﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlightMobileApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandController : ControllerBase
    {

        private readonly ILogger<CommandController> _logger;

        public CommandController(ILogger<CommandController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("api/command")]
        public async ActionResult<PlaneInfo> PostCommands(PlaneInfo info)
        {

            return info;
        }

        [HttpGet]
        [Route("screenshot")]
        public ActionResult<> GetPicture()
        {

        }
        
}
