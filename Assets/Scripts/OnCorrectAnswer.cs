using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface OnCorrectAnswer  {
	void OnCorrectAnswer(Question question, bool isNewlyMastered);
}
