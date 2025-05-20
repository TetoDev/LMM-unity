using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Text recordText;
    public SaveAndLoad saveAndLoad;

    public GameObject settingWindow;
    public GameObject playWindow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneManager.LoadScene("SceneTerrainMenu", LoadSceneMode.Additive);
        recordText.text = saveAndLoad.GetBestRecord();
    }

    public void PlayGame(){
        playWindow.SetActive(true);
    }

    public void ShopGame(){
        Debug.Log("Nothing Now");
    }

    public void SettingsButton(){
        settingWindow.SetActive(true);
    }

    public void CloseSettings(){
        settingWindow.SetActive(false);
    }

    public void ClosePlay(){
        playWindow.SetActive(false);
    }

    public void QuitGame(){
        Application.Quit();
    }
    
}
