using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class TouchInput : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {

	public UnityEvent up;
	public UnityEvent down;
	public UnityEvent left;
	public UnityEvent right;
	public UnityEvent press;

    public enum Touch {
        none,
        press,
        up,
        down,
        left,
        right,
    }
    
    private Touch touch;
	private float touchTime;

    void Update() {
        if (touch == Touch.none)
            return;
        touchTime += Time.deltaTime;
    }

    public void OnDrag(PointerEventData eventData) {
        float x = eventData.delta.x;
        float y = eventData.delta.y;
        if (x < 0) {
            if (y < 0) {
                if (x < y) { 
                    touch = Touch.left;
                    return;
                }
                else {
                    touch = Touch.down;
                    return;
                }
            } else {
                x = x * -1;
                if (x < y) {
                    touch = Touch.up;
                    return;
                }
                else { 
                    touch = Touch.left;
                    return;
                }
            }
        } else {
            if (y > 0) {
                if (x > y) { 
                    touch = Touch.right;
                    return;
                }
                else {
                    touch = Touch.up;
                    return;
                }
            } else {
                y = y * -1;
                if (x > y) {
                    touch = Touch.right;
                    return;
                }
                else {
                    touch = Touch.down;
                    return;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        touch = Touch.press;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (touchTime < 0.25) {
            switch (touch) {
			case Touch.up:
				up.Invoke ();
                break;
			case Touch.down:
				down.Invoke ();
                break;
			case Touch.left:
				left.Invoke ();
                break;
			case Touch.right:
				right.Invoke ();
                break;
            default:
                break;
            }
        }
		if (touch == Touch.press)
			press.Invoke ();
        touch = Touch.none;
        touchTime = 0;
    }
}
