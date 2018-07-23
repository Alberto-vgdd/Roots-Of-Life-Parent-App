using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsMenu : MonoBehaviour {

	public Image progressImage;
	public Text percentageText;
	public Text clockText;
	public Text seedsText;
	public Text gameOverText;
	public Text levelsCompletedText;
	public Text quizzesCompletedText;
	public Text secretAreasText;
	public Text batsKilled;
	public Text catsKilled;

	public void Initialise() {

		// Set progress bar values
		int percentage = FlagManager.getFlag ("progress");
		progressImage.fillAmount = percentage / 100f;
		percentageText.text = percentage + "%";

		// Set general statistics
		clockText.text = FlagManager.getFlag("hrs") + ":" + FlagManager.getFlag("mins");
		seedsText.text = FlagManager.getFlag ("seeds").ToString();
		gameOverText.text = FlagManager.getFlag ("gameover").ToString();

		// Set specific statistics
		levelsCompletedText.text = FlagManager.getFlag("lvlcmplt").ToString();
		quizzesCompletedText.text = FlagManager.getFlag("quizcmplt").ToString();
		secretAreasText.text = FlagManager.getFlag("scrtarea").ToString();
		batsKilled.text = FlagManager.getFlag("bats").ToString();
		catsKilled.text = FlagManager.getFlag("cats").ToString();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
