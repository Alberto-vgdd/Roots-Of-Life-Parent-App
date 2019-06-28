using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour {
	private static FlagManager main;
	private static string setURL = "http://62.131.170.46/roots-of-life/profileSetFlag.php";
	private static string getURL = "http://62.131.170.46/roots-of-life/profileGetFlags.php";
	private string[] flagData;
	private bool download;

	// Use this for initialization
	void Start () {
		main = this;
	}

	void Update() {
	}

	public static void setFlag(string flag, int value)
	{
		main.StartCoroutine(setform(flag, value));
	}

	public static int getFlag(string flag) 
	{
		IEnumerator flags = getForm (flag);
		while (flags.MoveNext ()) {
		}

		if (main.flagData == null)
			return 0;
		
		foreach (string data in main.flagData) 
			if (data.Split (':') [0] == flag) {
				return int.Parse (data.Split (':') [1]);
				break;
			}
		return 0;
	}

	static IEnumerator setform(string flag, int value)
	{
		WWWForm form = new WWWForm();
		form.AddField("userID", AppData.getChild().id);
		form.AddField("flagName", flag);
		form.AddField ("flagValue", value);

		WWW www = new WWW(setURL, form);
		yield return www;
	}

	static IEnumerator getForm(string flag) {
		WWWForm form = new WWWForm();
		form.AddField("setUserID", AppData.getChild().id);

		WWW www = new WWW(getURL, form);
		yield return www;

		while (!www.isDone) {
			yield return null;
		}

		if (www.text != "") 
			main.flagData = www.text.TrimEnd (',').Split (',');
		else 
			main.flagData = null;
	}
}
