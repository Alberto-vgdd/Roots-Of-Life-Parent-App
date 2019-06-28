using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : ScreenBehaviourExecute
{
    public Text text;
    public List<string> messages;
    private int currentMessage;

    override public void OnShow()
    {
        reset();
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
