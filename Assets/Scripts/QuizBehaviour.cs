using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizBehaviour : MonoBehaviour {

    public GameObject questionNumber;
    public Text question;
    public GameObject answerOptions;
	public FlagCommunicator flagCommunicator;

    public List<string> questions;
    public List<string> answers;

    private int questionN;
    private int answerN;
    private List<Toggle> answerToggles;

    private void OnValidate()
    {
        List<string> oldAnswers = answers;
        answers = new List<string>();
        for (int i = 0; i < questions.Count * 4; i++)
        {
            if (oldAnswers.Count > i)
                answers.Add(oldAnswers[i]);
            else
                answers.Add("");
        }
    }

    // Use this for initialization
    void Start () {
        answerToggles = new List<Toggle>();
        for (int i = 0; i < 4; i++)
        {
            answerToggles.Add(answerOptions.transform.GetChild(i + 1).GetComponent<Toggle>());
        }
        questionN = 0;
        loadQuestion();
        answerN = -1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onSelect()
    {
        int newAnswer = -1;
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(answerToggles[i].isOn);
            if (answerToggles[i].isOn && i != answerN)
                newAnswer = i;
        }
        if (newAnswer != -1 && answerN != -1)
            answerToggles[answerN].isOn = false;
        answerN = newAnswer;
    }

    public void onButton()
    {
        foreach (Toggle a in answerToggles)
        {
            if (a.isOn)
            {
                questionN++;
				if (questionN >= questions.Count)
				{
					flagCommunicator.setFlag (1);
					gameObject.SetActive (false);
					questionN = 0;
				}
				loadQuestion ();
                break;
            }
        }
    }

    private void loadQuestion()
    {
        questionNumber.GetComponentInChildren<Text>().text = "Question " + (questionN + 1);
        question.text = questions[questionN];
        for (int i = 0; i < 4; i++)
        {
            answerToggles[i].GetComponentInChildren<Text>().text = answers[(questionN * 4) + i];
            answerToggles[i].isOn = false;
        }
    }
}
