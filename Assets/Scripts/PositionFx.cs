using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFx : MonoBehaviour {
    [SerializeField] Transform referencePoint = null;

	void Start () {
        Debug.Log("PositionFx");
		StartCoroutine (InitFxPos ());
	}

	IEnumerator InitFxPos() {
		yield return new WaitForSeconds(0.5F); // Grid or Clock buttons need to be established first HACK
		Vector3 newPos = Camera.main.ScreenToWorldPoint (referencePoint.position);
		newPos.z = gameObject.transform.position.z;
		gameObject.transform.position = newPos;
	}
}
