using UnityEngine;

#if UNITY_ANDROID
using Unity.Notifications.Android;
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

    public static int SendNotification(NotificationType type, NotificationData data = new NotificationData())
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
        return AndroidNotifications.SendNotification(channelID, title, text, intent, scheduleInHours);
#elif UNITY_IOS
        return IOSNotifications.SendNotification();
#else
        return -1;
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
            int id = notificationIntentData.Id;
            string channel = notificationIntentData.Channel;
            AndroidNotification notification = notificationIntentData.Notification;

            switch (notification.IntentData)
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
#endif
    }

    private void OnApplicationQuit()
    {
        SendNotification(NotificationType.GONE_TOO_LONG);
        SendNotification(NotificationType.PATIENT_NEEDS_YOU);
    }
}