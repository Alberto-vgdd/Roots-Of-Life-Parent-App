using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    // Used to make static references to instance
    public LoginManager main;

    // Inspector input
    public InputField nameInput;
    public InputField passInput;
    public InputField passControl;
    public Toggle rememberToggle;
    public Toggle automaticToggle;

    // Variables used for login request
    private string username;
    private string password;

    // PHP urls
    private static string loginURL = "http://62.131.170.46/roots-of-life/loginRequest.php";
    private static string registerURL = "http://62.131.170.46/roots-of-life/parentInsert.php";

    // Use this for initialization
    void Start () {
        
        // Check if username should be remembered
        if (PlayerPrefs.GetInt("remember") == 1)
        {
            rememberToggle.isOn = true;
            username = PlayerPrefs.GetString("username");
            nameInput.text = username;
        }

        // Check if user should be logged inautomatically
        if (PlayerPrefs.GetInt("automatic") == 1)
        {
            password = PlayerPrefs.GetString("password");
            automaticToggle.isOn = true;
            StartCoroutine(loginRequest());
        }
        
        // If user is not logged in, show login screen
        if (!AppData.loggedIn)
            ScreenManager.showScreen("login");
    }
    
    // WWW form used to process login request
    public IEnumerator loginRequest()
    {
        // Set up form
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setPassword", password);
        WWW www = new WWW(loginURL, form);

        // Execute form and store result
        yield return www;
        int result;
        if (!string.IsNullOrEmpty(www.error))
            yield break;
        else
            result = int.Parse(www.text);

        // result represents the result of the login request. if succesful, it returns the value of the parent ID
        // (and thus result is equal to a value higher than 0)
        if (result > 0)
        {
            // Store new settings for remembering
            if (rememberToggle.isOn)
            {
                PlayerPrefs.SetInt("remember", 1);
                PlayerPrefs.SetString("username", username);
            }
            else
            {
                PlayerPrefs.SetInt("remember", 0);
                PlayerPrefs.SetString("username", "");
                nameInput.text = "";
                username = "";
            }
            if (automaticToggle.isOn)
            {
                PlayerPrefs.SetInt("automatic", 1);
                PlayerPrefs.SetString("password", password);
            }
            else
            {
                PlayerPrefs.SetInt("automatic", 0);
                PlayerPrefs.SetString("password", "");
                passInput.text = "";
                password = "";
            }

            // Store result in app data
            AppData.parentID = result;
            AppData.loggedIn = true;

            // Check if user needs to see intro or tutorial
			if (PlayerPrefs.GetInt ("skipIntro") == 0) 
				ScreenManager.showScreen ("intro");
			else 
				ProfileManager.loadUsers ();

            // Disable login manager
            ScreenManager.clearScreen();

        // A result value of -1 means the account was found but the password was wrong
        } else if (result == -1)
            ScreenManager.Popup("Wrong password.");
        // A result value of -2 means the account was not found
        else if (result == -2)
            ScreenManager.Popup("Account not found, please register.");
    }

    // WWW form used to insert new parent in the database
    public IEnumerator registerParent()
    {
        // Set up form
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setPassword", password);
        WWW www = new WWW(registerURL, form);

        // Execute form
        yield return www;
    }

    // Handle logout request
    public void logOut()
    {
        ProfileManager.unloadUsers();
        AppData.parentID = -1;
        AppData.loggedIn = false;
        ScreenManager.showScreen("login");
    }

    // Handle login request
    public void logIn()
    {
        if (nameInput.text == null || nameInput.text == "")
            return;
        if (passInput.text == null || passInput.text == "")
            return;
        username = nameInput.text;
        password = passInput.text;

        StartCoroutine(loginRequest());
    }

    // Handle register request
    public void register()
    {
        if (passInput.text != passControl.text)
        {
            ScreenManager.Popup("Your passwords do not match.");
            return;
        }

        ScreenManager.Popup("Account created, you are now logged in.");
        StartCoroutine(registerParent());
        StartCoroutine(loginRequest());

        return;
    }
}
