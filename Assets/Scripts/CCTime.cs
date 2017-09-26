using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTime {
	static System.DateTime? now;

	static public System.DateTime Now() {
		return (now.HasValue) ? now.Value : System.DateTime.UtcNow;
	}

	static public void SetNow(System.DateTime newNow) {
		now = newNow;
	}
}
