using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoLaunch : MonoBehaviour {

	public void LoadLaunchScene() {
		MDPrefs.SetBool ("autolaunch", true);
		PlayerPrefs.Save ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("launch");
	}
}
