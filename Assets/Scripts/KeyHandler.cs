using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandler : MonoBehaviour {
    [SerializeField] string backSceneName = "choosePlayer";
    bool isLoadingScene;

	void Update () {
		if (!isLoadingScene && Input.GetKeyDown(KeyCode.Escape)) { 
			isLoadingScene = true;
			if (backSceneName == "EXIT") {
				Application.Quit ();
			} else {
				UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (backSceneName);
			}
		}  	
	}
}
