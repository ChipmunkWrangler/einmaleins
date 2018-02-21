using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandler : MonoBehaviour {
    [SerializeField] string BackSceneName = "choosePlayer";
    bool IsLoadingScene;

	void Update () {
		if (!IsLoadingScene && Input.GetKeyDown(KeyCode.Escape)) { 
			IsLoadingScene = true;
			if (BackSceneName == "EXIT") {
				Application.Quit ();
			} else {
				UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (BackSceneName);
			}
		}  	
	}
}
