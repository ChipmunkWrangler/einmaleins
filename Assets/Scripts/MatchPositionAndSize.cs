using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPositionAndSize : MonoBehaviour {
    [SerializeField] RectTransform Target = null;
    [SerializeField] RectTransform Source = null;

	void Update () {
		Source.position = Target.position;
		Source.sizeDelta = Target.sizeDelta;
	}
}
