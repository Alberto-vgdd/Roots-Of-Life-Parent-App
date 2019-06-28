using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddUserScreen : ScreenBehaviour
{
    string URL = "http://62.131.170.46/roots-of-life/profileInsert.php";

    public InputField nameInput;

    public void onButtonPress()
    {
        if (nameInput.text == "")
            return;
        
        StartCoroutine(form(nameInput.text, AppData.parentID));
        gameObject.SetActive(false);
    }

    IEnumerator form(string username, int parentid)
    {
        WWWForm form = new WWWForm();
        form.AddField("setUsername", username);
        form.AddField("setParentID", parentid);

        WWW www = new WWW(URL, form);
        yield return www;

        // Reload users
        ProfileManager.loadUsers();
    }
}
