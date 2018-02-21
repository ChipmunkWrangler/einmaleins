using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForegroundDisplaySettings
{
    public float scale = 1.0F;
    public float yPos;
}

public class ShowSolarSystem : MonoBehaviour
{
    [SerializeField] float VerticalPadding = 0.1F; // 1.0 would be the whole screen
    [SerializeField] Renderer Rocket = null;
    [SerializeField] GameObject ParticleParent = null;
    [SerializeField] Renderer[] Planets = null;
    [SerializeField] Renderer Earth = null;
    [SerializeField] float MinParticleScale = 0.1F;
    [SerializeField] GameObject RecordLine = null;
    [SerializeField] float ZoomInTime = 5.0F;
    [SerializeField] float ZoomOutTime = 5.0F;
    [SerializeField] float PreDelay = 2.0F;
    [SerializeField] float PostDelay = 2.0F;
    [SerializeField] float[] PlanetZooms = null;
    [SerializeField] float[] PlanetYs = null;
    Vector3 OriginalScale;
    Transform ParticleSystemTransform;

    public float ZoomToPlanet(int i)
    {
        GameObject planet = Planets[i].gameObject;
        Transform oldTransform = planet.transform;
        float zoomedScale = PlanetZooms[i];
        iTween.MoveTo(planet, iTween.Hash("y", PlanetYs[i], "time", ZoomInTime, "delay", PreDelay, "islocal", true));
        iTween.ScaleTo(planet, iTween.Hash("scale", new Vector3(zoomedScale, zoomedScale, 1.0F), "time", ZoomInTime, "delay", PreDelay));
        float duration = PreDelay + ZoomInTime + PostDelay;
        // then zoom out again
        iTween.MoveTo(planet, iTween.Hash("y", oldTransform.localPosition.y, "time", ZoomOutTime, "delay", PreDelay + ZoomInTime, "islocal", true));
        iTween.ScaleTo(planet, iTween.Hash("scale", oldTransform.localScale, "time", ZoomOutTime, "delay", PreDelay + ZoomInTime));
        duration += ZoomOutTime;
        return duration;
    }

    public Renderer GetPlanet(int i) => Planets[i];

    void Start()
    {
        OriginalScale = transform.localScale;
        AdjustPlanetPositions();
    }

    void Update()
    {
        if (ParticleSystemTransform == null)
        {
            ParticleSystem sys = ParticleParent.GetComponentInChildren<ParticleSystem>(false);
            if (sys != null)
            {
                ParticleSystemTransform = sys.transform;
            }
        }
        float viewportTop = Camera.main.WorldToViewportPoint(Rocket.bounds.max).y + VerticalPadding;
        if (viewportTop > 1.0F)
        {
            transform.localScale /= viewportTop;
            ParticleSystemTransform.localScale /= viewportTop;
            RecordLine.transform.localScale *= viewportTop;
            float maintainMinScaleFactor = MinParticleScale / ParticleSystemTransform.localScale.y;
            if (maintainMinScaleFactor > 1.0F)
            {
                ParticleSystemTransform.localScale *= maintainMinScaleFactor;
                Rocket.transform.localScale *= maintainMinScaleFactor;
            }
        }
    }

    public void Reset()
    {
        transform.localScale = OriginalScale;
    }

    void AdjustPlanetPositions()
    {
        float earthAndRocketOffsets = Earth.bounds.extents.y + Rocket.bounds.size.y;
        foreach (var planet in Planets)
        {
            Vector3 newPos = planet.transform.position;  // planet.transform.y could be replaced by TargetPlanet.heights, except that newPos is already in a manipulated space due to its parent
            float offset = planet.bounds.extents.y + earthAndRocketOffsets;
            if (planet.transform.localPosition.y < 0)
            {
                offset = -offset;
            }
            newPos.y += offset;
            planet.transform.position = newPos;
        }
    }
}
