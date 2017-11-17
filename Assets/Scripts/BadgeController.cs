using UnityEngine;

public class BadgeController : MonoBehaviour {
	[SerializeField] EffortTracker effortTracker = null; // This is questionable since there can be multiple players...


	void Start() {
		Debug.Log ("Baz");
//		if (pauseStatus) {
			ScheduleTomorrowBadge ();
			UpdateBadge ();
//		}
	}

	void UpdateBadge() {
//		UnityEngine.iOS.NotificationServices.ClearLocalNotifications ();
		int badgeNumber = effortTracker.GetNumQuizzesLeftForToday ();
		if (badgeNumber <= 0) {
			badgeNumber = -1;
		}
		Debug.Log ("UpdateBadge "  + badgeNumber);
		UnityEngine.iOS.LocalNotification today = new UnityEngine.iOS.LocalNotification();
		today.applicationIconBadgeNumber = badgeNumber;
		today.alertBody = "Foo";
		UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow (today);
	}

	void ScheduleTomorrowBadge() {
//		UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications ();
		UnityEngine.iOS.LocalNotification tomorrow = new UnityEngine.iOS.LocalNotification();
		tomorrow.applicationIconBadgeNumber = EffortTracker.MIN_QUIZZES_PER_DAY * 2;
		tomorrow.fireDate = System.DateTime.Now + System.TimeSpan.FromMinutes (1.0);
		tomorrow.alertBody = "Bar";
		UnityEngine.iOS.NotificationServices.ScheduleLocalNotification (tomorrow);
	}
}
