using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Text recordText;
    public SaveAndLoad saveAndLoad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneManager.LoadScene("SceneTerrainMenu", LoadSceneMode.Additive);
        recordText.text = saveAndLoad.GetBestRecord();
    }

    public void PlayGame(){
        SceneManager.LoadScene("SceneMenuPlay");
        Debug.Log("Play");
    }

    public void ShopGame(){
        Debug.Log("Nothing Now");
    }

    public void QuitGame(){
        Application.Quit();
    }
    
}
