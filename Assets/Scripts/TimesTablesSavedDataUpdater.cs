using CrazyChipmunk;
using UnityEngine;

internal class TimesTablesSavedDataUpdater : SavedDataUpdater
{
    [SerializeField] private VariableString playerName;
    [SerializeField] private PlayerNameController playerNameController;
    [SerializeField] private Questions questions;

    public override void UpdateData(string fromVersion, string toVersion)
    {
        switch (fromVersion)
        {
            case "0.1.14":
            case "1.0.0":
            case "1.1":
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

    private void GiveUpAndDestroyData()
    {
        PlayerPrefs.DeleteAll();
    }

    private void UpdateFrom_0_1_11_To_0_1_14()
    {
        playerNameController.Load();
        string oldName = playerName;
        foreach (var name in playerNameController.Names)
        {
            playerName.Value = name;
            playerNameController.Save();
            questions.gameObject.SetActive(true); // load question list
            foreach (var question in questions.QuestionArray) question.SetNewFromAnswerTime();

            questions.Save();
            questions.gameObject.SetActive(false);
        }

        playerName.Value = oldName;
        playerNameController.Save();
    }

    private void UpdateFrom_0_1_8_To_0_1_11()
    {
        const float OldAnswerTimeInitial = 3F + 0.01F;
        playerNameController.Load();
        string oldName = playerName;
        foreach (var name in playerNameController.Names)
        {
            playerName.Value = name;
            playerNameController.Save();
            Debug.Log("Updating question for " + name);
            questions.gameObject.SetActive(true); // load question list
            foreach (var question in questions.QuestionArray) question.UpdateInitialAnswerTime(OldAnswerTimeInitial);

            questions.Save();
            questions.gameObject.SetActive(false);
        }

        playerName.Value = oldName;
        playerNameController.Save();
    }
}