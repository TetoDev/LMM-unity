using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndMenuManager : MonoBehaviour
{
    public TMP_Text timeText;
    public SaveAndLoad saveAndLoad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeText.text = saveAndLoad.GetLastTime();
    }

    public void BackMenu(){
        SceneManager.LoadScene("SceneMenu");
    }

}
