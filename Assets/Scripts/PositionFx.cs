using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFx : MonoBehaviour {
	[SerializeField] Transform referencePoint;

	bool isSet;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!isSet) { 
			isSet = true;
//			print (transform.position);
//			gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position);
		}
	}
}
