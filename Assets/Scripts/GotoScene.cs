using UnityEngine;

public class GotoScene : MonoBehaviour {
	public void LoadScene(string sceneName) {
		PlayerPrefs.Save ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (sceneName);
	}
}
