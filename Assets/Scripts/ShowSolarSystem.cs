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
	Vector3 originalScale;
	Transform particleSystemTransform;

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

		float viewportTop = Camera.main.WorldToViewportPoint(rocket.bounds.max).y + verticalPadding;
		if (viewportTop > 1.0f) {
			transform.localScale /= viewportTop;
			particleSystemTransform.localScale /= viewportTop;
		}
	}

	public void Reset() {
		transform.localScale = originalScale;
	}

	void AdjustPlanetPositions() {
		float earthAndRocketOffsets = earth.bounds.extents.y + rocket.bounds.size.y;
		foreach (var planet in planets) {
			Vector3 newPos = planet.transform.position;  // planet.transform.y could be replaced by FlashThrust.maxAttainableHeights
			float offset = planet.bounds.extents.y + earthAndRocketOffsets;
			if (planet.transform.localPosition.y < 0) {
				offset = -offset;
			}
			newPos.y += offset;
			planet.transform.position = newPos;
		}
	}
}
