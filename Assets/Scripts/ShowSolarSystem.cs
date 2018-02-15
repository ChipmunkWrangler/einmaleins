using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class ForegroundDisplaySettings {
	public float scale = 1.0f;
	public float yPos = 0;
}

public class ShowSolarSystem : MonoBehaviour {
	[SerializeField] float verticalPadding = 0.1f; // 1.0 would be the whole screen
	[SerializeField] Renderer rocket = null;
	[SerializeField] GameObject particleParent = null;
	[SerializeField] Renderer[] planets = null;
	[SerializeField] Renderer earth = null;
	[SerializeField] float minParticleScale = 0.1f;
	[SerializeField] GameObject recordLine = null;
	[SerializeField] float zoomInTime = 5.0f;
	[SerializeField] float zoomOutTime = 5.0f;
	[SerializeField] float preDelay = 2.0f;
	[SerializeField] float postDelay = 2.0f;
	[SerializeField] float[] planetZooms = null;
	[SerializeField] float[] planetYs = null;
	Vector3 originalScale;
	Transform particleSystemTransform;

	public float ZoomToPlanet(int i, bool thenZoomOut) {
		GameObject planet = planets [i].gameObject;
		Transform oldTransform = planet.transform;
		float zoomedScale = planetZooms [i];
		iTween.MoveTo(planet, iTween.Hash("y", planetYs[i], "time", zoomInTime, "delay", preDelay, "islocal", true));	
		iTween.ScaleTo(planet, iTween.Hash("scale", new Vector3(zoomedScale, zoomedScale, 1.0f), "time", zoomInTime, "delay", preDelay));
		float duration = preDelay + zoomInTime + postDelay;
		if (thenZoomOut) {
			iTween.MoveTo (planet, iTween.Hash ("y", oldTransform.localPosition.y, "time", zoomOutTime, "delay", preDelay + zoomInTime, "islocal", true));	
			iTween.ScaleTo (planet, iTween.Hash ("scale", oldTransform.localScale, "time", zoomOutTime, "delay", preDelay + zoomInTime));
			duration += zoomOutTime;
		}
		return duration;
	}

	public Renderer GetPlanet(int i) => planets [i];

	void Start() {
		originalScale = transform.localScale;
		AdjustPlanetPositions ();
	}

	void Update() {
		if (particleSystemTransform == null) {
			ParticleSystem sys = particleParent.GetComponentInChildren<ParticleSystem> (false);
			if (sys != null) {
				particleSystemTransform = sys.transform;
			}
		}
		float viewportTop = Camera.main.WorldToViewportPoint (rocket.bounds.max).y + verticalPadding;
		if (viewportTop > 1.0f) {
			transform.localScale /= viewportTop;
			particleSystemTransform.localScale /= viewportTop;
			recordLine.transform.localScale *= viewportTop;
			float maintainMinScaleFactor = minParticleScale / particleSystemTransform.localScale.y;
			if (maintainMinScaleFactor > 1.0f) {
				particleSystemTransform.localScale *= maintainMinScaleFactor;
				rocket.transform.localScale *= maintainMinScaleFactor;
			}
		}
	}

	public void Reset() {
		transform.localScale = originalScale;
	}

	void AdjustPlanetPositions() {
		float earthAndRocketOffsets = earth.bounds.extents.y + rocket.bounds.size.y;
		foreach (var planet in planets) {
			Vector3 newPos = planet.transform.position;  // planet.transform.y could be replaced by TargetPlanet.heights, except that newPos is already in a manipulated space due to its parent
			float offset = planet.bounds.extents.y + earthAndRocketOffsets;
			if (planet.transform.localPosition.y < 0) {
				offset = -offset;
			}
			newPos.y += offset;
			planet.transform.position = newPos;
		}
	}
}
