using UnityEngine;

[CreateAssetMenu(fileName = "InitialGameSceneLoader", menuName = "TimesTables/InitialGameSceneLoader")]
class InitialGameSceneLoader : ScriptableObject
{
    RocketPartsPersistentData rocketPartsData = new RocketPartsPersistentData();

    public void LoadInitialGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ChooseScene());
    }

    string ChooseScene()
    {
        return IsRocketBuilt() ? "launch" : "rocketBuilding";
    }

    bool IsRocketBuilt()
    {
        if (PlayerNameController.IsPlayerSet())
        {
            rocketPartsData.Load();
            return rocketPartsData.IsRocketBuilt && ChooseRocketColour.HasChosenColour();
        }
        return false;
    }
}
