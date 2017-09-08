using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour {
	[SerializeField] float scrollSpeedFactor;
	[SerializeField] float tileSizeY;

	Vector3 startPosition;
	float baseOffset;
	float scrollSpeed;

	void Start() { 
		startPosition = transform.position;
	}

	void Update ()
	{
		float newOffset = Mathf.Repeat((Time.deltaTime) * scrollSpeed + baseOffset, tileSizeY);
		transform.position = startPosition + Vector3.up * newOffset;
	}

	public void SetRocketSpeed(float speed ) {
		// This is assumed to be called every frame, so you should probably pull instead 
		baseOffset = transform.position.y - startPosition.y;
		scrollSpeed = speed * scrollSpeedFactor;
	}
}
