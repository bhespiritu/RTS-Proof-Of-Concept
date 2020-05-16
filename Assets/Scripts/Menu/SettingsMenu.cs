// Author: Kevin Mulliss (kam8ef@virginia.edu)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// event lisetner for the main settings menu
public class SettingsMenu : MonoBehaviour {

    // method that runs when the back button is pressed
    public void back() {

        // placeholder print to show the button has been pressed
        print("The back button has been pressed");
        // load the main menu Unity scene
        SceneManager.LoadScene("mainMenu", LoadSceneMode.Single);        

    }

    // TODO: Add Settings :)

}
