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
    public class WeatherForecastController : ControllerBase
    {

        private NotificationHubClient hub = Notifications.Instance.Hub;

   

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("/register")]
        public async Task<ActionResult<String>> Post([FromBody] Token handle)
        {
            string newRegistrationId = null;

            // make sure there are no existing registrations for this push handle (used for iOS and Android)
            if (handle != null)
            {
                var registrations = await hub.GetRegistrationsByChannelAsync(handle.PNSIdentifier, 100);

                foreach (RegistrationDescription registration in registrations)
                {
                    if (newRegistrationId == null)
                    {
                        newRegistrationId = registration.RegistrationId;
                    }
                    else
                    {
                        await hub.DeleteRegistrationAsync(registration);
                    }
                }
            }

            if (newRegistrationId == null)
                newRegistrationId = await hub.CreateRegistrationIdAsync();

            RegistrationUpdateResponse response = new RegistrationUpdateResponse();
            response.registrationId = newRegistrationId;

            return Ok(new { response });

        }

        [HttpPost]
        [Route("/push")]
        public async Task<ActionResult<String>> Push([FromBody] MessageDetails msgDetails)
        {
            string[] userTag = new string[1];
            userTag[0] = "username:" + msgDetails.user;

            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            HttpStatusCode ret = HttpStatusCode.InternalServerError;


            var notification = new Dictionary<string, string> { { "message", "Hello, " + msgDetails.message } };
            outcome = await Notifications.Instance.Hub.SendTemplateNotificationAsync(notification, userTag);
      
            if (outcome != null)
            {
                if (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown)))
                {
                    ret = HttpStatusCode.OK;
                }
            }

            return Ok(ret);
        }

        [HttpPut]
        [Route("/register/{id}")]
        public  async Task<ActionResult<String>> Put(string id, DeviceRegistationEntity entity)
        {
            RegistrationDescription registration = null;

            switch (entity.Platform)
            {
                case "apns":
                    var alertTemplate = "{\"aps\":{\"alert\":\"$(message)\"}}";
                    registration = new AppleTemplateRegistrationDescription(entity.Handle, alertTemplate);
                    break;
                case "fcm":
                    var template = "{\"data\":{\"message\":\"$(message)\"}}";
                    registration = new FcmTemplateRegistrationDescription(entity.Handle, template);
                    break;
                default:
                    throw new Exception("something bad happened");
            }

            registration.RegistrationId = id;
            var username = entity.userId;

            // add check if user is allowed to add these tags
            registration.Tags = new HashSet<string>();
            registration.Tags.Add("username:" + username);
      
             await hub.CreateOrUpdateRegistrationAsync(registration);
         
            return Ok();
        }

    }
}
