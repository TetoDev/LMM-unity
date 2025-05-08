using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LunchGame : MonoBehaviour
{
    public TMP_InputField seedInput;
    public SaveAndLoad saveAndLoad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneManager.LoadScene("SceneTerrainMenu", LoadSceneMode.Additive);
    }

     public void PlayGame(){
        if (seedInput.text != ""){
            Debug.Log(int.Parse(seedInput.text));
            saveAndLoad.SaveSeed(int.Parse(seedInput.text));
        }
        SceneManager.LoadScene("SceneGame");
        Debug.Log("Play");
    }
}
