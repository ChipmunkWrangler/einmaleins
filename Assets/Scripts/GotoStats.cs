using CrazyChipmunk;
using UnityEngine;

class GotoStats : MonoBehaviour
{
    [SerializeField] private Prefs prefs;

    private void OnEnable()
    {
        if (QuestionGenerator.GetQuestionType(prefs) != "multiplication")
        {
            gameObject.SetActive(false);
        }
    }

    public void LoadStatsScene()
    {
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("stats");
    }
}