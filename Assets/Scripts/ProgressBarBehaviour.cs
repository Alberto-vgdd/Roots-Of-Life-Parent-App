using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarBehaviour : MonoBehaviour {
	public Image progressImage;
	public Text percentageText;
	public ProfileSelector selector;

	public void updateProfile() 
	{
		float percentage = selector.profiles [selector.selected].completion;
		progressImage.fillAmount = percentage;
		percentageText.text = Mathf.Round(percentage * 100) + "%";
	}
}
