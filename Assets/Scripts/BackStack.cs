using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackStack : MonoBehaviour {
	public static string Pop(string defaultScene = "main") {
		List<string> backStack = new List<string>(MDPrefs.GetStringArray ("backStack"));
		string sceneName = defaultScene;
		if (backStack.Count > 0) {
			sceneName = backStack [backStack.Count - 1];
			backStack.RemoveAt (backStack.Count - 1);
			Clear ();
			if (backStack.Count > 0) {
				MDPrefs.SetStringArray ("backStack", backStack.ToArray ());
			}
		}
		return sceneName;
	}

	public static void PushCurrentScene() {
		List<string> backStack = new List<string>(MDPrefs.GetStringArray ("backStack"));
		backStack.Add (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		MDPrefs.SetStringArray ("backStack", backStack.ToArray ());
	}

	public static void Clear() {
		MDPrefs.DeleteKey("backStack");
	}
}
