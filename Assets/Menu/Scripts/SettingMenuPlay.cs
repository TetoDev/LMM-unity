using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingMenuPlay : MonoBehaviour
{
    public TMP_InputField seedInput;
    public SaveAndLoad saveAndLoad;

    public void PlayGame(){
        if (seedInput.text != ""){
            Debug.Log(int.Parse(seedInput.text));
            saveAndLoad.SaveSeed(int.Parse(seedInput.text));
        }else{
            int seed = Random.Range(-100000, 100000);
            saveAndLoad.SaveSeed(seed);
        }
        SceneManager.LoadScene("SceneGame");
        Debug.Log("Play");
    }
}
