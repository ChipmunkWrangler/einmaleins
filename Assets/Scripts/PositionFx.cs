using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFx : MonoBehaviour {
	[SerializeField] Transform referencePoint;

	void Start () {
		Vector3 newPos = Camera.main.ScreenToWorldPoint(referencePoint.position);
		newPos.z = gameObject.transform.position.z;
		gameObject.transform.position = newPos;
	}
}
