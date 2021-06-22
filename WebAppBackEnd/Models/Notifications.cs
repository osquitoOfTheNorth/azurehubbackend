using System;
using Microsoft.Azure.NotificationHubs;

namespace WebAppBackEnd.Models
{
    public class Notifications
    {
        public static Notifications Instance = new Notifications();

        public NotificationHubClient Hub { get; set; }

        private String ConnectionString = "Endpoint=sb://notificationhubtutorialspfcm.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=pg5k1xh8wc8w8KQq/srY2dbY6EcUFAi83o78LSP++0o=";

        private Notifications()
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString(ConnectionString, "OscarsNotificationHubTutorial");
        }
    }
}
