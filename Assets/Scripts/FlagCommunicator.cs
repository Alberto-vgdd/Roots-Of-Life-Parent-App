using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCommunicator : MonoBehaviour {
	string URL = "http://62.131.170.46/roots-of-life/setFlag.php";
    public ProfileSelector profileSelector;
    public string flag;
    private bool switcher;

    // Use this for initialization
    void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

	IEnumerator form(int value)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", profileSelector.profiles[profileSelector.selected].name);
        form.AddField("flagName", flag);
		form.AddField ("flagValue", value);

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
    }

	public void setFlag(int value)
    {
        StartCoroutine(form(value));
    }
}
