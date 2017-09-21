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
	[SerializeField] float[] planetScale = null;
	Vector3 originalScale;
	Transform particleSystemTransform;

	void Start() {
		originalScale = transform.localScale;
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
}
