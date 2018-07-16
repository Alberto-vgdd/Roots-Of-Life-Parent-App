using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppData : MonoBehaviour {

    // Login Manager
    public static int parentID = -1; // ID of the parent account
    public static string username = "test"; // name of the child account
    public static bool loggedIn = false; // Determines if user is logged in

    // Profile Selector
    public static List<Profile> profiles;
    public static int selected = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static Profile getChild()
    {
        return profiles[selected];
    }
}
