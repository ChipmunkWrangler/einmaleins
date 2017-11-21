using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPositionAndSize : MonoBehaviour {
	[SerializeField] RectTransform target;
	[SerializeField] RectTransform source;

	void Update () {
		source.position = target.position;
		source.sizeDelta = target.sizeDelta;
	}
}
