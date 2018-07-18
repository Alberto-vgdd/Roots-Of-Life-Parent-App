using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    // Used to make static references to instance
    public static ScreenManager main;

    // Inspector input
    public ScreenBehaviour settingsScreen;
    public ScreenBehaviour introScreen;
    public ScreenBehaviour tutorialScreen;
    public ScreenBehaviour addUserScreen;
    public ScreenBehaviour loginScreen;
    public ScreenBehaviour quizScreen;

    public GameObject popupField;

    // List to store screens and easily access them
    private List<ScreenBehaviour> screens;

	// Use this for initialization
	void Start () {
        main = this;

        screens = new List<ScreenBehaviour>();
        screens.Add(settingsScreen);
        screens.Add(introScreen);
        screens.Add(tutorialScreen);
        screens.Add(addUserScreen);
        screens.Add(loginScreen);
        screens.Add(quizScreen);
    }

    // Instance calls to static function (used to call methods from events)
    public void iShowScreen(string screen) { showScreen(screen); }
    public void iToggleScreen(string screen) { toggleScreen(screen); }
    public void iClearScreen(string screen) { toggleScreen(screen); }

    // Set screen specified by string
    public static void showScreen(string screen)
    {
        setScreen(convertString(screen));
    }

    // Toggle screen on or off
    public static void toggleScreen(string screen)
    {
        int i = convertString(screen);
        if (i == -1)
            return;

        // If the screen is a quiz, shorten int to one digit
        if (i >= 500 && i <= 599)
            i = 5;

        // If screen is active, disable
        if (main.screens[i].gameObject.activeSelf)
            main.screens[i].gameObject.SetActive(false);

        // If screen is not active and it is a quiz, activate it
        else
            setScreen(i);
    }

    // Remove all screens and show the main menu
    public static void clearScreen()
    {
        foreach (ScreenBehaviour s in main.screens)
            s.gameObject.SetActive(false);
    }

    // Set screen specified by index, and set quiz number if necessary
    private static void setScreen(int i)
    {
        // Make sure index is within bounds
        if (i <= -1)
            return;

        int quiz = -1;
        if (i >= 500 && i <= 599)
        {
            quiz = (i - 500);
            i = 5;

            Debug.Log("i: " + i + ", quiz: " + quiz);
        }

        // Clear screen
        clearScreen();

        // Enable new screen
        main.screens[i].gameObject.SetActive(true);

        // Call special function if screen needs execution on show
        if (main.screens[i] is ScreenBehaviourExecute)
            ((ScreenBehaviourExecute)main.screens[i]).OnShow();
    }

    // Convert a string input for a screen into the corresponding index number
    private static int convertString(string screen)
    {
        if (screen == "settings")
            return 0;
        if (screen == "intro")
            return 1;
        if (screen == "tutorial")
            return 2;
        if (screen == "addUser")
            return 3;
        if (screen == "login")
            return 4;

        // Quiz screen also requires input for what quiz has to be opened, which is given as a double digit int after the string.
        // Example: quiz01, quiz69, quiz55
        if (screen.StartsWith("quiz"))
        {
            // Make sure a two digit number is given after the string
            if (screen.Length == 4 || screen.Length > 6)
                return -1;

            // Store digits behind the string
            int extra = int.Parse(screen.Substring(4));

            // Return quiz number after screen number. The screen number for quizzes is 5
            // This means the output will be: 501, 569, 555
            return extra + 500;
        }

        // No screen found
        return -1;
    }

    // Create a popup message to the user that will display for 3 seconds
    public static void Popup(string popupMessage)
    {
        main.StartCoroutine(popup(popupMessage));
    }

    // Display message to the user for 3 seconds
    static IEnumerator popup(string popup)
    {
        main.popupField.GetComponentInChildren<Text>().text = popup;

        main.popupField.SetActive(true);
        yield return new WaitForSeconds(3);
        main.popupField.SetActive(false);
        main.popupField.GetComponentInChildren<Text>().text = "Popup Text";
    }
}
