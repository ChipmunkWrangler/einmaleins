using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMotionSolarSystem : MonoBehaviour {
	[SerializeField] FlashThrust thrust = null;
	[SerializeField] float heightScale = 0.0001f;
	float minY;

	void Start() {
		minY = gameObject.transform.position.y;
	}

	void Update () {
		Vector3 pos = gameObject.transform.position;

		pos.y = minY + thrust.height * heightScale * gameObject.transform.parent.localScale.y;
		gameObject.transform.position = pos;
	}
}
