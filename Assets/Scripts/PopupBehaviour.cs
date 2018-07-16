using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupBehaviour : MonoBehaviour {

    public GameObject PopupField;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Popup(string popupMessage)
    {
        StartCoroutine(popup(popupMessage));
    }


    // Display message to the user
    IEnumerator popup(string popup)
    {
        PopupField.GetComponentInChildren<Text>().text = popup;

        PopupField.SetActive(true);
        yield return new WaitForSeconds(3);
        PopupField.SetActive(false);
        PopupField.GetComponentInChildren<Text>().text = "Popup Text";
    }
}
