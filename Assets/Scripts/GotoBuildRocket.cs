using UnityEngine;
using UnityEngine.SceneManagement;

internal class GotoBuildRocket : MonoBehaviour
{
    public void LoadScene()
    {
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync("rocketBuilding");
    }
}