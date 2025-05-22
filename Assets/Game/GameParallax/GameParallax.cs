using UnityEngine;

public class GameParallax : MonoBehaviour
{
    public MapDisplay mapDisplay;
    public GameObject target;
    float offset = 1;
    float speed;
    float previousPlayerPos;
    int layer;
    Texture2D currentText;

    private void LoadParallaxBackground(){
        // if the current layer have a texture save for him
        currentText = mapDisplay.biome.lstParallaxBackground[layer - 1].texture;
        GetComponent<Renderer>().material.SetTexture("_MainTex", currentText);
        speed = mapDisplay.biome.lstParallaxBackground[layer - 1].speed;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        layer = int.Parse(gameObject.name);
         if (layer <= mapDisplay.biome.lstParallaxBackground.Count){
            LoadParallaxBackground();
        }else{
            GetComponent<Renderer>().enabled = false;
            speed = 0;
        }

        if (target != null){
            previousPlayerPos = target.transform.localPosition.x;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        bool mouveParallaxBg = false;
        layer = int.Parse(gameObject.name);
        // if there is a biome change we need to change the texture
        if (layer <= mapDisplay.biome.lstParallaxBackground.Count){
            if(currentText != mapDisplay.biome.lstParallaxBackground[layer - 1].texture){
                LoadParallaxBackground();
            }
        }else{
            GetComponent<Renderer>().enabled = false;
            speed = 0;
        }
        
        // if the player is really mouving or if his game object is null
        if (target != null){
            if (previousPlayerPos != target.transform.localPosition.x){
                mouveParallaxBg = true;
            }
        } else {
            mouveParallaxBg = true;
        }
        
        if (mouveParallaxBg && target != null){
            offset += Input.GetAxis("Horizontal") * speed; // return a value [0, 1] to have a smoth mouvement
            GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
            previousPlayerPos = target.transform.localPosition.x;
        }
    }
}
