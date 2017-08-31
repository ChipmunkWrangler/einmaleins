using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneDisplay : TextDisplay, OnQuestionChanged {

	public void OnQuestionChanged(Question question) {
		SetText( (question == null) ? "Fertig!" : "" );
	}
}
