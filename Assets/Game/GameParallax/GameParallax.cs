using UnityEngine;

public class GameParallax : MonoBehaviour
{
    public MapDisplay mapDisplay;
    public GameObject target;
    float offset = 1;
    float speed;
    float previousPlayerPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        // if the current layer have a texture save for him
        int layer = int.Parse(gameObject.name);
        if (layer <= mapDisplay.biome.lstParallaxBackground.Count){
            GetComponent<Renderer>().material.SetTexture("_MainTex", mapDisplay.biome.lstParallaxBackground[layer - 1].texture);
            speed = mapDisplay.biome.lstParallaxBackground[layer - 1].speed;
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
        // if the player is really mouving or if his game object is null
        if (target != null){
            if (previousPlayerPos != target.transform.localPosition.x){
                mouveParallaxBg = true;
            }
        } else {
            mouveParallaxBg = true;
        }
        
        if (mouveParallaxBg){
            offset += Input.GetAxis("Horizontal") * speed;
            GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
            previousPlayerPos = target.transform.localPosition.x;
        }
    }
}
