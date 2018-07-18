using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddButton : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject AddUserScreen;
    public ProfileSelector profileSelector;
    private bool drag = false;

    public void OnDrag(PointerEventData eventData)
    {
        drag = true;
        profileSelector.passDragg(eventData.delta.x);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        profileSelector.isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!drag)
            ScreenManager.showScreen("addUser");
        else
        {
            drag = false;
            profileSelector.isDragging = false;
            profileSelector.update = true;
        }
    }
}
