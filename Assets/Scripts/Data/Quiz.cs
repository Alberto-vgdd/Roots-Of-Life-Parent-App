using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz {

	public List<string> questions;
	public List<string> answers;

	public Quiz (string data) {
		questions = new List<string> ();
		answers = new List<string> ();

		string[] e = data.Replace("\r","").Replace("\n", "").TrimEnd('#').Split ('#');
		int q = 2;
		while (e.Length > q) {
			loadQuestion (e [q]);
			q++;
		}
	}

	// Extract question data out of string
	private void loadQuestion(string data) {
		string[] q = data.Split ('~');
		questions.Add (q [0]);
		answers.Add (q [1]);
		answers.Add (q [2]);
		answers.Add (q [3]);
		answers.Add (q [4]);
	}
}
