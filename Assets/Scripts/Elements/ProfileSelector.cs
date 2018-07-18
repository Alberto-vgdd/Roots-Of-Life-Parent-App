using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ProfileSelector : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject profiles;
	public GameObject profileTemplate;
    public GameObject addButton;

    public bool isDragging;
    public bool update;
    private float x = (540 - 320);
    private float scroll = 0.0f;
    private List<Image> images;

    void Start()
    {
        images = new List<Image>();
    }

    private void Update()
    {
        RectTransform rt = profiles.GetComponent<RectTransform>();
        if (update)
        {
            int newselected = AppData.selected + (int)Mathf.Round(scroll);
            ProfileManager.setProfile(newselected);

            // scroll = difference between x value and current position of scroll to make smooth transition
            scroll = (x - rt.anchoredPosition.x) / 700;

            update = false;
        }

        if (!isDragging)
        {
            float speed = Time.deltaTime * 5;
            if (scroll > 0)
                if ((scroll - speed) < 0)
                    scroll = 0;
                else
                    scroll -= speed;
            else if (scroll < 0)
                if ((scroll + speed) > 0)
                    scroll = 0;
                else
                    scroll += speed;
        }

        rt.anchoredPosition = new Vector2(x + (scroll * -700), 0);
    }

    public void displayUsers()
    {
        foreach (Image i in images)
            Destroy(i.gameObject);
        images = new List<Image>();

        RectTransform rt = profiles.GetComponent<RectTransform>();

        // Make sure there is at least one user to display
        if (AppData.profiles.Count < 1)
            return;

         // Change size of profiles object to accomodate all profile images
        rt.sizeDelta = new Vector2(700f * (1 + AppData.profiles.Count), 640);

        // Place each image
        for (int i = 0; i < AppData.profiles.Count; i++)
        {
            Image image = Instantiate(profileTemplate).GetComponent<Image>();
            image.transform.SetParent(profiles.transform, false);
            image.rectTransform.anchorMin = new Vector2(0f, 0.5f);
            image.rectTransform.anchorMax = new Vector2(0f, 0.5f);
            image.rectTransform.anchoredPosition = new Vector2(320f + (i * 700f), 0);
            image.name = AppData.profiles[i].profilename;
            StartCoroutine(AppData.profiles[i].getImage(image));
            images.Add(image);
        }

        // Place add button at the end
        addButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(AppData.profiles.Count * 700f + 320f, 0);

        // Place on selected profile
    }

    public void selectNext()
    {
        RectTransform rt = profiles.GetComponent<RectTransform>();
        ProfileManager.setProfile(AppData.selected + 1);
        Debug.Log(AppData.selected);
        scroll = (x - rt.anchoredPosition.x) / 700;
    }

    public void selectPrevious()
    {
        RectTransform rt = profiles.GetComponent<RectTransform>();
        ProfileManager.setProfile(AppData.selected - 1);
        Debug.Log(AppData.selected);
        scroll = (x - rt.anchoredPosition.x) / 700;
    }

    public void passDragg(float deltaX)
    {
        scroll += (deltaX * 4) / -700f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        scroll += (eventData.delta.x * 4) / -700f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        update = true;
    }

    public void updateX()
    {
        x = (540 - 320) - (700 * AppData.selected);
    }
}
