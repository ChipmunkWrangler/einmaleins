using UnityEngine;

public class ShowSolarSystem : MonoBehaviour
{
    [SerializeField] float verticalPadding = 0.1F; // 1.0 would be the whole screen
    [SerializeField] Renderer rocket = null;
    [SerializeField] GameObject particleParent = null;
    [SerializeField] Renderer[] planets = null;
    [SerializeField] Renderer earth = null;
    [SerializeField] float minParticleScale = 0.1F;
    [SerializeField] GameObject recordLine = null;
    [SerializeField] float zoomInTime = 5.0F;
    [SerializeField] float zoomOutTime = 5.0F;
    [SerializeField] float preDelay = 2.0F;
    [SerializeField] float postDelay = 2.0F;
    [SerializeField] float[] planetZooms = null;
    [SerializeField] float[] planetYs = null;
    Vector3 originalScale;
    Transform particleSystemTransform;

    public Renderer GetPlanet(int i) => planets[i];

    public float ZoomToPlanet(int i)
    {
        GameObject planet = planets[i].gameObject;
        Transform oldTransform = planet.transform;
        float zoomedScale = planetZooms[i];
        iTween.MoveTo(planet, iTween.Hash("y", planetYs[i], "time", zoomInTime, "delay", preDelay, "islocal", true));
        iTween.ScaleTo(planet, iTween.Hash("scale", new Vector3(zoomedScale, zoomedScale, 1.0F), "time", zoomInTime, "delay", preDelay));
        float duration = preDelay + zoomInTime + postDelay;
        // then zoom out again
        iTween.MoveTo(planet, iTween.Hash("y", oldTransform.localPosition.y, "time", zoomOutTime, "delay", preDelay + zoomInTime, "islocal", true));
        iTween.ScaleTo(planet, iTween.Hash("scale", oldTransform.localScale, "time", zoomOutTime, "delay", preDelay + zoomInTime));
        duration += zoomOutTime;
        return duration;
    }

    public void Reset()
    {
        transform.localScale = originalScale;
    }

    void Start()
    {
        originalScale = transform.localScale;
        AdjustPlanetPositions();
    }

    void Update()
    {
        if (particleSystemTransform == null)
        {
            ParticleSystem sys = particleParent.GetComponentInChildren<ParticleSystem>(false);
            if (sys != null)
            {
                particleSystemTransform = sys.transform;
            }
        }
        float viewportTop = Camera.main.WorldToViewportPoint(rocket.bounds.max).y + verticalPadding;
        if (viewportTop > 1.0F)
        {
            transform.localScale /= viewportTop;
            particleSystemTransform.localScale /= viewportTop;
            recordLine.transform.localScale *= viewportTop;
            float maintainMinScaleFactor = minParticleScale / particleSystemTransform.localScale.y;
            if (maintainMinScaleFactor > 1.0F)
            {
                particleSystemTransform.localScale *= maintainMinScaleFactor;
                rocket.transform.localScale *= maintainMinScaleFactor;
            }
        }
    }

    void AdjustPlanetPositions()
    {
        float earthAndRocketOffsets = earth.bounds.extents.y + rocket.bounds.size.y;
        foreach (var planet in planets)
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
