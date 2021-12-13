#if UNITY_ANDROID

using Unity.Notifications.Android;

public class AndroidNotifications
{
    public static void CreateNotificationChannel(string channelID, string name, string description, Importance importance = Importance.Default)
    {
        var newNotification = new AndroidNotificationChannel()
        {
            Id = channelID,
            Name = name,
            Importance = importance,
            Description = description,
        };

        AndroidNotificationCenter.RegisterNotificationChannel(newNotification);
    }

    public static int SendNotification(string channelID, string title, string text, string intent, int scheduleInHours = 0)
    {
        var notification = new AndroidNotification
        {
            Title = title,
            Text = text,
            FireTime = System.DateTime.Now.AddSeconds(scheduleInHours),
            IntentData = intent
            //SmallIcon = "my_custom_icon_id",
            //LargeIcon = "my_custom_large_icon_id"
        };

        return AndroidNotificationCenter.SendNotification(notification, channelID);
    }
}
#endif