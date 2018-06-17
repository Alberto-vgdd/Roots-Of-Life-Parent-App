using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveLoading : MonoBehaviour {
	
	string loadURL = "http://143.176.117.92/roots-of-life/getObjectives.php";
	string delURL = "http://143.176.117.92/roots-of-life/removeObjective.php";

	public ProfileSelector selector;
	public Transform template;
	public Text templateLabel;
	public GameObject content;

	List<Transform> objectives;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void updateObjectives() {
		Debug.Log ("update");
		StartCoroutine (loadObjectives ());
	}

	public IEnumerator loadObjectives() {
		Debug.Log ("loading");
		if (objectives != null)
			foreach (Transform o in objectives) {
				Destroy (o.gameObject);
			}
		objectives = new List<Transform> ();

		WWWForm form = new WWWForm ();
		form.AddField ("setUser", selector.getSelected().name);

		WWW www = new WWW (loadURL, form);
		yield return www;
		string objectivesString = www.text.TrimEnd (';');
		if (objectivesString != "")
			foreach (string o in objectivesString.Split(';')) 
			{
				int reward = (int) char.GetNumericValue (o.ToCharArray () [0]);
				string objective = o.Substring(2);
				create (reward, objective);
			}
		Debug.Log ("loaded");
		sort ();
		Debug.Log (objectives.Count);
	}

	void create(int reward, string text)
	{
		templateLabel.text = text;
		Transform newObjective = Instantiate(template);
		newObjective.SetParent(gameObject.transform, false);
		string rewardtext = "";
		if (reward == 0)
			rewardtext = "10 Seeds";
		if (reward == 1)
			rewardtext = "50 Seeds";
		if (reward == 2)
			rewardtext = "150 Seeds";
		newObjective.GetComponent<Image>().GetComponentInChildren<Text>().text = rewardtext;
		objectives.Add(newObjective);
	}

	void sort()
	{
		int index = 0;
		foreach (Transform objective in objectives)
		{
			Vector2 pos = template.GetComponent<RectTransform>().anchoredPosition;
			pos.y -= (objective.GetComponent<RectTransform>().sizeDelta.y + 10) * index;
			objective.GetComponent<RectTransform>().anchoredPosition = pos;
			objective.gameObject.SetActive(true);

			index++;
		}
		calculateSize();
		content.GetComponent<ContentExpander>().updateSize();
	}

	void calculateSize()
	{
		RectTransform rT = GetComponent<RectTransform>();
		rT.sizeDelta = new Vector2(rT.sizeDelta.x, 250 + (150 * objectives.Count));
		rT.anchoredPosition = new Vector2(rT.anchoredPosition.x, -800 - (75 * objectives.Count));
	}

	public void complete()
	{
		Debug.Log("complete");
		Transform completed = null;
		foreach (Transform objective in objectives)
		{
			if (!objective.GetComponent<Toggle>().isOn)
				continue;

			StartCoroutine (form (objective));
			break;
		}
		updateObjectives ();
	}

	IEnumerator form(Transform objective)
	{
		WWWForm form = new WWWForm ();
		form.AddField ("setUser", selector.getSelected().name);
		string objectivestring = objective.GetChild (2).GetComponent<Text>().text;
		Debug.Log (objectivestring);
		form.AddField ("setObjective", objectivestring);

		WWW www = new WWW (delURL, form);
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
}