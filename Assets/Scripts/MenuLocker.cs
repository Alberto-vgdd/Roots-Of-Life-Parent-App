using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLocker : MonoBehaviour {
	public Scrollbar scrollbar;
	public GameObject content;
	private float dX;
	private float dY;
	private float contentsize;

	// Use this for initialization
	void Start () 
	{
		dX = GetComponent<RectTransform> ().anchoredPosition.x;
		dY = GetComponent<RectTransform> ().anchoredPosition.y;
		contentsize = (content.GetComponent<RectTransform> ().sizeDelta.y) * -0.5f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void lockMenu()
	{
		float v = scrollbar.value;
		float y = contentsize * (v - 1);
		GetComponent<RectTransform> ().anchoredPosition = new Vector2 (dX, dY - y);
	}
}
