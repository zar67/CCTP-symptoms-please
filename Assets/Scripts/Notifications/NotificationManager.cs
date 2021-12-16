using UnityEngine;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

public enum NotificationType
{
    LEADERBOARD_UPDATE,
    PATIENT_NEEDS_YOU,
    GONE_TOO_LONG,
    NEW_CHAT
}

public class NotificationManager : MonoBehaviour
{
    public const string NOTIFICATIONS_LOADED_DEPENDANCY = "NOTIFICATIONS_LOADED";

    public struct NotificationData
    {
        public string LeaderboardOtherPlayerName;
        public string PatientName;
    }

    public static string SendNotification(NotificationType type, NotificationData data = new NotificationData())
    {
        string channelID = "";
        string title = "";
        string text = "";
        string intent = "";

        int scheduleInHours = 0;

        switch (type)
        {
            case NotificationType.LEADERBOARD_UPDATE:
            {
                channelID = "updates";
                title = "Leaderboard Position Taken!";
                text = $"{data.LeaderboardOtherPlayerName} has taken your place! You have dropped to nth! Play now to claim back your spot.";
                intent = "open_game";
                break;
            }
            case NotificationType.PATIENT_NEEDS_YOU:
            {
                channelID = "reminders";
                title = "Patient Needs You!";
                text = $"{data.PatientName} needs your help! Many more patients are waiting for your advice inside. Help them!";
                intent = "open_game";
                scheduleInHours = 24;
                break;
            }
            case NotificationType.GONE_TOO_LONG:
            {
                channelID = "reminders";
                title = "You've Been Gone So Long!";
                text = "You’ve been gone so long! Refresh your memory with some training!";
                intent = "open_training";
                scheduleInHours = 144;
                break;
            }
            case NotificationType.NEW_CHAT:
            {
                channelID = "updates";
                title = "You Have A New Chat!";
                text = $"{data.PatientName} has sent you an urgent message!";
                intent = "open_chat";
                break;
            }
        }

#if UNITY_ANDROID
        return AndroidNotifications.SendNotification(channelID, title, text, intent, scheduleInHours).ToString();
#elif UNITY_IOS
        return IOSNotifications.SendNotification(channelID, title, text, intent, scheduleInHours);
#else
        return "";
#endif
    }

    private void Awake()
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllScheduledNotifications();

        AndroidNotifications.CreateNotificationChannel("updates", "Updates", "Updates about the state of the game.");
        AndroidNotifications.CreateNotificationChannel("reminders", "Reminders", "Reminders to keep playing and learning.");

        AndroidNotificationIntentData notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
        if (notificationIntentData != null)
        {
            AndroidNotification notification = notificationIntentData.Notification;
            HandleIntentData(notification.IntentData);
        }

#elif UNITY_IOS
        iOSNotificationCenter.RemoveAllScheduledNotifications();

        IOSNotifications.RequestAuthorization(this);

        var notification = iOSNotificationCenter.GetLastRespondedNotification();
        if (notification != null)
        {
            HandleIntentData(notification.Data);
        }
#endif

        SendNotification(NotificationType.GONE_TOO_LONG);
        SendNotification(NotificationType.PATIENT_NEEDS_YOU);
    }

    private void HandleIntentData(string data)
    {
        switch (data)
        {
            case "open_training":
            {
                Debug.Log("OPEN TRAINING");
                break;
            }
            case "open_chat":
            {
                Debug.Log("OPEN CHAT");
                break;
            }
            default:
            {
                break;
            }
        }
    }
}