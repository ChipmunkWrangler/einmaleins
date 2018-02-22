using UnityEngine;
using UnityEngine.SceneManagement;

public class MDVersion : MonoBehaviour
{
    const int MajorVersion = 1;
    const int MinorVersion = 0;
    const int BuildNumber = 0;

    [SerializeField] Questions questions = null;

    bool isChecking;

    public static string GetCurrentVersion()
    {
        return MajorVersion + "." + MinorVersion + "." + BuildNumber;
    }

    public static void WriteNewVersion()
    {
        PlayerPrefs.SetString("version", GetCurrentVersion());
        PlayerPrefs.Save();
    }

    public void CheckVersion()
    {
        if (isChecking)
        {
            return;
        }
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;
        isChecking = true;
        string oldVersion = PlayerPrefs.GetString("version");
        if (oldVersion == GetCurrentVersion())
        {
            return;
        }
        if (SceneManager.GetActiveScene().name != "updateVersion")
        {
            SceneManager.LoadScene("updateVersion");
            return;
        }
        switch (oldVersion)
        {
            case "0.1.14":
                break;
            case "0.1.13":
            case "0.1.12":
            case "0.1.11":
                UpdateFrom_0_1_11_To_0_1_14();
                break;
            case "0.1.10":
            case "0.1.9":
            case "0.1.8":
                UpdateFrom_0_1_8_To_0_1_11();
                break;
            default:
                RestartWithNewVersion();
                break;
        }
        WriteNewVersion();
        isChecking = false;
        SceneManager.LoadScene("choosePlayer");
    }

    void Start()
    {
        CheckVersion();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            CheckVersion();
        }
    }

    void RestartWithNewVersion()
    {
        PlayerPrefs.DeleteAll();
    }

    void UpdateFrom_0_1_11_To_0_1_14()
    {
        var playerNameController = new PlayerNameController();
        playerNameController.Load();
        string oldName = playerNameController.CurName;
        foreach (string playerName in playerNameController.Names)
        {
            playerNameController.CurName = playerName;
            playerNameController.Save();
            questions.gameObject.SetActive(true); // load question list
            foreach (Question question in questions.QuestionArray)
            {
                question.SetNewFromAnswerTime();
            }
            questions.Save();
            questions.gameObject.SetActive(false);
        }
        playerNameController.CurName = oldName;
        playerNameController.Save();
    }

    void UpdateFrom_0_1_8_To_0_1_11()
    {
        const float OldAnswerTimeInitial = 3F + 0.01F;
        var playerNameController = new PlayerNameController();
        playerNameController.Load();
        string oldName = playerNameController.CurName;
        foreach (string playerName in playerNameController.Names)
        {
            playerNameController.CurName = playerName;
            playerNameController.Save();
            Debug.Log("Updating question for " + playerName);
            questions.gameObject.SetActive(true); // load question list
            foreach (Question question in questions.QuestionArray)
            {
                question.UpdateInitialAnswerTime(OldAnswerTimeInitial);
            }
            questions.Save();
            questions.gameObject.SetActive(false);
        }
        playerNameController.CurName = oldName;
        playerNameController.Save();
    }
}
