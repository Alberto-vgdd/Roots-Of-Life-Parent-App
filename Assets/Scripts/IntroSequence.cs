using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroSequence : MonoBehaviour {
    public Text text;
    public List<string> messages;
    private int currentMessage;

	// Use this for initialization
	void Start () {
        reset();
        if (PlayerPrefs.GetInt("skipIntro") == 1)
            gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void reset()
    {
        currentMessage = 0;
        text.text = messages[currentMessage];
    }

    public void clickButton()
    {
        currentMessage++;
        StartCoroutine(showMessage());
    }

    IEnumerator showMessage()
    {
        text.CrossFadeAlpha(0, 0.5f, false);
        if (currentMessage == messages.Count)
        {
            yield return new WaitForSeconds(0.5f);
            reset();
            gameObject.SetActive(false);
            PlayerPrefs.SetInt("skipIntro", 1);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        text.text = messages[currentMessage];
        text.CrossFadeAlpha(1, 0.5f, false);
    }
}
