using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentExpander : MonoBehaviour {
    public GameObject objectOne;
    public GameObject objectTwo;
    public GameObject objectThree;
    public GameObject objectFour;
    public GameObject objectFive;
    public GameObject objectSix;
    public List<GameObject> objects;

    // Use this for initialization
    void Start () {
        objects = new List<GameObject>();
        if (objectOne != null)
            objects.Add(objectOne);
        if (objectTwo != null)
            objects.Add(objectTwo);
        if (objectThree != null)
            objects.Add(objectThree);
        if (objectFour != null)
            objects.Add(objectFour);
        if (objectFive != null)
            objects.Add(objectFive);
        if (objectSix != null)
            objects.Add(objectSix);
        updateSize();
    }
	
	// Update is called once per frame
	void Update () {
	}
    
    public void updateSize()
    {
        float highest = -10000.0f;
        float lowest = 10000.0f;
        foreach (GameObject gObject in objects)
        {
            RectTransform orT = gObject.GetComponent<RectTransform>();
            float top = orT.anchoredPosition.y + (orT.sizeDelta.y / 2);
            float bot = orT.anchoredPosition.y - (orT.sizeDelta.y / 2);
            if (top > highest)
                highest = top;
            if (bot < lowest)
                lowest = bot;
        }
        float size = highest - lowest;
        RectTransform rT = GetComponent<RectTransform>();
        rT.sizeDelta = new Vector2(rT.sizeDelta.x, size + 60);
     }
}
