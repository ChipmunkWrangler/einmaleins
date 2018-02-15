using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPositionAndSize : MonoBehaviour {
    [SerializeField] RectTransform target = null;
    [SerializeField] RectTransform source = null;

	void Update () {
		source.position = target.position;
		source.sizeDelta = target.sizeDelta;
	}
}
