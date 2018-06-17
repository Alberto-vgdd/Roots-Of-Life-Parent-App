using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : Object {
    public string name;
    public bool active = false;
    public Image image;
	public int progress;
	public float completion;
	public int playtime;
	public int openings;
	public int deathcount;

	public Profile(string name, Image image, int progress, float completion, int playtime, int openings, int deathcount)
    {
        this.name = name;
        this.image = image;
        this.image.name = name;
        this.image.rectTransform.anchoredPosition = new Vector2(0, 0);
		this.progress = progress;
		this.completion = completion;
		this.playtime = playtime;
		this.openings = openings;
		this.deathcount = deathcount;
    }

}
