using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : Object {
    public int id;
    public string profilename;
    public bool active = false;
    public int lastlogin;
    public string imageURL;

    public Profile(int id, string profilename, int lastlogin, string imageURL)
    {
        this.id = id;
        this.profilename = profilename;
        this.lastlogin = lastlogin;
        this.imageURL = imageURL;
    }

    public IEnumerator getImage(Image image)
    {
        if (imageURL == null || imageURL == "")
            yield break;
        WWW www = new WWW(imageURL);
        yield return www;
        image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }
}
