using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideImageBehaviour : MonoBehaviour {
	public SlideScreen slideScreen;
	public Image image;
	public Text text;
	private bool loopup = false;
	private float alpha = 0.9f;
	private float fadeSpeed = 0.005f;

	private float range = 0.0f;
	private float fadeTime = 1.5f;
	private float timer = 0.0f;

	// Update is called once per frame
	void Update () {
		float s = slideScreen.scroll * 2;
		float b = Mathf.Lerp (1.0f, 0.0f, s);
		float c = Mathf.Lerp (0.5f, 0.0f, s);
		float a = Mathf.Lerp (b, c, range);
		image.CrossFadeAlpha (a, 0.0f, true);
		text.CrossFadeAlpha (a, 0.0f, true);

		if (timer >= fadeTime) {
			timer = 0;
			loopup = !loopup;
		} else {
			timer += Time.deltaTime;
			range = loopup ? timer / 1.5f : 1 - (timer / 1.5f);
		}
	}

	public void calc() {
		float a = 1 - (slideScreen.scroll * 2);
		image.CrossFadeAlpha (a, 0.0f, true);
		text.CrossFadeAlpha (a, 0.0f, true);
	}
}
