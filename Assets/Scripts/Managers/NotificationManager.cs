#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

using System.Collections;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
#if UNITY_ANDROID
    private static string CHANNEL_ID = "notis";

    private void Start()
    {
        if (!PlayerPrefs.HasKey("NotisChannel_Created"))
        {
            var group = new AndroidNotificationChannelGroup()
            {
                Id = "Main",
                Name = "Main Notification"
            };
            AndroidNotificationCenter.RegisterNotificationChannelGroup(group);

            var channel = new AndroidNotificationChannel()
            {
                Id = CHANNEL_ID,
                Name = "Default Channel",
                Importance = Importance.Default,
                Description = "Generic Notification",
                Group = "Main"
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            StartCoroutine(RequestPermission());

            PlayerPrefs.SetString("NotisChannel_Created", "y");
            PlayerPrefs.Save();
        }
        else 
        {
            ScheduleNotis();
        }
    }

    private IEnumerator RequestPermission()
    {
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
        {
            yield return new WaitForEndOfFrame();
        }
        ScheduleNotis();
    }

    private void ScheduleNotis()
    {
        AndroidNotificationCenter.CancelAllScheduledNotifications();
        var notification10Minutes = new AndroidNotification();
        notification10Minutes.Title = "La Batalla te espera, VUELVE AL COMBATE!";
        notification10Minutes.Text = "Manuel Cabrerizo";
        notification10Minutes.FireTime = System.DateTime.Now.AddMinutes(1);
        AndroidNotificationCenter.SendNotification(notification10Minutes, CHANNEL_ID);
        Debug.Log("Notifications Scheduled!!");
    }
#endif
}

