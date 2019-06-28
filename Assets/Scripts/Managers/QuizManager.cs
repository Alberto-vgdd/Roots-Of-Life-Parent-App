using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour {

	// TODO add database communication stuff for when quiz ends!!

	private static QuizManager main;

	// Inspector input
	public List<TextAsset> puzzledata;
	public Text questionNumber;
	public Text questionText;
	public Text resultText;
	public Text resultNumber;
	public GameObject optionToggles;
	public GameObject result;

	private static List<Quiz> quizzes;
	private static List<string> questions;
	private static List<string> answers;
	private static List<Toggle> options;

	private static int questionN;
	private static int answerN;
	private static int correct;
	private static int active;
	private static bool playing;

	// Use this for initialization
	void Start () {
		main = this;

		// Store answer toggles from the gameobject
		options = new List<Toggle> ();
		for (int i = 0; i < 4; i++)
			options.Add (optionToggles.transform.GetChild (i + 1).GetComponent<Toggle> ());

		// Load quizzes and store data
		quizzes = new List<Quiz> ();
		foreach (TextAsset p in puzzledata)
			quizzes.Add (new Quiz(System.Text.Encoding.Default.GetString (p.bytes)));
	}

	public static void startQuiz(int i) {

		// Load quiz from data
		questions = quizzes[i].questions;
		answers = quizzes[i].answers;

		// Setup variables
		playing = true;
		questionN = 0;
		answerN = -1;
		correct = 0;
		active = i;

		// Display quiz
		ScreenManager.showScreen("quiz");
		displayQuestion ();
	}

	public void endQuiz() {

		// End quiz and display score
		optionToggles.SetActive (false);
		questionText.gameObject.SetActive (false);
		result.SetActive (true);
		resultText.text = "You answered\n\n\nout of " + questionN + " questions correctly!";
		resultNumber.text = "" + correct;
		questionNumber.text = "The End!";

		FlagManager.setFlag ("quiz"+active, 1);

		playing = false;
	}

	public void closeQuiz() {
		active = -1;
		ScreenManager.toggleScreen ("quiz");
	}

	// Display the question and answers of the currently selected question
	private static void displayQuestion() {
		
		// Replace question number and text
		main.questionNumber.text = "Question " + (questionN + 1);
		main.questionText.text = questions [questionN];

		int[] order = { 0, 1, 2, 3 };
		for (int i = 0; i < 3; i++) {
			int r = UnityEngine.Random.Range (i, 4);
			int tmp = order [i];
			order [i] = order [r];
			order [r] = tmp;
		}

		// Put answer text in the answer toggles
		for (int i = 0; i < 4; i++) {
			options [i].GetComponentInChildren<Text> ().text = answers [(questionN * 4) + order[i]];
			options [i].isOn = false;
		}
	}

	// When the player selects a new answer from the list of toggles
	public void select() {

		// Store new selected number in a variable
		int newAnswer = -1;
		for (int i = 0; i < 4; i++)
			if (options [i].isOn && i != answerN)
				newAnswer = i;

		// Disable previous selected answer if a selection was made
		if (newAnswer != -1 && answerN != -1)
			options [answerN].isOn = false;

		// Store number of new answer
		answerN = newAnswer;
	}
		
	// When the player presses the button
	public void check() {

		// If the quiz has ended, close the quiz
		if (!playing) {
			closeQuiz ();

			// Reset results display
			result.SetActive (false);
			optionToggles.SetActive (true);
			return;
		}

		// Make sure an answer is provided
		if (answerN == -1) 
			return;

		// Check if answer is correct
		if (options [answerN].GetComponentInChildren<Text> ().text == answers [questionN * 4])
			correct++;

		// Reset selected answer
		answerN = -1;

		// Check if this was the final question
		if (questionN == (questions.Count - 1))
			endQuiz ();
		else {
			
			// Display next question
			questionN++;
			displayQuestion ();
		}
	}
}
