using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMotionSolarSystem : MonoBehaviour {
	[SerializeField] FlashThrust thrust = null;
	[SerializeField] Params paramObj = null;
	[SerializeField] Transform rocketItself = null;
	[SerializeField] float orbitScaleFactor = 0.1f;
	float minY;

	void Start() {
		minY = gameObject.transform.position.y;
	}

	void Update () {
		if (thrust.orbitingPlanet == null) {
			Ascend ();
		} else {
			Orbit ();
		}
	}

	void Ascend() {
		Vector3 pos = gameObject.transform.position;
		pos.y = minY + thrust.height * paramObj.heightScale * gameObject.transform.parent.localScale.y;
		gameObject.transform.position = pos;
	}

	void Orbit ()
	{
		Vector3 zeroPos = new Vector3 ();
		zeroPos.y = rocketItself.transform.localPosition.y;
		rocketItself.transform.localPosition = zeroPos;
		Vector3 pos = thrust.orbitingPlanet.transform.position;
		float radius = thrust.orbitingPlanet.bounds.extents.x * orbitScaleFactor;
		float fraction = thrust.orbitalDistance / thrust.planetCircumferance;
		float radians = 2.0f * Mathf.PI * fraction;
		pos.x += radius * Mathf.Cos (radians);
		pos.y += radius * Mathf.Sin (radians);
		Debug.Log (thrust.orbitalDistance + " fraction = " + fraction + " radians = " + radians + " cos = " + Mathf.Cos (radians) + " pos = " + pos);
		gameObject.transform.position = pos;
		rocketItself.transform.localRotation = Quaternion.Euler(0, -radians * 180.0f / Mathf.PI, 0);

	}
}
