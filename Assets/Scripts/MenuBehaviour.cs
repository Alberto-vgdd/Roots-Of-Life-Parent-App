using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBehaviour : ScrollRectEx {
    public GameObject menuBar;
    public Screen activeScreen;
    public float scrollSpeed = 4f;
    private bool dragging = false;
	private bool moving = false;
	private int swipeCooldown = 0;
	private float dX;
	private float dY;

	void Start() {
		setActiveScreen (1);
		dX = GetComponent<RectTransform> ().anchoredPosition.x;
		dY = GetComponent<RectTransform> ().anchoredPosition.y;
	}

	float defaultPosition(Screen screen) {
		if (screen == Screen.Story)
			return 0.0f;
		if (screen == Screen.Statistics)
			return 0.5f;
		if (screen == Screen.Objectives)
			return 1.0f;
		return -1f;
	}

	int nextScreen(Screen screen) {
		if (screen == Screen.Story)
			return 1;
		if (screen == Screen.Statistics)
			return 2;
		if (screen == Screen.Objectives)
			return 2;
		return -1;
	}

	int prevScreen(Screen screen) {
		if (screen == Screen.Story)
			return 0;
		if (screen == Screen.Statistics)
			return 0;
		if (screen == Screen.Objectives)
			return 1;
		return -1;
	}

    public void swipeRight()
    {
        if (swipeCooldown > 0)
            return;
        setActiveScreen(prevScreen(activeScreen));
    }

    public void swipeLeft()
    {
        if (swipeCooldown > 0)
            return;
        setActiveScreen(nextScreen(activeScreen));
    }

    void Update()
	{
		if (swipeCooldown > 0)
			swipeCooldown--;
		float v = horizontalScrollbar.value;

		if (v < 0.25)
			setActiveMenubutton (0);
		else if (v > 0.25 && v < 0.75)
			setActiveMenubutton (1);
		else if (v > 0.75)
			setActiveMenubutton (2);

		if (dragging)
			return;

		if (v < 0.25 && activeScreen != Screen.Story && !moving) {
			setActiveScreen (0);
		} else if (v > 0.25 && v < 0.75 && activeScreen != Screen.Statistics && !moving) {
			setActiveScreen (1);
		} else if (v > 0.75 && activeScreen != Screen.Objectives && !moving) {
			setActiveScreen (2);
		}

		if (v != defaultPosition (activeScreen))
			moving = true;

		if (moving)
			animate ();
	}

    private void animate()
    {
        float v = horizontalScrollbar.value;

        float d = defaultPosition(activeScreen);
        if (v > d)
        {
            float next = v - (Time.deltaTime * scrollSpeed);
            if (next < d)
                v = d;
            else v = next;
        }
        else if (v < d)
        {
            float next = v + (Time.deltaTime * scrollSpeed);
            if (next > d)
                v = d;
            else
                v = next;
        }
        if (v == d)
            moving = false;
        horizontalScrollbar.value = v;
    }

	public void setActiveScreen(int s) {
	    moving = true;
		if (s == 0) 
			activeScreen = Screen.Story;
		else if (s == 1) 
			activeScreen = Screen.Statistics;
		else if (s == 2) 
			activeScreen = Screen.Objectives;
		setActiveMenubutton (s);
        swipeCooldown = 30;
	}

	public void setActiveMenubutton(int s) {
		for (int i = 0; i < 3; i++)
			menuBar.transform.GetChild (i).gameObject.GetComponent<TabBehaviour> ().unselect ();

		menuBar.transform.GetChild (s).gameObject.GetComponent<TabBehaviour> ().select ();
	}

	public void startDrag() {
		dragging = true;
	}

	public void endDrag() {
		dragging = false;
	}

	public enum Screen {
		Story,
		Statistics,
		Objectives
	}
}
