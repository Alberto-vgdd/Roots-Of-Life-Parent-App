using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleScroll : MonoBehaviour, IDragHandler {

	public GameObject content;

	private RectTransform contentRect;
	private RectTransform scrollRect;
	private float scroll = 0.0f;
	private float height;
	private bool swap = true;

	void Awake() {
		contentRect = content.GetComponent<RectTransform> ();
		scrollRect = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<RectTransform> ().rect.height != height)
			height = contentRect.rect.height - scrollRect.rect.height;

		RectTransform rt = content.GetComponent<RectTransform> ();
		if (height < 0)
			scroll = 0.0f;
		rt.anchoredPosition = new Vector2 (0, (-contentRect.rect.height / 2)+ (height * scroll));
	}

	public void OnDrag(PointerEventData eventData) {
		scroll += eventData.delta.y * 0.01f;
		if (scroll > 1.0f)
			scroll = 1.0f;
		if (scroll < 0.0f)
			scroll = 0.0f;
	}
}
