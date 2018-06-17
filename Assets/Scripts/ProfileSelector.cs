using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProfileSelector : MonoBehaviour {
    public Text playerName;
    public Text playerStatus;
    public Text lastLogin;
	public GameObject content;
	public GameObject profileTemplate;
    public GameObject addButton;

	public UnityEvent onSelectNewProfile;

    public int parentID;
	public List<Profile> profiles;
    public float profileWidth;
    public int selected;
    private bool dragging;
    private bool moving;
    private bool update;

	string URL = "http://62.131.170.46/roots-of-life/userSelect.php";
    public string[] usersData;

    // Use this for initialization
    void Start()
    {

    }

    public void unloadUsers()
    {
        foreach (Profile p in profiles)
        {
            Destroy(p.image);
        }
        playerName.text = "No users found";
        playerStatus.text = "Status:";
    }

    public void loadUsers()
    {
        StartCoroutine(userForm());
    }

    public IEnumerator userForm()
    {
        addButton.SetActive(false);

        WWWForm form = new WWWForm();
        form.AddField("setParentID", parentID);

        WWW www = new WWW(URL, form);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("error: " + www.error);
        }
        else
        {
            Debug.Log("result: " + www.text);
        }
        
        profiles = new List<Profile>();

        if (www.text != "")
        {
            string usersDataString = www.text.TrimEnd(';');
            usersData = usersDataString.Split(';');
            foreach (string data in usersData)
            {
                string name = data.Split(',')[0];
                int lastlogin = int.Parse(data.Split(',')[2]);
                Image image = Instantiate(profileTemplate).GetComponent<Image>();
                image.transform.SetParent(content.transform, false);
                Profile p = new Profile(name, image, lastlogin);
                if (data.Split(',')[1] == "1")
                    p.active = true;
                profiles.Add(p);
            }
            profileWidth = 1f / profiles.Count;
            selected = 0;
            displayUsers();
        }
        else
        {
            addButton.SetActive(true);
        }
    }

    void displayUsers()
    {
        float contentwidth = 1080 + (660 * (profiles.Count - 1));
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(contentwidth, 660);
        for (int i = 0; i < profiles.Count; i++)
        {
            Profile p = profiles[i];
            p.image.rectTransform.anchoredPosition = new Vector2(660 * i - ((contentwidth - 1080) * 0.5f), 0);
        }

        setProfile(profiles.Count / 2);
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
        float t = getTarget(selected);
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
        if (selection != selected)
            setProfile(selection);

        // move view to make sure current profile is shown in the exact center
        if (v != getTarget(selected))
            moving = true;
    }

	// Changes the value of selected to the index of the new selected user
	// gets called every time the slider is changed, only call it when the user is actually updated
    public void setProfile(int profile)
    {
        selected = profile;
        playerName.text = profiles[selected].name;
        if (profiles[selected].active)
        {
            playerStatus.text = "Status: Playing";
            lastLogin.text = "Logged in:";
        }
        else
        {
            playerStatus.text = "Status: Offline";
            lastLogin.text = "Last seen:";
        }
        lastLogin.transform.GetChild(0).GetComponent<Text>().text = getTimeString(profiles[selected].lastlogin);
        onSelectNewProfile.Invoke ();
    }

    private string getTimeString(int unixTime)
    {
        DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        time = time.AddSeconds(unixTime).ToLocalTime();
        Debug.Log(time.ToString());
        return time.ToString().Split(' ')[1] + "\n" + time.ToString().Split(' ')[0];
    }

	// Return the profile instance of the selected user
	public Profile getSelected() 
	{
		return profiles [selected];
	}

	// Find the location on the scrollbar to display the given user
    private float getTarget(int account)
    {
		if (account > profiles.Count)
            return -1;
		return account * (1f / (profiles.Count - 1));
    }

    public void startDrag()
    {
        dragging = true;
    }

    public void endDrag()
    {
        dragging = false;
        update = true;
    }
}
