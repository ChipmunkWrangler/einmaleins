using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBuilder : MonoBehaviour {
	[SerializeField] float builtY = 0;
	[SerializeField] float hiddenY = -2.0f;
	[SerializeField] float buildingTime = 5.0f;
	[SerializeField] float buildingDelay = 1.0f;
	[SerializeField] RocketPartCounter counter = null;
	[SerializeField] ParticleSystem buildParticles;

	void Start () {
		Vector3 pos = gameObject.transform.position;
		pos.y = (RocketParts.IsRocketBuilt()) ? builtY : hiddenY;
		gameObject.transform.position = pos;
		if (!RocketParts.IsRocketBuilt () && RocketParts.CanBuild ()) {
			Build ();
		}
	}

	void Build ()
	{
		iTween.MoveTo (gameObject, iTween.Hash ("y", builtY, "time", buildingTime, "delay", buildingDelay, "easetype", iTween.EaseType.easeOutQuad, "oncomplete", "OnBuilt"));
		buildParticles.Play ();
		counter.OnSpend (RocketParts.GetNumParts (), RocketParts.GetNumParts () - RocketParts.GetNumPartsRequired());
	}

	void OnBuilt() {
		RocketParts.Build ();
		buildParticles.Stop ();
	}

}
