#if UNITY_IOS

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.iOS;
using UnityEngine;

public class IOSNotifications
{
    public static string SendNotification(string channelID, string title, string text, string intent, int scheduleInHours = 0)
    {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(scheduleInHours, 0, 0),
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            Title = title,
            Body = text,
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Sound,
            CategoryIdentifier = channelID,
            Data = intent,
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
        return notification.Identifier;
    }

    public static void RequestAuthorization(NotificationManager manager)
    {
        manager.StartCoroutine(RequestAuthorizationEnumerator());
    }

    private static IEnumerator RequestAuthorizationEnumerator()
    {
        AuthorizationOption authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        var request = new AuthorizationRequest(authorizationOption, true);
        
        while (!request.IsFinished)
        {
            yield return null;
        };

        string res = "\n RequestAuthorization:";
        res += "\n finished: " + request.IsFinished;
        res += "\n granted :  " + request.Granted;
        res += "\n error:  " + request.Error;
        res += "\n deviceToken:  " + request.DeviceToken;
        Debug.Log(res);
    }
}
#endif