using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour {

    public void back() {

        print("The back button has been pressed");
        SceneManager.LoadScene("mainMenu", LoadSceneMode.Single);        

    }

}
