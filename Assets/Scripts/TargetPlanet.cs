using CrazyChipmunk;
using UnityEngine;

[CreateAssetMenu(menuName = "TimesTables/TargetPlanet")]
class TargetPlanet : ScriptableObject
{
    [SerializeField] Prefs prefs = null;

    public static readonly float[] Heights =
    {
        7.8e+07F,
        6.3e+08F,
        1.287e+09F,
        2.73e+09F,
        4.357e+09F,
        5.772e+09F
    };

    const string TargetKey = "targetPlanet";
    const string LastReachedKey = "lastReachedPlanet";
    const float FinalHeight = 9999999999F;

    static int targetPlanetIdx = -1;
    static int lastReachedPlanetIdx = -2;

    public static void Reset()
    {
        targetPlanetIdx = -1;
        lastReachedPlanetIdx = -2;
    }

    public int GetLastReachedIdx()
    {
        if (lastReachedPlanetIdx < -1)
        {
            lastReachedPlanetIdx = prefs.GetInt(LastReachedKey, -1);
        }
        return lastReachedPlanetIdx;
    }

    public void SetLastReachedIdx(int planetIdx)
    {
        prefs.SetInt(LastReachedKey, planetIdx);
        lastReachedPlanetIdx = planetIdx;
    }

    public void TargetNextPlanet()
    {
        SetTargetPlanetIdx(GetTargetPlanetIdx() + 1);
    }

    public int GetTargetPlanetIdx()
    {
        if (targetPlanetIdx < 0)
        {
            targetPlanetIdx = prefs.GetInt(TargetKey, 0);
        }
        return targetPlanetIdx;
    }

    public static int GetMaxPlanetIdx() => Heights.Length - 1;
    public static float GetPlanetHeight(int i) => (i < Heights.Length) ? Heights[i] : FinalHeight;

    public void SetTargetPlanetIdx(int newIdx)
    {
        prefs.SetInt(TargetKey, newIdx);
        targetPlanetIdx = newIdx;
    }
}
