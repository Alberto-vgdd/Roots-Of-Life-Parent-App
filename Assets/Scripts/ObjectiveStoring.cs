using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ObjectiveStoring : MonoBehaviour {
	
	public ProfileSelector selector;
	public ObjectiveLoading loader;
	public Text textfield;

	public UnityEvent onCreateObjective;

	string URL = "http://143.176.117.92/roots-of-life/insertObjective.php";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onCreate()
	{
		Debug.Log("create");
		if (textfield.text == "")
			return;

		StartCoroutine (form ());
		textfield.text = "";
		loader.updateObjectives ();
		onCreateObjective.Invoke ();
	}

	IEnumerator form()
	{
		WWWForm form = new WWWForm();
		form.AddField ("setObjective", textfield.text);
		form.AddField ("setUser", selector.profiles [selector.selected].name);
		form.AddField ("setReward", getRewardInput().value);

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

	private Dropdown getRewardInput()
	{
		return transform.GetChild (3).GetComponent<Dropdown> ();
	}
}
