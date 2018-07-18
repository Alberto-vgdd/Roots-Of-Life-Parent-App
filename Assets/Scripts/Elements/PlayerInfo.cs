using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {
	public SlideScreen slideScreen;
	public GameObject lastSeen;
	
	// Update is called once per frame
	void Update () {
		calc ();
	}

	public void calc() {
		RectTransform rT = GetComponent<RectTransform> ();
		rT.anchorMax = new Vector2 (0.5f, Mathf.Lerp (0.45f, 1f, slideScreen.scroll));
		rT.anchorMin = new Vector2 (0.5f, Mathf.Lerp (0.45f, 1f, slideScreen.scroll));
		rT.anchoredPosition = new Vector2 (0, Mathf.Lerp(0, -100, slideScreen.scroll));
		foreach (Text t in lastSeen.GetComponentsInChildren<Text>())
			t.CrossFadeAlpha (1 - (slideScreen.scroll * 2), 0.0f, false);
	}
}
