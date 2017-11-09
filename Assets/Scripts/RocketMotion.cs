using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMotion : MonoBehaviour {
	[SerializeField] FlashThrust thrust = null;
	[SerializeField] float minY;
	[SerializeField] float maxY;
	[SerializeField] float baseY; // position when speed is zero
	[SerializeField] float maxSpeed; // movement speed of the rocket object, not thrust speed
	[SerializeField] GameObject earth;
	[SerializeField] float earthSpeed;

	float speedCap; // we can be moving faster than this, but at this speed, we attain maxY and never exceed it.
	float initialEarthY;
	float lowestRocketYRelativeToEarth;

	void Start() {
		initialEarthY = earth.transform.position.y;
		lowestRocketYRelativeToEarth = baseY - initialEarthY;
	}

	void Update () {
		UpdateEarthPos ();
		UpdateRocketPos ();
	}

	void UpdateRocketPos () {
		if (speedCap == 0) {
			speedCap = thrust.GetMaxSingleQuestionSpeed();
		}
		Vector3 pos = gameObject.transform.position;
		float effectiveSpeed = Mathf.Clamp (thrust.speed, -speedCap, speedCap);
		float yRange = (effectiveSpeed > 0) ? (maxY - baseY) : (baseY - minY);
		pos.y = Mathf.MoveTowards (pos.y, baseY + yRange * effectiveSpeed / speedCap, maxSpeed * Time.deltaTime);
		if (pos.y - earth.transform.position.y < lowestRocketYRelativeToEarth) {
			pos.y = earth.transform.position.y + lowestRocketYRelativeToEarth;
		}
		gameObject.transform.position = pos;
	}

	void UpdateEarthPos() {
		Vector3 pos = earth.transform.position;
		pos.y = initialEarthY - earthSpeed * thrust.height;
		earth.transform.position = pos;
	}
}

