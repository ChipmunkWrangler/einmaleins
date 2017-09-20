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

	void Update() {
		float viewportTop = Camera.main.WorldToViewportPoint(rocket.bounds.max).y + verticalPadding;
		if (viewportTop > 1.0f) {
			transform.localScale /= viewportTop;
		}
	}

	void MoveTo (ForegroundDisplaySettings settings, float transitionTime) {
		Transform transform = gameObject.transform;
		transform.localScale = new Vector3 (settings.scale, settings.scale, 1.0f);
		Vector3 oldPos = transform.position;
		transform.position = new Vector3 (oldPos.x, settings.yPos, oldPos.z);
	}

}
