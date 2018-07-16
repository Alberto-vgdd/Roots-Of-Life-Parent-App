using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCommunicator : MonoBehaviour {
	string URL = "http://62.131.170.46/roots-of-life/setFlag.php";
    public string flag;
    private bool switcher;

	IEnumerator form(int value)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", AppData.getChild().profileName);
        form.AddField("flagName", flag);
		form.AddField ("flagValue", value);

        WWW www = new WWW(URL, form);
        yield return www;
    }

	public void setFlag(int value)
    {
        StartCoroutine(form(value));
    }
}
