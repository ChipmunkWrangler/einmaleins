using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour {
	[SerializeField] float scrollSpeedFactor;
	[SerializeField] float tileSizeY;

	Vector3 startPosition;
	float baseOffset;
	float scrollSpeed;
	float speedChangeTime;

	void Start() { 
		startPosition = transform.position;
	}

	void Update ()
	{
		float newOffset = Mathf.Repeat((Time.time - speedChangeTime) * scrollSpeed + baseOffset, tileSizeY);
		transform.position = startPosition + Vector3.up * newOffset;
	}

	public void SetRocketSpeed(float speed ) {
		speedChangeTime = Time.time;
		baseOffset = transform.position.y - startPosition.y;
		scrollSpeed = speed * scrollSpeedFactor;
	}
}
