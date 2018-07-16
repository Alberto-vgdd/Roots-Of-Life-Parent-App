using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

    // PHP urls
    private static string loginURL = "http://62.131.170.46/roots-of-life/loginRequest.php";
    private static string registerURL = "http://62.131.170.46/roots-of-life/insertParent.php";

    // Set up references to the UI objects
    public GameObject loginScreen;
    public InputField nameInput;
    public InputField passInput;
    public InputField passControl;
    public Toggle rememberToggle;
    public Toggle automaticToggle;

    // Variables used for login request
    private string username;
    private string password;
    
    // Use this for initialization
    void Start () {
        AppData.loggedIn = false;

        // Check if username is to be remembered
        if (PlayerPrefs.GetInt("remember") == 1)
        {
            rememberToggle.isOn = true;
            username = PlayerPrefs.GetString("username");
            GameObject.Find("NameInput").GetComponent<InputField>().text = username;
        }

        // Check if user is to be logged inautomatically
        if (PlayerPrefs.GetInt("automatic") == 1)
        {
            password = PlayerPrefs.GetString("password");
            automaticToggle.isOn = true;
            StartCoroutine(loginRequest());
        }
	}
    
    // Create WWW form to process login
    public IEnumerator loginRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setPassword", password);

        WWW www = new WWW(loginURL, form);
        yield return www;
        int result;
        if (!string.IsNullOrEmpty(www.error))
            yield break;
        else
            result = int.Parse(www.text);
        yield return result;

        if (result > 0)
        {
            if (rememberToggle.isOn)
            {
                PlayerPrefs.SetInt("remember", 1);
                PlayerPrefs.SetString("username", username);
            }
            else
            {
                PlayerPrefs.SetInt("remember", 0);
                PlayerPrefs.SetString("username", "");
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
            }
            
            AppData.parentID = result;
            AppData.loggedIn = true;
            GameObject.Find("ProfileManager").GetComponent<ProfileManager>().loadUsers();

            // Disable login manager
            loginScreen.SetActive(false);
        } else if (result == -1)
            GameObject.Find("PopupManager").GetComponent<PopupBehaviour>().Popup("Wrong password.");
        else if (result == -2)
            GameObject.Find("PopupManager").GetComponent<PopupBehaviour>().Popup("Account not found, please register.");
    }

    // Create WWW form to insert parent
    public IEnumerator registerParent()
    {
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setPassword", password);

        WWW www = new WWW(registerURL, form);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("error: " + www.error);
        }
        else
        {
            Debug.Log("result: " + www.text);
        }
    }

    // Handle logout request
    public void logOut()
    {
        GameObject.Find("ProfileManager").GetComponent<ProfileManager>().unloadUsers();
        AppData.parentID = -1;
        AppData.loggedIn = false;
        loginScreen.SetActive(true);
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
            GameObject.Find("PopupManager").GetComponent<PopupBehaviour>().Popup("Your passwords do not match.");
            return;
        }

        GameObject.Find("PopupManager").GetComponent<PopupBehaviour>().Popup("Account created, you are now logged in.");
        StartCoroutine(registerParent());
        StartCoroutine(loginRequest());

        return;
    }
}
