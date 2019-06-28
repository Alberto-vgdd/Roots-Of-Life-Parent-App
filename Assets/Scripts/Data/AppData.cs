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

    public static Profile getChild()
    {
        if (selected >= profiles.Count)
            return null;
        return profiles[selected];
    }
}
