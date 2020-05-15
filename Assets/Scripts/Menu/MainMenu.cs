using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void play() {

        print("The play button has been pressed");

    }

    public void settings() {

        print("The settings button has been pressed.");
        SceneManager.LoadScene("mainSettings", LoadSceneMode.Single);

    }

    public void quit() {

        print("The quit button has been pressed.");
        Application.Quit();

    }

}
