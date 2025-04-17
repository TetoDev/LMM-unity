using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InGameMenu : MonoBehaviour
{
    public Button PauseButton;
    public void BackMenu(){
        SceneManager.LoadScene("SceneMenu");
        Debug.Log("Play");
    }

    public void PauseGame(){
        Debug.Log("Nothing in here");
        if (EventSystem.current.currentSelectedGameObject == PauseButton.gameObject){
            //PauseButton.interactable = false;
            //PauseButton.interactable = true;
        } 
        
        
    }
    
}
