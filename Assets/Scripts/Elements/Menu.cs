using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	public Image highlight;
	public GameObject statistics;
	public GameObject puzzles;
	public GameObject messages;
	public MenuOption currentMenuOption;

	private List<GameObject> menus;
	private int currentmenu;
	public enum MenuOption {
		Statistics,
		Puzzles,
		Messages
	}

	// Use this for initialization
	void Start () {

		menus = new List<GameObject>();
		menus.Add (statistics);
		menus.Add (puzzles);
		menus.Add (messages);

		setMenu (MenuOption.Puzzles);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnValidate() {

		menus = new List<GameObject>();
		menus.Add (statistics);
		menus.Add (puzzles);
		menus.Add (messages);

		setMenu (currentMenuOption);
	}

	public void setMenu(MenuOption menu)
	{
		int i = convertMenu (menu);
		if (currentmenu == i || i == -1)
			return;

		foreach (GameObject m in menus)
			m.SetActive(false);
		currentmenu = i;
		menus [i].SetActive (true);
		highlight.rectTransform.anchoredPosition = new Vector2 (180 + (i * 360), 0);
	}

	public void setMenu(string menu)
	{
		int i = convertMenu (menu);
		if (currentmenu == i || i == -1)
			return;

		StartCoroutine(switchMenu(i));
	}

	IEnumerator switchMenu(int i)
	{
		foreach (GameObject m in menus)
			fadeMenu (m, 0.0f, 0.1f);

		float timer = 0.2f;
		float f = 1.0f;
		while (timer >= 0.0f) {
			timer -= Time.deltaTime;
			f -= Time.deltaTime * 5f;
			if (f < 0)
				f = 0;
			highlight.rectTransform.anchoredPosition = new Vector2 (180 + (i * 360) + (f * 360 * (currentmenu - i)), 0);
			yield return new WaitForEndOfFrame ();
		}

		foreach (GameObject m in menus)
			m.SetActive(false);

		currentmenu = i;
		menus [i].SetActive (true);
		fadeMenu (menus [i], 0.0f, 0.0f);
		fadeMenu (menus [i], 1.0f, 0.1f);
	}

	void fadeMenu(GameObject o, float a, float s)
	{
		foreach (Image im in o.GetComponentsInChildren<Image>())
			im.CrossFadeAlpha(a, s, false);
		foreach (Text t in o.GetComponentsInChildren<Text>())
			t.CrossFadeAlpha(a, s, false);
	}

	private int convertMenu(MenuOption menu)
	{
		if (menu == MenuOption.Statistics)
			return 0;
		if (menu == MenuOption.Puzzles)
			return 1;
		if (menu == MenuOption.Messages)
			return 2;
		return -1;
	}

	private int convertMenu(string menu)
	{
		if (menu == "statistics")
			return 0;
		if (menu == "puzzles")
			return 1;
		if (menu == "messages")
			return 2;
		return -1;
	}
}
