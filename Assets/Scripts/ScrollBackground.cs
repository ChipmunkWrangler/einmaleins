using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour {
	[SerializeField] float[] scrollSpeedMultiplier = null;
	[SerializeField] float maxScrollSpeed = 0.0001F;
	[SerializeField] float tileSizeY = 80.0F;

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

	public void SetRocketSpeed(float speed, float maxSpeed ) {
		// This is assumed to be called every frame, so you should probably pull instead 
		baseOffset = transform.position.y - startPosition.y;
		float speedFraction = speed / maxSpeed;
		scrollSpeed = speedFraction * maxScrollSpeed * scrollSpeedMultiplier[RocketParts.instance.upgradeLevel];
	}
}
