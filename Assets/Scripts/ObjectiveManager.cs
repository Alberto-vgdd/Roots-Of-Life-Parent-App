using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour {
    public GameObject content;
    public InputField taskNameInput;
    public Dropdown rewardInput;
    public Transform template;
    public Text templateLabel;
	public ProfileSelector selector;
    public List<Transform> objectives;

	string URL = "http://143.176.117.92/roots-of-life/insertObjective.php";

	// Use this for initialization
	void Start () {
        objectives = new List<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void create(int reward)
    {
        templateLabel.text = taskNameInput.text;
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
        Debug.Log("sorting: " + objectives.Count);
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

    void resetTemplate()
    {
        taskNameInput.text = "";
    }

    public void onCreate()
    {
        if (taskNameInput.text == "")
            return;
        /*int reward = rewardInput.value;
        create(reward);
        sort();
        resetTemplate();*/
		StartCoroutine (form ());
		resetTemplate ();
    }

	IEnumerator form()
	{
		WWWForm form = new WWWForm();
		form.AddField ("setObjective", taskNameInput.text);
		form.AddField ("setUser", selector.profiles [selector.selected].name);
		form.AddField ("setReward", rewardInput.value);

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

    public void complete()
    {
        Debug.Log("complete");
        Transform completed = null;
        foreach (Transform objective in objectives)
        {
            if (!objective.GetComponent<Toggle>().isOn)
                continue;
            completed = objective;
            Debug.Log("found objective");
            break;
        }
        if (completed != null)
            objectives.Remove(completed);
        Object.Destroy(completed.gameObject);
        sort();
    }
}
