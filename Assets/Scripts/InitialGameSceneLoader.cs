using CrazyChipmunk;
using UnityEngine;

[CreateAssetMenu(fileName = "InitialGameSceneLoader", menuName = "TimesTables/InitialGameSceneLoader")]
class InitialGameSceneLoader : ScriptableObject
{
    [SerializeField] PersistentStringReference curPlayerName = null;

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
        if (curPlayerName.Value != "")
        {
            rocketPartsData.Load();
            return rocketPartsData.IsRocketBuilt && ChooseRocketColour.HasChosenColour();
        }
        return false;
    }
}