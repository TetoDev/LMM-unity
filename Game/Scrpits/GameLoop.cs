using UnityEngine;
using System.Collections.Generic;

public class GameLoop : MonoBehaviour

{
    public SaveAndLoad saveAndLoad;
    public MapDisplay mapDisplay;
    List<string> gameBiomeList;
    public int seed = 0;
    [Header("Warning less or equal the number of existing biome")]
    public int nbBiomeOfAGame = 4;
    int biomeCounter = 0;

    private List<string> ShufflePseudoRandom(List<string> lst, int seed){
        System.Random prng = new System.Random (seed);
        List<string> res = new List<string>();
        int nbBiome = lst.Count;
        string tempBiome;
        for(int i = 0 ; i < nbBiome ; i++){
            tempBiome = lst[prng.Next (0, lst.Count)];
            res.Add(tempBiome);
            lst.Remove(tempBiome);
        }
        return res;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameBiomeList = ShufflePseudoRandom(saveAndLoad.LstBiome(), seed);
        mapDisplay.seed = seed;
        mapDisplay.biome = saveAndLoad.Load(gameBiomeList[biomeCounter]);
        mapDisplay.GenerateMap();
        foreach(string b in gameBiomeList){
            Debug.Log(b);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mapDisplay.biome.isCompleted && biomeCounter <= gameBiomeList.Count - 1){
            biomeCounter += 1;
            mapDisplay.biome = saveAndLoad.Load(gameBiomeList[biomeCounter]);
            mapDisplay.GenerateMap();
        }
    }
}
