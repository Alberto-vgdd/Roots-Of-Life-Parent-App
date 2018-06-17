using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerinfoBehaviour : MonoBehaviour {
	public Scrollbar scrollbar;
	public ProfileSelector selector;
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
	}

    /*public void updateInfo()
	{
		Profile selected = selector.profiles [selector.selected];
		transform.GetChild (0).GetComponent<Text> ().text = selected.name;
		if (selected.active)
			transform.GetChild (1).GetComponent<Text> ().text = "Status: Playing";
		else
			transform.GetChild (1).GetComponent<Text> ().text = "Status: Offline";
	}*/
}
