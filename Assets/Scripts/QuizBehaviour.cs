using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizBehaviour : MonoBehaviour {

    public GameObject quiz;
    public GameObject questionNumber;
    public Text question;
    public GameObject answerOptions;
    public GameObject end;
    public Text resultText;
	public FlagCommunicator flagCommunicator;

    public List<string> questions;
    public List<string> answers;

    private int questionN;
    private int answerN;
    private List<Toggle> answerToggles;
    private int correct;

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
            if (answerToggles[i].isOn && i != answerN)
                newAnswer = i;
        }
        if (newAnswer != -1 && answerN != -1)
            answerToggles[answerN].isOn = false;
        answerN = newAnswer;
    }

    public void onClose()
    {
        correct = 0;
        questionN = 0;
        end.SetActive(false);
        quiz.SetActive(true);
        loadQuestion();
    }

    public void onButton()
    {
        foreach (Toggle a in answerToggles)
        {
            if (a.isOn)
            {
                checkQuestion();

                questionN++;
				if (questionN >= questions.Count)
				{
					if (flagCommunicator != null)
						flagCommunicator.setFlag (1);

					quiz.SetActive (false);
                    end.SetActive(true);
                    resultText.text = correct + " out of " + (questions.Count - 1);
				}
				loadQuestion ();
                break;
            }
        }
    }

    private void checkQuestion()
    {
        string answer = "";
        foreach (Toggle a in answerToggles)
        {
            if (!a.isOn)
                continue;
            answer = a.GetComponentInChildren<Text>().text;
        }

        if (answer == answers[questionN * 4])
            correct++;

    }

    private void loadQuestion()
    {
        questionNumber.GetComponentInChildren<Text>().text = "Question " + (questionN + 1);
        question.text = questions[questionN];
        List<int> values = new List<int> { 0, 1, 2, 3 };
        for (int i = 0; i < values.Count; i++)
        {
            int temp = values[i];
            int randomIndex = Random.Range(i, values.Count);
            values[i] = values[randomIndex];
            values[randomIndex] = temp;
        }

        for (int i = 0; i < 4; i++)
        {
            answerToggles[i].GetComponentInChildren<Text>().text = answers[(questionN * 4) + values[i]];
            answerToggles[i].isOn = false;
        }
    }
}
