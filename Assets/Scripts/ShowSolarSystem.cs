using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class ForegroundDisplaySettings {
	public float scale = 1.0f;
	public float yPos = 0;
}

public class ShowSolarSystem : MonoBehaviour {
	[SerializeField] ForegroundDisplaySettings play = null;
	[SerializeField] ForegroundDisplaySettings[] solarSystemByUpgradeLevel = null;
	[SerializeField] float transitionTime = 2.0f;

	public void ShowResults() {
		MoveTo(solarSystemByUpgradeLevel[RocketParts.GetUpgradeLevel()], transitionTime);
	}

	void Start () {
		MoveTo (play, 0);
	}
	
	void MoveTo (ForegroundDisplaySettings settings, float transitionTime) {
		Transform transform = gameObject.transform;
		transform.localScale = new Vector3 (settings.scale, settings.scale, 1.0f);
		Vector3 oldPos = transform.position;
		transform.position = new Vector3 (oldPos.x, settings.yPos, oldPos.z);
	}

}
