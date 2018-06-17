using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

    private string requestURL = "http://62.131.170.46/roots-of-life/loginRequest.php";
    private string insertURL = "http://62.131.170.46/roots-of-life/insertParent.php";

    public InputField nameInput;
    public InputField passInput;
    public InputField passControl;
    public Button cancelButton;
    public Toggle rememberToggle;
    public Toggle automaticToggle;
    public GameObject errorField;
    public ProfileSelector profileSelector;

    private string username;
    private string password;

    private bool register;
    private bool loggedIn;
    private int parentID;

    public int getParentID()
    {
        return parentID;
    }

	// Use this for initialization
	void Start () {
        loggedIn = false;
        if (PlayerPrefs.GetInt("remember") == 1)
        {
            rememberToggle.isOn = true;
            username = PlayerPrefs.GetString("username");
            nameInput.text = username;
        }
        if (PlayerPrefs.GetInt("automatic") == 1)
        {
            password = PlayerPrefs.GetString("password");
            automaticToggle.isOn = true;
        }

        if (automaticToggle.isOn)
            StartCoroutine(loginRequest());

        register = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void logOut()
    {
        gameObject.SetActive(true);
        parentID = -1;
        profileSelector.unloadUsers();
    }

    private void registerMode()
    {
        register = true;
        rememberToggle.gameObject.SetActive(false);
        automaticToggle.gameObject.SetActive(false);
        passControl.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
    }

    public void loginMode()
    {
        register = false;
        rememberToggle.gameObject.SetActive(true);
        automaticToggle.gameObject.SetActive(true);
        passControl.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }

    public void onPress()
    {
        if (nameInput.text == null || nameInput.text == "")
            return;
        if (passInput.text == null || passInput.text == "")
            return;
        username = nameInput.text;
        password = passInput.text;

        if (register)
        {
            // do register stuff
            if (passInput.text != passControl.text)
            {
                StartCoroutine(error(3));
                return;
            }

            StartCoroutine(error(4));
            StartCoroutine(insertParent());
            loginMode();

            return;
        }

        StartCoroutine(loginRequest());
        if (!loggedIn)
            return;
    }

    private void loginSuccesful()
    {
        loggedIn = true;

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

        profileSelector.StartCoroutine(profileSelector.loadUsers(parentID));

        // load profiles in profileselector

        gameObject.SetActive(false);
    }

    IEnumerator insertParent()
    {
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setPassword", password);

        WWW www = new WWW(insertURL, form);
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

    IEnumerator loginRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setPassword", password);

        WWW www = new WWW(requestURL, form);
        yield return www;
        int result;
        if (!string.IsNullOrEmpty(www.error))
        {
            //Debug.Log("error: " + www.error);
            yield break;
        }
        else
        {
            //Debug.Log("result: " + www.text);
            result = int.Parse(www.text);
        }

        if (result == -1)
        {
            // wrong password
            passInput.text = "";
            StartCoroutine(error(1));
        }
        else if (result == -2)
        {
            // enter register mode
            registerMode();
            StartCoroutine(error(2));
        }
        else
        {
            Debug.Log("result:" + result);
            // logged in;
            parentID = result;
            loginSuccesful();
        }
    }

    IEnumerator error(int error)
    {
        if (error == 1)
            errorField.GetComponentInChildren<Text>().text = "Wrong password.";
        if (error == 2)
            errorField.GetComponentInChildren<Text>().text = "Account not found, please register.";
        if (error == 3)
            errorField.GetComponentInChildren<Text>().text = "Passwords don't match.";
        if (error == 4)
            errorField.GetComponentInChildren<Text>().text = "Account created. You can now log in.";

        errorField.SetActive(true);
        yield return new WaitForSeconds(3);
        errorField.SetActive(false);
    }
}
