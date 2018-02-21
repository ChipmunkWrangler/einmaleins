using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMotionSolarSystem : MonoBehaviour {
    [SerializeField] FlashThrust Thrust = null;
    [SerializeField] Params ParamObj = null;
    float MinY;

	void Start() {
		MinY = gameObject.transform.position.y;
	}

	void Update () {
		Ascend ();
	}

	void Ascend() {
		Vector3 pos = gameObject.transform.position;
		pos.y = MinY + Thrust.Height * ParamObj.HeightScale * gameObject.transform.parent.localScale.y;
		gameObject.transform.position = pos;
	}
}
