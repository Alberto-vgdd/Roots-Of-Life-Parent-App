using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : Object {
    public string name;
    public bool active = false;
    public Image image;
    public int lastlogin;

	public Profile(string name, Image image, int lastlogin)
    {
        this.name = name;
        this.image = image;
        this.image.name = name;
        this.image.rectTransform.anchoredPosition = new Vector2(0, 0);
        this.lastlogin = lastlogin;
    }

}
