using UnityEngine;

internal class ShowSolarSystem : MonoBehaviour
{
    [SerializeField] private Renderer earth;
    [SerializeField] private float minParticleScale = 0.1F;
    private Vector3 originalScale;
    [SerializeField] private GameObject particleParent;
    private Transform particleSystemTransform;
    [SerializeField] private Renderer[] planets;
    [SerializeField] private float[] planetYs;
    [SerializeField] private float[] planetZooms;
    [SerializeField] private float postDelay = 2.0F;
    [SerializeField] private float preDelay = 2.0F;
    [SerializeField] private GameObject recordLine;
    [SerializeField] private Renderer rocket;
    [SerializeField] private float verticalPadding = 0.1F; // 1.0 would be the whole screen
    [SerializeField] private float zoomInTime = 5.0F;
    [SerializeField] private float zoomOutTime = 5.0F;

    public Renderer GetPlanet(int i)
    {
        return planets[i];
    }

    public float ZoomToPlanet(int i)
    {
        var planet = planets[i].gameObject;
        var oldTransform = planet.transform;
        var zoomedScale = planetZooms[i];
        iTween.MoveTo(planet, iTween.Hash("y", planetYs[i], "time", zoomInTime, "delay", preDelay, "islocal", true));
        iTween.ScaleTo(planet,
            iTween.Hash("scale", new Vector3(zoomedScale, zoomedScale, 1.0F), "time", zoomInTime, "delay", preDelay));
        var duration = preDelay + zoomInTime + postDelay;
        // then zoom out again
        iTween.MoveTo(planet,
            iTween.Hash("y", oldTransform.localPosition.y, "time", zoomOutTime, "delay", preDelay + zoomInTime,
                "islocal", true));
        iTween.ScaleTo(planet,
            iTween.Hash("scale", oldTransform.localScale, "time", zoomOutTime, "delay", preDelay + zoomInTime));
        duration += zoomOutTime;
        return duration;
    }

    public void Reset()
    {
        transform.localScale = originalScale;
    }

    private void Start()
    {
        originalScale = transform.localScale;
        AdjustPlanetPositions();
    }

    private void Update()
    {
        if (particleSystemTransform == null)
        {
            var sys = particleParent.GetComponentInChildren<ParticleSystem>(false);
            if (sys != null) particleSystemTransform = sys.transform;
        }

        var viewportTop = Camera.main.WorldToViewportPoint(rocket.bounds.max).y + verticalPadding;
        if (viewportTop > 1.0F)
        {
            transform.localScale /= viewportTop;
            particleSystemTransform.localScale /= viewportTop;
            recordLine.transform.localScale *= viewportTop;
            var maintainMinScaleFactor = minParticleScale / particleSystemTransform.localScale.y;
            if (maintainMinScaleFactor > 1.0F)
            {
                particleSystemTransform.localScale *= maintainMinScaleFactor;
                rocket.transform.localScale *= maintainMinScaleFactor;
            }
        }
    }

    private void AdjustPlanetPositions()
    {
        var earthAndRocketOffsets = earth.bounds.extents.y + rocket.bounds.size.y;
        foreach (var planet in planets)
        {
            var newPos =
                planet.transform
                    .position; // planet.transform.y could be replaced by TargetPlanet.heights, except that newPos is already in a manipulated space due to its parent
            var offset = planet.bounds.extents.y + earthAndRocketOffsets;
            if (planet.transform.localPosition.y < 0) offset = -offset;
            newPos.y += offset;
            planet.transform.position = newPos;
        }
    }
}