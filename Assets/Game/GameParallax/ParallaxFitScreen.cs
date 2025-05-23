using UnityEngine;

public class ParallaxFitScreen : MonoBehaviour
{

    private Vector2 lastScreenSize;

    void Start()
    {
        FitToScreen();
        lastScreenSize = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        // if the user change the screen size we modify the size of the background
        if (Screen.width != lastScreenSize.x || Screen.height != lastScreenSize.y)
        {
            FitToScreen();
            lastScreenSize = new Vector2(Screen.width, Screen.height);
        }
    }

    void FitToScreen()
    {
        // use of the camera to have the size of the window in world
        Camera cam = Camera.main;
        float heightInWorld = 8f * cam.orthographicSize;
        float widthInWorld = heightInWorld * cam.aspect;

        transform.localScale = new Vector3(
            widthInWorld,
            heightInWorld,
            transform.localScale.z
        );
    }
}
