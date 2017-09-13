using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoLaunch : MonoBehaviour {

	public void LoadLaunchScene() {
		MDPrefs.SetBool ("autolaunch", true);
		BackStack.PushCurrentScene();
		PlayerPrefs.Save ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("launch");
	}
}
