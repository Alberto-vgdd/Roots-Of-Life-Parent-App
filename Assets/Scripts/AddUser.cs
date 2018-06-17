using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddUser : MonoBehaviour
{

    string URL = "http://62.131.170.46/roots-of-life/insertUser.php";

    public InputField nameInput;
    public LoginManager loginManager;
    public ProfileSelector profileSelector;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onButtonPress()
    {
        if (nameInput.text == "")
            return;

        int parentID = loginManager.getParentID();
        StartCoroutine(form(nameInput.text, parentID));
        profileSelector.loadUsers();
        gameObject.SetActive(false);
    }

    IEnumerator form(string username, int parentid)
    {
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setParentID", parentid);

        WWW www = new WWW(URL, form);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("error: " + www.error);
        }
        else
        {
            Debug.Log("result123: " + www.text);
        }
    }
}
