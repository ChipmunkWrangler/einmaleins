using CrazyChipmunk;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class GotoStats : MonoBehaviour
{
    [SerializeField] private Prefs prefs;

    private void OnEnable()
    {
        if (QuestionGenerator.GetQuestionType(prefs) != "multiplication") gameObject.SetActive(false);
    }

    public void LoadStatsScene()
    {
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync("stats");
    }
}