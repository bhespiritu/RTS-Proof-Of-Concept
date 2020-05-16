// Author: Kevin Mulliss (kam8ef@virginia.edu)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Event Listener code for the main menu
public class MainMenu : MonoBehaviour {

    // method that is called when the main menu play button is pressed
    public void play() {

        // placeholder method to show that the play button has been pressed
        print("The play button has been pressed");

    }

    // method that is called when the main menu setting button is pressed
    public void settings() {

        // placeholder method to show that the settings button has been pressed
        print("The settings button has been pressed.");
        // loads the unity scene to display the settings menu
        SceneManager.LoadScene("mainSettings", LoadSceneMode.Single);

    }

    // method that is called when the main menu play button is pressed
    public void quit() {

        // placeholder method to show that the settings button has been pressed
        print("The quit button has been pressed.");
        // quit (note: this does not work in the test environment in Unity)
        Application.Quit();

    }

}
