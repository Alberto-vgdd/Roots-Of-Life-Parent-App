using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideImageBehaviour : MonoBehaviour {
	public Scrollbar scrollbar;
	public Image image;
	public Text text;
	private bool loopup = false;
	private float alpha = 0.9f;
	private float fadeSpeed = 0.005f;

	// Update is called once per frame
	void Update () {
		if (loopup) {
			alpha += fadeSpeed;
			if (alpha >= 0.8)
				loopup = false;
		} else {
			alpha -= fadeSpeed;
			if (alpha <= 0.1)
				loopup = true;
		}
		float v = (float) (scrollbar.value - 0.7) * 5;
		if (v < 0)
			v = 0;
		if (v > 1)
			v = 1;
		alpha = alpha * v;
		image.CrossFadeAlpha (alpha, 0.0f, true);
		text.CrossFadeAlpha (alpha, 0.0f, true);
	}
}
