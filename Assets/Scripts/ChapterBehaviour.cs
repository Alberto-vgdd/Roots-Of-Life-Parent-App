using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChapterBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
    public GameObject content;
	public State state = State.closed;
	public Image chapterMask;
	public ChapterBehaviour nextChapter;
	public ScrollRect parent;

	public static float openTime = 0.3f;
	public float t = 0.0f;
	private float opensize;

	private static List<ChapterBehaviour> chapters = new List<ChapterBehaviour>();
	private bool drag;

	public enum State {
		opening,
		open,
		closing,
		closed
	}

	// Use this for initialization
	void Start () {
		chapters.Add (this);
		RectTransform rT = GetComponent<RectTransform> ();
		opensize = chapterMask.GetComponent<RectTransform> ().sizeDelta.y + 100;
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.opening || state == State.closing)
			animate ();
	}

	private void animate() {
		t += Time.deltaTime;

		// Calculate p for t as percentage of openTime, if t exceeds opentime then p will be 1.0 and the state will be switched
		float p = getPercentage();
		RectTransform rT = GetComponent<RectTransform> ();

		// Adjust size of textfield according to p
		float d = Mathf.Lerp(200, opensize, p) - rT.sizeDelta.y;
		rT.sizeDelta = new Vector2(rT.sizeDelta.x,rT.sizeDelta.y + d);

		// Adjust y of textfield according to p
		Vector2 position = rT.anchoredPosition;
		position.y -= d * 0.5f;
		rT.anchoredPosition = position;

		// Adjust mask of textfield according to p
		chapterMask.fillAmount = p;

		// Move lower chapters based on size increment
		if (nextChapter != null)
			nextChapter.move (d);
	}

	private float getPercentage() {
		if (t >= openTime) {
			t = 0.0f;
			switchState ();
			if (state == State.open)
				return 1.0f;
			else
				return 0.0f;
		}
		if (state == State.opening)
			return t / openTime;
		else
			return (t / openTime - 1) * -1;
	}

	private void move (float difference) {
		RectTransform rT = GetComponent<RectTransform> ();
		Vector2 position = rT.anchoredPosition;
        position.y -= difference;
		rT.anchoredPosition = position;
		if (nextChapter != null)
			nextChapter.move (difference);
	}

	private void switchState() {
        if (state == State.opening)
            state = State.open;
        else if (state == State.closing)
            state = State.closed;
        content.GetComponent<ContentExpander>().updateSize();
    }

	public void onClick() {
		if (drag) {
			drag = false;
			return;
		}
		if (state == State.closed || state == State.closing) {
			foreach (var c in chapters) {
				c.close ();
			}
			state = State.opening;
		} else
			close ();
	}

	private void close() {
		if (state == State.open || state == State.opening) 
			state = State.closing;
	}

	public void OnPointerDown(PointerEventData eventData) {
		parent.OnBeginDrag (eventData);
	}

	public void OnPointerUp(PointerEventData eventData) {
		parent.OnEndDrag(eventData);
	}

	public void OnDrag(PointerEventData eventData) {
		parent.OnDrag (eventData);
		drag = true;
	}
}
