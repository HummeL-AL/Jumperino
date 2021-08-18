using UnityEngine;
using Unity.Notifications.Android;

public class PushManager : MonoBehaviour
{
    public AndroidNotificationChannel ANC;

    public int notification_ID;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Creating notification channel...");
        ANC = new AndroidNotificationChannel()
        {
            Id = "jumperino",
            Name = "Jumperino notification channel",
            Importance = Importance.Default,
            Description = "Game notifications",
        };


        Debug.Log("Registering notification channel...");
        AndroidNotificationCenter.RegisterNotificationChannel(ANC);


        Debug.Log("Creating notification...");
        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Road to new record!",
            Text = "Let's make some jumps!",
            SmallIcon = "game_icon_small",
            LargeIcon = "game_icon_large",
            FireTime = System.DateTime.Now.AddDays(1),
        };


        Debug.Log("Notification sending...");
        AndroidNotificationCenter.SendNotificationWithExplicitID(notification, "channel_id", notification_ID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
