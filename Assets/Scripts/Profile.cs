using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : Object {
    public string profileName;
    public bool active = false;
    public int lastlogin;
    public string imageURL;

    public Profile(string name, int lastlogin, string imageURL)
    {
        this.profileName = name;
        this.lastlogin = lastlogin;
        this.imageURL = imageURL;
    }

    public IEnumerator getImage(Image image)
    {
        WWW www = new WWW(imageURL);
        yield return www;
        image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }
}
