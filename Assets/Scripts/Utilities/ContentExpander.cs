using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentExpander : MonoBehaviour {
    
    public void updateSize()
    {
        float highest = -10000.0f;
        float lowest = 10000.0f;
		foreach (RectTransform rt in GetComponentsInChildren<RectTransform>()) {
			if (!rt.gameObject.activeInHierarchy || !rt.gameObject.name.StartsWith("Puzzle"))
				continue;

			float a = rt.anchoredPosition.y + (rt.rect.height / 2);
			float b = rt.anchoredPosition.y - (rt.rect.height / 2);
			if (a > highest)
				highest = a;
			if (b < lowest)
				lowest = b;
		}
		float size = highest - lowest;
		GetComponent<RectTransform>().sizeDelta = new Vector2(1080, size + 60);
     }
}
