using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour

{
    public SaveAndLoad saveAndLoad;
    public MapDisplay mapDisplay;
    public WorldUnitConvertion convertionSysteme;
    public GameObject player;
    public int nbBiomeOfAGame = 4;
    public int playerOffset = 7;

    List<string> gameBiomeList;

    bool GameIsCompleted = false;
    int seed = 0;
    float gameTimer = 0;
    int biomeCounter = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 tempPlayerPos = player.transform.position;
        player.transform.position = new Vector3(playerOffset, tempPlayerPos.y + mapDisplay.biome.mapMaxHeight, tempPlayerPos.z);
        
        seed = saveAndLoad.GetSeed();
        mapDisplay.seed = seed;
        
        gameBiomeList = ShufflePseudoRandom(saveAndLoad.LstBiome(), seed);
        gameBiomeList = gameBiomeList.GetRange(0, Mathf.Min(nbBiomeOfAGame, gameBiomeList.Count));
        mapDisplay.biome = saveAndLoad.Load(gameBiomeList[biomeCounter - 1]);
        biomeCounter += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (mapDisplay.biome.isCompleted && biomeCounter <= gameBiomeList.Count){
            mapDisplay.biome = saveAndLoad.Load(gameBiomeList[biomeCounter - 1]);
            mapDisplay.GenerateMap();
            Vector3 tempPlayerPos = player.transform.position;
            player.transform.position = new Vector3(playerOffset, tempPlayerPos.y + mapDisplay.biome.mapMaxHeight, tempPlayerPos.z);
            biomeCounter += 1;
        }

        if (biomeCounter == gameBiomeList.Count + 1){
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
            Debug.Log(tempBiome);
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
