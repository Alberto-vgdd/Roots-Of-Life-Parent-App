using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SnapscrollBehaviour : Scrollbar {
	private bool dragging = false;
	private float scrollSpeed = 1.65f;
	public bool state = true; // true = up, false = down

	// Update is called once per frame
	void Update () {
		if (dragging)
        { if (state && value < 0.6)
                state = false;
            else if (!state && value > 0.6)
                state = true;
            return;
        }
		
		if (state)
        {
            value += Time.deltaTime * scrollSpeed;
		} else
        {
            value -= Time.deltaTime * scrollSpeed;
        }
	}

    public void up()
    {
        Debug.Log("go up");
        state = true;
    }

    public void down()
    {
        Debug.Log("go down");
        state = false;
    }

    public void startDrag() {
		dragging = true;
	}

	public void endDrag() {
		dragging = false;
	}
}
