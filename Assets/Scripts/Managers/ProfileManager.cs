using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProfileManager : MonoBehaviour
{
    // Used to make static references to instance
    public static ProfileManager main;

    // Inspector input
    public ProfileSelector profileSelector;
    public Text playerName;
    public Text playerStatus;
    public Text lastLogin;

	public UnityEvent onSelect;

    private float reloadTimer = 0.0f;
    private static string loadURL = "http://62.131.170.46/roots-of-life/profileSelect.php";
    private static string deleteURL = "http://62.131.170.46/roots-of-life/profileDelete.php";
    private bool setup = true;

    void Start()
    {
        main = this;
    }

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

    public void iLoadUsers() { loadUsers(); }
    public void iUnloadUsers() { unloadUsers(); }
    public void iDelete() { deleteCurrent(); }

    public static void loadUsers()
    {
        main.StartCoroutine(loadRequest());
    }

    public static void unloadUsers()
    {
        AppData.profiles = new List<Profile>();
        main.playerName.text = "No users found";
        main.playerStatus.text = "Status:";
        main.lastLogin.text = "";
		main.setup = true;
    }

    public static void deleteCurrent()
    {
        main.StartCoroutine(delete());
    }

    // Load all child users from parent account
    static IEnumerator loadRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("setParentID", AppData.parentID);

        WWW www = new WWW(loadURL, form);
        yield return www;
        if (www.text == "")
            yield break;

        AppData.profiles = new List<Profile>();
        string[] usersData = www.text.TrimEnd(';').Split(';');

        // Extract users from data
        foreach (string data in usersData)
            storeUser(data);

        if (main.setup)
        {
            main.setup = false;
            setProfile(0);
        }

        main.profileSelector.displayUsers();
    }

    static IEnumerator delete()
    {
        WWWForm form = new WWWForm();
        form.AddField("setChildID", AppData.getChild().id);
        WWW www = new WWW(deleteURL, form);
        yield return www;

        setProfile(AppData.selected - 1);
        loadUsers();
    }
    
    static void storeUser(string data)
    {
        // Retrieve user info
        string name = data.Split(',')[0];
        int lastlogin = int.Parse(data.Split(',')[2]);
        string imageURL = data.Split(',')[4];
        int id = int.Parse(data.Split(',')[5]);

        // Create user profile
        Profile p = new Profile(id, name, lastlogin, imageURL);
        if (data.Split(',')[1] == "1")
            p.active = true;

        // Store user profile
        AppData.profiles.Add(p);
    }

    public static void setProfile(int profN)
    {
        if (profN > AppData.profiles.Count)
            profN = AppData.profiles.Count;
        else if (profN < 0)
            profN = 0;
        AppData.selected = profN;

        if (AppData.selected == AppData.profiles.Count)
        {
            main.playerName.text = "Add Profile";
            main.playerStatus.text = "Add a new profile for your child.";
            main.lastLogin.text = "";
            main.lastLogin.transform.GetChild(0).GetComponent<Text>().text = "";
        } else {
            main.playerName.text = AppData.getChild().profilename;
            if (AppData.getChild().active)
            {
                main.playerStatus.text = "Status: Playing";
                main.lastLogin.text = "Logged in:";
            }
            else
            {
                main.playerStatus.text = "Status: Offline";
                main.lastLogin.text = "Last seen:";
            }
            main.lastLogin.transform.GetChild(0).GetComponent<Text>().text = getTimeString(AppData.getChild().lastlogin);
        }

        main.profileSelector.updateX();

		// Invoke onselect method if add screen wasn't selected
		if (profN != AppData.profiles.Count)
			main.onSelect.Invoke ();
    }

    private static string getTimeString(int unixTime)
    {
        DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        time = time.AddSeconds(unixTime).ToLocalTime();
        return time.ToString().Split(' ')[1] + "\n" + time.ToString().Split(' ')[0];
    }
}
