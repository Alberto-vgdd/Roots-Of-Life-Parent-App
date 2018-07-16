using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerinfoBehaviour : MonoBehaviour {
	public LockscreenBehaviour lockscreen;
	public GameObject lastSeen;
	
	// Update is called once per frame
	void Update () {
		calc ();
	}

	public void calc() {
		RectTransform rT = GetComponent<RectTransform> ();
		rT.anchorMax = new Vector2 (0.5f, Mathf.Lerp (0.4f, 1f, lockscreen.scroll));
		rT.anchorMin = new Vector2 (0.5f, Mathf.Lerp (0.4f, 1f, lockscreen.scroll));
		rT.anchoredPosition = new Vector2 (0, Mathf.Lerp(0, -100, lockscreen.scroll));
		foreach (Text t in lastSeen.GetComponentsInChildren<Text>())
			t.CrossFadeAlpha (1 - (lockscreen.scroll * 2), 0.0f, false);
	}
}
