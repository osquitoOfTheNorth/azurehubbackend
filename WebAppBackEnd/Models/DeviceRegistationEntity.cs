using System;
namespace WebAppBackEnd.Models
{
    public class DeviceRegistationEntity
    {
        public string Platform { get; set; }
        public string Handle { get; set; }
        public string userId { get; set; }

        public DeviceRegistationEntity()
        {
        }
    }
}
