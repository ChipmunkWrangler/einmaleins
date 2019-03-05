using System;
using UnityEngine;

[Serializable]
internal class TargetPlanetPersistentData : ScriptableObject
{
    [SerializeField] private TargetPlanet targetPlanet;

    public int TargetPlanetIdx { get; set; }
    public int LastReachedPlanetIdx { get; set; }

    public void Load()
    {
        TargetPlanetIdx = targetPlanet.GetTargetPlanetIdx();
        LastReachedPlanetIdx = targetPlanet.GetLastReachedIdx();
    }

    public void Save()
    {
        targetPlanet.SetTargetPlanetIdx(TargetPlanetIdx);
        targetPlanet.SetLastReachedIdx(LastReachedPlanetIdx);
    }
}