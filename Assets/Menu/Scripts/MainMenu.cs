using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Text recordText; // where the recor will be display
    public SaveAndLoad saveAndLoad; //access to the library saveAndLoad

    public GameObject settingWindow; //panel (ui element) with settings
    public GameObject playWindow; // panel (ui element) with options to play

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneManager.LoadScene("SceneTerrainMenu", LoadSceneMode.Additive);
        recordText.text = saveAndLoad.GetBestRecord();
    }

    public void PlayGame(){ // display the panel playWindow
        playWindow.SetActive(true);
    }

    public void ShopGame(){
        Debug.Log("Nothing Now");
    }

    public void SettingsButton(){ // display the panel setting window
        settingWindow.SetActive(true);
    }

    public void CloseSettings(){ // hide the panel setting
        settingWindow.SetActive(false);
    }

    public void ClosePlay(){ // hide the panel playWindow
        playWindow.SetActive(false);
    }

    public void QuitGame(){ // work only in build mode
        Application.Quit();
    }
    
}
