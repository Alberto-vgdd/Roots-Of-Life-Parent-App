using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzleButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler, IPointerClickHandler {

	public float time = 0.1f;
	public Image mask;
	public Text title;
	public Text summary;
	public Button button;

	public PuzzleButton nextButton;

	private Image image;
	private Color pressed = new Color(0.8f, 0.8f, 0.8f);
	private bool isOpen;
	private float opensize;
	private float timer;
	private float defaultY;

	// Set up reference to button image
	void Awake() {
		image = GetComponent<Image> ();
	}

	public void Initialise(int buttonindex, string quizdata) {

		// Adjust position of summary text according to size of level summary
		RectTransform rt = summary.GetComponent<RectTransform> ();
		rt.anchoredPosition = new Vector2 (0, -(rt.sizeDelta.y / 2));
		float top = rt.anchoredPosition.y + (rt.sizeDelta.y / 2); // find top coordinate to determine size of mask

		// Place quiz button
		float buttonY = rt.anchoredPosition.y - (rt.sizeDelta.y / 2) - 50; // place button based on size of summary rectangle
		rt = button.GetComponent<RectTransform>();
		buttonY -= rt.sizeDelta.y / 2;
		rt.anchoredPosition = new Vector2 (0, buttonY);
		float bot = rt.anchoredPosition.y - (rt.sizeDelta.y / 2); // find bot coordinate to determine size of mask

		// Adjust size of mask
		rt = mask.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2 (rt.sizeDelta.x, top - bot); // find mask height by comparing top and bot coordinates
		rt.anchoredPosition = new Vector2 (0, -(rt.sizeDelta.y / 2) - 160);
		opensize = rt.sizeDelta.y;

		// Link button to quiz
		button.onClick.AddListener( delegate { QuizManager.startQuiz(buttonindex);	});
		if (FlagManager.getFlag ("quiz"+buttonindex) == 1)
			button.GetComponentInChildren<Text> ().text = "Try quiz again!";

		defaultY = GetComponent<RectTransform> ().anchoredPosition.y;
	}

	public void OnDrag(PointerEventData eventData) {
		if (eventData.pointerCurrentRaycast.gameObject != gameObject)
			image.CrossFadeColor (Color.white, 0.1f, false, true);
		transform.parent.parent.GetComponent<SimpleScroll> ().OnDrag (eventData);
	}

	public void OnPointerDown(PointerEventData eventData) {
		image.CrossFadeColor (pressed, 0.1f, false, true);
	}

	public void OnPointerUp(PointerEventData eventData) {
		image.CrossFadeColor (Color.white, 0.1f, false, true);
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (isOpen)
			StartCoroutine (open(false));
		else {
			foreach (PuzzleButton p in GameObject.FindObjectsOfType(this.GetType()))
				if (p != this && p.isOpen)
					p.StartCoroutine (p.open (false));
			StartCoroutine (open (true));
		}
	}

	IEnumerator open(bool openOrClose) {
		button.gameObject.SetActive (openOrClose);
		RectTransform rt = GetComponent<RectTransform> ();

		timer = time;
		while (timer > 0) {
			timer -= Time.deltaTime;
			float p = (openOrClose) ? (timer * 5f * -1) + 1 : timer * 5f;
			if (p < 0)
				p = 0;
			mask.fillAmount = p;

			rt.sizeDelta = new Vector2 (rt.sizeDelta.x, 200 + (opensize * p));
			rt.anchoredPosition = new Vector2 (0, defaultY - ((opensize * p) / 2));
			if (nextButton != null)
				nextButton.move (-opensize * p);

			yield return new WaitForEndOfFrame ();
		}

		isOpen = openOrClose;
		transform.parent.GetComponent<ContentExpander> ().updateSize ();
	}

	private void move(float distance) {
		RectTransform rt = GetComponent<RectTransform> ();
		rt.anchoredPosition = new Vector2 (0, defaultY + distance);
		if (nextButton != null)
			nextButton.move (distance);
	}
}
