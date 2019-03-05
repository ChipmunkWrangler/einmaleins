using UnityEngine;
using UnityEngine.Assertions;

internal class RocketBuilder : MonoBehaviour
{
    [SerializeField] private float buildingDelay = 1.0F;
    [SerializeField] private float buildingTime = 5.0F;
    [SerializeField] private ParticleSystem buildParticles;
    [SerializeField] private float builtY;
    [SerializeField] private GameObject chooseColourButton;
    [SerializeField] private RocketColour chooseRocketColourData;
    [SerializeField] private RocketPartCounter counter;

    private readonly iTween.EaseType[] easeTypes =
    {
        iTween.EaseType.linear,
        iTween.EaseType.easeInOutCubic,
        iTween.EaseType.easeOutQuad,
        iTween.EaseType.easeInOutQuint,
        iTween.EaseType.easeOutExpo,
        iTween.EaseType.linear,
        iTween.EaseType.easeOutExpo
    };

    [SerializeField] private ParticleSystem[] exhaustParticles;
    [SerializeField] private float hiddenY = -2.0F;
    [SerializeField] private GameObject launchButton;
    [SerializeField] private float maxY = 1.0F;
    [SerializeField] private GameObject rocketPartsWidget;
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private float upgradeFlightTime = 5.0F;

    public void OnUpgrade()
    {
        Assert.AreEqual(exhaustParticles.Length - 1, RocketParts.Instance.MaxUpgradeLevel);
        if (RocketParts.Instance.Upgrade())
        {
            counter.Spend(RocketParts.Instance.NumParts + RocketParts.Instance.NumPartsRequired,
                RocketParts.Instance.NumParts);
            StartEngine();
            iTween.MoveTo(gameObject,
                iTween.Hash("y", maxY, "time", upgradeFlightTime, "delay", buildingDelay, "easetype",
                    easeTypes[RocketParts.Instance.UpgradeLevel], "oncomplete", "Descend"));
        }
    }

    private void Start()
    {
        chooseColourButton.SetActive(false);
        if (RocketParts.Instance.IsRocketBuilt)
        {
            SetY(builtY);
            rocketPartsWidget.SetActive(RocketParts.Instance.UpgradeLevel < RocketParts.Instance.MaxUpgradeLevel - 1);
            if (!RocketParts.Instance.HasEnoughPartsToUpgrade) DoneBuildingOrUpgrading();
        }
        else
        {
            SetY(hiddenY);
            rocketPartsWidget.SetActive(false);
            Build();
        }
    }

    private void StartEngine()
    {
        for (var i = 0; i < exhaustParticles.Length; ++i)
            if (exhaustParticles[i] != null)
                exhaustParticles[i].gameObject.SetActive(false);
        var upgradeLevel = RocketParts.Instance.UpgradeLevel;
        exhaustParticles[upgradeLevel].gameObject.SetActive(true);
        exhaustParticles[upgradeLevel].Play();
    }

    private void Descend()
    {
        GotoBasePos("DoneUpgrading");
    }

    private void DoneUpgrading()
    {
        DoneBuildingOrUpgrading();
    }

    private void GotoBasePos(string onComplete)
    {
        iTween.MoveTo(gameObject,
            iTween.Hash("y", builtY, "time", buildingTime, "delay", buildingDelay, "easetype",
                iTween.EaseType.easeOutQuad, "oncomplete", onComplete));
    }

    private void Build()
    {
        GotoBasePos("DoneBuilding");
        buildParticles.Play();
        StartEngine();
    }

    private void DoneBuilding()
    {
        RocketParts.Instance.IsRocketBuilt = true;
        buildParticles.Stop();
        DoneBuildingOrUpgrading();
    }

    private void DoneBuildingOrUpgrading()
    {
        exhaustParticles[RocketParts.Instance.UpgradeLevel].Stop();
        upgradeButton.Hide();
        if (chooseRocketColourData.HasChosenColour())
        {
            launchButton.SetActive(true);
        }
        else
        {
            launchButton.SetActive(false);
            chooseColourButton.SetActive(true);
        }
    }

    private void SetY(float y)
    {
        var pos = gameObject.transform.position;
        pos.y = y;
        gameObject.transform.position = pos;
    }
}