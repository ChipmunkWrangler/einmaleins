using CrazyChipmunk;
using UnityEngine;

[CreateAssetMenu(fileName = "InitialGameSceneLoader", menuName = "TimesTables/InitialGameSceneLoader")]
class InitialGameSceneLoader : ScriptableObject
{
    [SerializeField] RocketColour rocketColour = null;
    [SerializeField] RocketPartsPersistentData rocketPartsData = null;
    [SerializeField] ReadOnlyString curPlayerName = null;

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
        if (curPlayerName != "")
        {
            rocketPartsData.Load();
            return rocketPartsData.IsRocketBuilt && rocketColour.HasChosenColour();
        }
        return false;
    }
}