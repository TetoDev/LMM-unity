using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadScene("SceneGame");
        Debug.Log("Play");
    }

    public void ShopGame(){
        Debug.Log("Nothing Now");
    }

    public void QuitGame(){
        Application.Quit();
    }
    
}
