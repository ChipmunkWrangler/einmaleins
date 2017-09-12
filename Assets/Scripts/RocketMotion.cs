using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMotion : MonoBehaviour {
	[SerializeField] FlashThrust thrust = null;
	[SerializeField] float minY;
	[SerializeField] float maxY;
	[SerializeField] float baseY; // position when speed is zero
	[SerializeField] float maxSpeed; // movement speed of the rocket object, not thrust speed

	float thrustCap; // we can be moving faster than this, but at this speed, we attain maxY and never exceed it.

	void Update () {
		if (thrustCap == 0) {
			thrustCap = thrust.accelerationOnCorrect;
		}
		Vector3 pos = gameObject.transform.position;
		float effectiveSpeed = Mathf.Clamp (thrust.speed, -thrustCap, thrustCap);
		float yRange = (effectiveSpeed > 0) ? (maxY - baseY) : (baseY - minY);
		pos.y = Mathf.MoveTowards (pos.y, baseY + yRange * effectiveSpeed / thrustCap, maxSpeed * Time.deltaTime);
		gameObject.transform.position = pos;
	}
}

