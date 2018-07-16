using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour {

    string URL = "http://62.131.170.46/roots-of-life/userSelect.php";
    public ProfileSelector profileSelector;
    public Text playerName;
    public Text playerStatus;
    public Text lastLogin;

    private float reloadTimer = 0.0f;
	
	// Update is called once per frame
	void Update () {
		if (AppData.loggedIn)
        {
            // Make sure to reload users every minute
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= 60.0f)
            {
                reloadTimer = 0;
                loadUsers();
            }
        }
	}

    public void loadUsers()
    {
        StartCoroutine(loadRequest());
    }

    public void unloadUsers()
    {
        AppData.profiles = new List<Profile>();
        playerName.text = "No users found";
        playerStatus.text = "Status:";
        lastLogin.text = "";
    }

    // Load all child users from parent account
    IEnumerator loadRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("setParentID", AppData.parentID);

        WWW www = new WWW(URL, form);
        yield return www;
        if (www.text == "")
            yield break;

        AppData.profiles = new List<Profile>();
        string[] usersData = www.text.TrimEnd(';').Split(';');

        // Extract users from data
        foreach (string data in usersData)
            storeUser(data);
        profileSelector.displayUsers();
        setProfile(0);
    }
    
    void storeUser(string data)
    {
        // Retrieve user info
        string name = data.Split(',')[0];
        int lastlogin = int.Parse(data.Split(',')[2]);
        string imageURL = data.Split(',')[4];

        // Create user profile
        Profile p = new Profile(name, lastlogin, imageURL);
        if (data.Split(',')[1] == "1")
            p.active = true;

        // Store user profile
        AppData.profiles.Add(p);
    }

    public void setProfile(int profN)
    {
        AppData.selected = profN;
        playerName.text = AppData.getChild().profileName;
        if (AppData.getChild().active)
        {
            playerStatus.text = "Status: Playing";
            lastLogin.text = "Logged in:";
        } else
        {
            playerStatus.text = "Status: Offline";
            lastLogin.text = "Last seen:";
        }
        lastLogin.transform.GetChild(0).GetComponent<Text>().text = getTimeString(AppData.getChild().lastlogin);
    }

    private string getTimeString(int unixTime)
    {
        DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        time = time.AddSeconds(unixTime).ToLocalTime();
        Debug.Log(time.ToString());
        return time.ToString().Split(' ')[1] + "\n" + time.ToString().Split(' ')[0];
    }
}
