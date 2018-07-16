using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ProfileSelector : MonoBehaviour
{
    public GameObject profiles;
	//public GameObject content;
	public GameObject profileTemplate;
    public GameObject addButton;

	//public UnityEvent onSelectNewProfile;
    
    //public float profileWidth;
    //private bool dragging;
    //private bool moving;
    //private bool update;

    public void displayUsers()
    {
        RectTransform rt = profiles.GetComponent<RectTransform>();

        // Make sure there is at least one user to display
        if (AppData.profiles.Count >= 1)
        {
            // Change size of profiles object to accomodate all profile images
            rt.sizeDelta = new Vector2(700f * (1 + AppData.profiles.Count), 640);
            rt.anchoredPosition = new Vector2(1080, 0);

            // Place each image
            for (int i = 0; i < AppData.profiles.Count; i++)
            {
                Image image = Instantiate(profileTemplate).GetComponent<Image>();
                image.transform.SetParent(profiles.transform, false);
                image.rectTransform.anchorMin = new Vector2(0f, 0.5f);
                image.rectTransform.anchorMax = new Vector2(0f, 0.5f);
                image.rectTransform.anchoredPosition = new Vector2(320f + (i * 700f), 0);
                StartCoroutine(AppData.profiles[i].getImage(image));
            }

            // Place add button at the end
            addButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(AppData.profiles.Count * 700f + 320f, 0);
        }
    }


    /*void displayUsers()
    {
        float contentwidth = 1080 + (660 * (AppData.profiles.Count - 1));
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentwidth, 660);
        for (int i = 0; i < AppData.profiles.Count; i++)
        {
            Profile p = AppData.profiles[i];
            p.image.rectTransform.anchoredPosition = new Vector2(660 * i - ((contentwidth - 1080) * 0.5f), 0);
        }
    }

    // Update is called once per frame
    void Update() {
        if (dragging)
            return;

        // Check if adjusting of view is necessary and animate the proper action
        if (moving)
        {
            animate();
            return;
        }

        if (update)
            updateAfterDrag();
        update = false;
    }

	// Move the scrollbar
    private void animate()
    {
        float v = GetComponent<ScrollRectEx>().horizontalScrollbar.value;
        float t = getTarget(AppData.selected);
        if (v > t)
        {
            float next = v - (Time.deltaTime * 5);
            if (next < t)
                v = t;
            else
                v = next;
        }
        else if (v < t)
        {
            float next = v + (Time.deltaTime * 5);
            if (next > t)
                v = t;
            else
                v = next;
        }
        if (v == t)
            moving = false;
        GetComponent<ScrollRectEx>().horizontalScrollbar.value = v;
    }

    void updateAfterDrag()
    {
        float v = GetComponent<ScrollRectEx>().horizontalScrollbar.value;
        float w = 0;
        int selection = 0;

        // find what profile the scrollbar value has in view
        while (!(v >= w && v <= (w + profileWidth)))
        {
            w += profileWidth;
            selection++;
        }

        // update selected profile if view was open on a different profile than the currently selected profile
        if (selection != AppData.selected)
            setProfile(selection);

        // move view to make sure current profile is shown in the exact center
        if (v != getTarget(AppData.selected))
            moving = true;
    }

	// Changes the value of selected to the index of the new selected user
	// gets called every time the slider is changed, only call it when the user is actually updated
    public void setProfile(int profile)
    {
        AppData.selected = profile;
        playerName.text = AppData.profiles[AppData.selected].name;
        if (AppData.profiles[AppData.selected].active)
        {
            playerStatus.text = "Status: Playing";
            lastLogin.text = "Logged in:";
        }
        else
        {
            playerStatus.text = "Status: Offline";
            lastLogin.text = "Last seen:";
        }
        lastLogin.transform.GetChild(0).GetComponent<Text>().text = getTimeString(AppData.profiles[AppData.selected].lastlogin);
        onSelectNewProfile.Invoke ();
    }

	// Find the location on the scrollbar to display the given user
    private float getTarget(int account)
    {
		if (account > AppData.profiles.Count)
            return -1;
		return account * (1f / (AppData.profiles.Count - 1));
    }

    public void startDrag()
    {
        dragging = true;
    }

    public void endDrag()
    {
        dragging = false;
        update = true;
    }*/
}
