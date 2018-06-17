using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchInput : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {

    public MenuBehaviour menuBehaviour;
    public SnapscrollBehaviour snapscrollBehaviour;

    public enum Touch {
        none,
        press,
        up,
        down,
        left,
        right,
    }
    
    public Touch touch;
    public float touchTime;

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
                    swipeUp();
                    break;
                case Touch.down:
                    swipeDown();
                    break;
                case Touch.left:
                    swipeLeft();
                    break;
                case Touch.right:
                    swipeRight();
                    break;
                default:
                    break;
            }
        }
        if (touch == Touch.press)
            press();
        touch = Touch.none;
        touchTime = 0;
    }

    public void swipeUp()
    {
        if (snapscrollBehaviour != null)
            snapscrollBehaviour.down();
        Debug.Log("Swipe Up");
    }

    public void swipeDown()
    {
        if (snapscrollBehaviour != null)
            snapscrollBehaviour.up();
        Debug.Log("Swipe Down");
    }

    public void swipeLeft()
    {
        if (menuBehaviour != null)
            menuBehaviour.swipeLeft();
        Debug.Log("Swipe Left");
    }

    public void swipeRight()
    {
        if (menuBehaviour != null)
            menuBehaviour.swipeRight();
        Debug.Log("Swipe Right");
    }

    public void press()
    {
        Debug.Log("Press");
    }
}
