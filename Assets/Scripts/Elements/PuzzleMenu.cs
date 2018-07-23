using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleMenu : MonoBehaviour {

	public List<TextAsset> puzzledata;
	public Transform puzzleTemplate;

	private List<PuzzleButton> puzzleButtons;

	// Use this for initialization
	public void Initialise() {
		puzzleButtons = new List<PuzzleButton> ();
		foreach (TextAsset p in puzzledata)
			loadPuzzle (System.Text.Encoding.Default.GetString (p.bytes));
		puzzleTemplate.gameObject.SetActive (false);
	}

	// Extract puzzle data out of string
	private void loadPuzzle (string data) {

		// Obtain name and summary from data
		string[] e = data.Replace("\r","").Replace("\n", "").TrimEnd('#').Split ('#');
		string levelName = e [0];
		string levelText = e [1];

		// Create new instance of puzzle button and place it
		PuzzleButton newPuzzle = Instantiate (puzzleTemplate).GetComponent<PuzzleButton>();
		newPuzzle.transform.SetParent (transform.GetChild(0).transform);
		newPuzzle.GetComponent<RectTransform> ().anchoredPosition = new Vector2(0, -132 - (230 * puzzleButtons.Count));
		newPuzzle.GetComponent<RectTransform> ().sizeDelta = puzzleTemplate.GetComponent<RectTransform> ().sizeDelta;
		newPuzzle.GetComponent<RectTransform> ().localScale = puzzleTemplate.GetComponent<RectTransform> ().localScale;
		if (puzzleButtons.Count > 0)
			puzzleButtons [puzzleButtons.Count - 1].nextButton = newPuzzle;
		puzzleButtons.Add (newPuzzle);

		// Load puzzle data into puzzle button
		newPuzzle.name = "Puzzle" + puzzleButtons.Count;
		newPuzzle.title.text = "Puzzle " + puzzleButtons.Count + ": " + levelName;
		newPuzzle.summary.text = levelText;

		// Update canvas in order to update the size of the summary text (otherwise the puzzlebutton cannot be initialised)
		Canvas.ForceUpdateCanvases ();

		// Initialise button
		newPuzzle.Initialise ((puzzleButtons.Count - 1), data);
	}
}
