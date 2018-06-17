using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerinfoBehaviour : MonoBehaviour {
	public Scrollbar scrollbar;
    public ProfileSelector selector;
    public Text lastLogin;
	private float defaulty;

	void Start() 
	{
		defaulty = gameObject.GetComponent<RectTransform> ().anchoredPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void move() 
	{
		Vector2 position = gameObject.GetComponent<RectTransform> ().anchoredPosition;
		position.y = defaulty + -820 * (scrollbar.value * -1 + 1);
		gameObject.GetComponent<RectTransform> ().anchoredPosition = position;

        lastLogin.CrossFadeAlpha(-1 + (scrollbar.value * 2), 0.0f, false);
        lastLogin.transform.GetChild(0).GetComponent<Text>().CrossFadeAlpha(-1 + (scrollbar.value * 2), 0.0f, false);
    }
}
