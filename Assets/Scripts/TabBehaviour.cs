using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabBehaviour : MonoBehaviour {

	public void unselect() {
		GetComponent<Image> ().CrossFadeAlpha (0f, 0.0f, false);
	}

	public void select() {
		GetComponent<Image> ().CrossFadeAlpha (1f, 0.0f, false);
	}
}
