using UnityEngine;
using UnityEngine.SceneManagement;

internal class KeyHandler : MonoBehaviour
{
    [SerializeField] private string backSceneName = "choosePlayer";
    private bool isLoadingScene;

    private void Update()
    {
        if (!isLoadingScene && Input.GetKeyDown(KeyCode.Escape))
        {
            isLoadingScene = true;
            if (backSceneName == "EXIT")
                Application.Quit();
            else
                SceneManager.LoadSceneAsync(backSceneName);
        }
    }
}