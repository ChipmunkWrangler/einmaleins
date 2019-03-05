using CrazyChipmunk;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "InitialGameSceneLoader", menuName = "TimesTables/InitialGameSceneLoader")]
internal class InitialGameSceneLoader : ScriptableObject
{
    [SerializeField] private ReadOnlyString curPlayerName;
    [SerializeField] private RocketColour rocketColour;
    [SerializeField] private RocketPartsPersistentData rocketPartsData;

    public void LoadInitialGameScene()
    {
        SceneManager.LoadSceneAsync(ChooseScene());
    }

    private string ChooseScene()
    {
        return IsRocketBuilt() ? "launch" : "rocketBuilding";
    }

    private bool IsRocketBuilt()
    {
        if (curPlayerName != "")
        {
            rocketPartsData.Load();
            return rocketPartsData.IsRocketBuilt && rocketColour.HasChosenColour();
        }

        return false;
    }
}