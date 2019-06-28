using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreen : ScreenBehaviourExecute
{

    public Image shader;
    public Image transition;
    public List<GameObject> screens;

    private int currentscreen = 0;

	// Use this for initialization
	void Start ()
    {
        transition.gameObject.SetActive(true);
        transition.CrossFadeAlpha(0.0f, 0.0f, false);
    }

    public override void OnShow()
    {
        currentscreen = 0;
        foreach (GameObject s in screens)
            s.SetActive(false);
        screens[currentscreen].SetActive(true);
    }

    public void Transition()
    {
        transition.CrossFadeAlpha(1f, 0.25f, false);
        StartCoroutine(trans());
    }

    IEnumerator trans()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject s in screens)
            s.SetActive(false);
        currentscreen++;
        if (currentscreen == screens.Count)
            disable();
        else
            screens[currentscreen].SetActive(true);
        transition.CrossFadeAlpha(0.0f, 0.25f, false);
        
        if (currentscreen == 1)
        {

        }

        if (currentscreen == 3)
            ProfileManager.loadUsers();
    }

    public void disable()
    {
        gameObject.SetActive(false);
        PlayerPrefs.SetInt("skipTutorial", 1);
    }
}
