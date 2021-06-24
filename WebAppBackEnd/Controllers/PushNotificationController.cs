using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using Microsoft.Extensions.Logging;
using WebAppBackEnd.Models;

namespace WebAppBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PushNotificationController : ControllerBase
    {

        private NotificationHubClient hub = Notifications.Instance.Hub;



        private readonly ILogger<PushNotificationController> _logger;

        public PushNotificationController(ILogger<PushNotificationController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("/register")]
        public async Task<ActionResult<String>> Post([FromBody] Token handle)
        {
  

            return Ok(new {  });

        }

        [HttpPost]
        [Route("/push")]
        public async Task<ActionResult<String>> Push([FromBody] MessageDetails msgDetails)
        {
       
            return Ok(ret);
        }

        [HttpPut]
        [Route("/register/{id}")]
        public async Task<ActionResult<String>> Put(string id, DeviceRegistationEntity entity)
        {


            return Ok();
        }

        [HttpPut]
        [Route("/unregister/{id}")]
        public async Task<ActionResult<String>> Unregister(string id, DeviceRegistationEntity entity)
        {

            return Ok();

        }
    }
}
