using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (gameIsPaused){
                Resume();
            }
            else{
                Paused();
            }
        }
    }

    void Paused(){
        // display pause menu
        pauseMenuUI.SetActive(true);

        // stop the game
        Time.timeScale = 0;

        // change game status
        gameIsPaused = true;
    }

    public void Resume(){
        // display pause menu
        pauseMenuUI.SetActive(false);

        // stop the game
        Time.timeScale = 1;

        // change game status
        gameIsPaused = false;
    }

    public void BackMenu(){
        Resume();
        SceneManager.LoadScene("SceneMenu");
    }

}
