using UnityEngine;

public class GameParallax : MonoBehaviour
{
    public MapDisplay mapDisplay;
    float offset = 1;
    float speed;
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
    }

    // Update is called once per frame
    public void Update()
    {
        offset += Input.GetAxis("Horizontal") * speed;
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
