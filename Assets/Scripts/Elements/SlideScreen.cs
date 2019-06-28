using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlideScreen : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {

	public PlayerInfo playerInfo;
	public SlideImageBehaviour slideImage;

	[Range(1.0f, 10.0f)]
	public float scrollSpeed = 5f;
	[Range(0.0f, 1.0f)]
	public float flipPoint = 0.6f;

	// Background image
	public Image image;

	// Value of how far the image is scrolled up (1 = out of screen)
	[Range(0.0f, 1.0f)]
	public float scroll = 0.0f;

	// Determine if the lockscreen is being moved by the code
	private bool isMoving = false;

	// Determine if the lockscreen is being moved by the user
	private bool isDragging = false;

	// Determine if the lockscreen position is up or not (up = the lockscreen is hidden)
	// This value controls the move function, so changing this value will make the code move the lockscreen
	private bool up = false;

	// Use this for initialization
	void Start () {
		if (scroll == 1.0f)
			up = true;
	}
	
	// Update is called once per frame
	void Update () {
		int height = Screen.height;

		// Place image based on scroll level
		image.rectTransform.anchorMin = new Vector2(0, scroll);
		image.rectTransform.anchoredPosition = new Vector2 (0, 0);

		if (isDragging) {
			if (up && scroll < flipPoint)
				up = false;
			if (!up && scroll > flipPoint)
				up = true;
			return;
		}

		// Calculate new scroll position if player is not dragging the lockscreen
		if (isMoving || checkPosition())
			move ();
	}

	// Obtain Game View resolution in editor mode
	static Vector2 getMainGameViewSize() {
		System.Type T = System.Type.GetType ("UnityEditor.GameView,UnityEditor");
		System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod ("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		System.Object Res = GetSizeOfMainGameView.Invoke (null, null);
		return (Vector2)Res;
	}

	void OnValidate() {
		float height = getMainGameViewSize ().y;

		// Place image based on scroll level
		image.rectTransform.anchorMin = new Vector2(0, scroll);
		image.rectTransform.anchoredPosition = new Vector2 (0, 0);
		playerInfo.calc ();
		slideImage.calc ();
	}

	public void swipeUp() {
		if (isMoving)
			return;
		isMoving = true;
		up = true;
	}

	public void swipeDown() {
		if (isMoving)
			return;
		isMoving = true;
		up = false;
	}

	bool checkPosition() {
		// Check if scroll position is different than what the up variable describes it should be
		if ((up && scroll != 1.0f) || (!up && scroll != 0.0f))
			return true;

		// Indicates that the position is as it should be
		return false;
	}

	void move() {
		if (up) {
			scroll += 0.1f * Time.deltaTime * scrollSpeed * 25;
			if (scroll >= 1) {
				scroll = 1.0f;
				isMoving = false;
			}
		} else {
			scroll -= 0.1f  * Time.deltaTime * scrollSpeed * 25;
			if (scroll <= 0) {
				scroll = 0.0f;
				isMoving = false;
			}
		}
	}

	public void OnDrag(PointerEventData eventData) {
		float yDelta = eventData.delta.y / Screen.height;
		scroll += yDelta;
		if (scroll < 0)
			scroll = 0;
		if (scroll > 1)
			scroll = 1;
	}

	public void OnPointerDown(PointerEventData eventData) {
		isDragging = true;
	}

	public void OnPointerUp(PointerEventData eventData) {
		isDragging = false;
	}
}
