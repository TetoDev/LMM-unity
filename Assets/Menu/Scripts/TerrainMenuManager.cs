using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;

public class TerrainMenuManager : MonoBehaviour
{

    public MapDisplay mapDispaly;
    public SaveAndLoad saveAndLoad;
    public int changeBackgroundTimer = 30; 
    
    float timer = 0f;
    System.Random rnd = new System.Random();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        mapDispaly.biome = saveAndLoad.Load(saveAndLoad.LstBiome()[rnd.Next(saveAndLoad.LstBiome().Count)]);
        mapDispaly.seed = rnd.Next(10000);
        mapDispaly.GenerateMap();

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > changeBackgroundTimer){
            timer = 0;
            mapDispaly.seed = rnd.Next(10000);
            mapDispaly.GenerateMap();
        }
    }
}
