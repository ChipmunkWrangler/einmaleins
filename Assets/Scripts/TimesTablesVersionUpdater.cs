using UnityEngine;
using CrazyChipmunk;

class TimesTablesVersionUpdater : VersionUpdater
{
    [SerializeField] Questions questions = null;

    public override void UpdateVersion(string fromVersion, string toVersion)
    {
        switch (fromVersion)
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
                UpdateFrom_0_1_11_To_0_1_14();
                break;
            default:
                GiveUpAndDestroyData();
                break;
        }
    }

    void GiveUpAndDestroyData()
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