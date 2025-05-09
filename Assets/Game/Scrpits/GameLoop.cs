using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour

{
    public SaveAndLoad saveAndLoad;
    public MapDisplay mapDisplay;
    public WorldUnitConvertion convertionSysteme;
    public int nbBiomeOfAGame = 4;

    List<string> gameBiomeList;

    bool GameIsCompleted = false;
    int seed = 0;
    float gameTimer = 0;

    
    int biomeCounter = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameBiomeList = ShufflePseudoRandom(saveAndLoad.LstBiome(), seed);
        gameBiomeList = gameBiomeList.GetRange(0, Mathf.Min(nbBiomeOfAGame, gameBiomeList.Count));
        mapDisplay.seed = saveAndLoad.GetSeed();
        mapDisplay.biome = saveAndLoad.Load(gameBiomeList[biomeCounter]);
        biomeCounter += 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (mapDisplay.biome.isCompleted && biomeCounter <= gameBiomeList.Count - 1){
            mapDisplay.biome = saveAndLoad.Load(gameBiomeList[biomeCounter]);
            mapDisplay.GenerateMap();
            biomeCounter += 1;
        }

        if (biomeCounter == gameBiomeList.Count){
            GameIsCompleted = true;
        }

        if (GameIsCompleted){
            EndGame();
        } else{
            // compute the duration of the run 
            gameTimer += Time.deltaTime;
        }
    }

    // Shuffle the elements of a list of string 
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

    private void EndGame(){
        saveAndLoad.SaveRecord(gameTimer);
        SceneManager.LoadScene("SceneMenu");
    }
}
