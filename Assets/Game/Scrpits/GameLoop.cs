using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour

{
    public SaveAndLoad saveAndLoad;
    public MapDisplay mapDisplay;
    public WorldUnitConvertion convertionSysteme;
    public GameObject player;
    public GameObject manager;
    public int nbBiomeOfAGame = 4;
    public int playerOffset = 7;

    List<string> gameBiomeList;
    PlayerMovement scriptPlayer;

    bool GameIsCompleted = false;
    int seed = 0;
    float gameTimer = 0;
    int biomeCounter = 1;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // we place the player in the right place of the screen
        Vector3 tempPlayerPos = player.transform.position;
        player.transform.position = new Vector3(playerOffset, tempPlayerPos.y + mapDisplay.biome.mapMaxHeight, tempPlayerPos.z);
        
        scriptPlayer = player.GetComponent<PlayerMovement>();

        seed = saveAndLoad.GetSeed();
        mapDisplay.seed = seed;
        
        // we create the list of the different biome of the game using pseudo random and seep
        gameBiomeList = ShufflePseudoRandom(saveAndLoad.LstBiome(), seed);
        gameBiomeList = gameBiomeList.GetRange(0, Mathf.Min(nbBiomeOfAGame, gameBiomeList.Count));
        mapDisplay.biome = saveAndLoad.Load(gameBiomeList[biomeCounter - 1]);

        // we instantiate spwaning system
        GameObject newEnemy = Instantiate(mapDisplay.biome.spawnEnemisManager, new Vector3(0,0,0), Quaternion.identity, manager.transform);


    }

    // Update is called once per frame
    void Update()
    {
        // change the biome and place the player in the screen 
        if (mapDisplay.biome.isCompleted && biomeCounter <= gameBiomeList.Count){
            mapDisplay.biome = saveAndLoad.Load(gameBiomeList[biomeCounter - 1]);

            Vector3 tempPlayerPos = player.transform.position;
            player.transform.position = new Vector3(playerOffset, tempPlayerPos.y + mapDisplay.biome.mapMaxHeight, tempPlayerPos.z);
            biomeCounter += 1;
        }

        if (biomeCounter == gameBiomeList.Count + 1){
            GameIsCompleted = true;
        }

        if (GameIsCompleted || scriptPlayer.GetHealth() <= 0){
            EndGame();
        }else{
            // compute the duration of the run 
            gameTimer += Time.deltaTime;
        }
    }

    // direct the user in the appropriate scene
    private void EndGame(){
        if(scriptPlayer.GetHealth() > 0){
            saveAndLoad.SaveRecord(gameTimer);
            SceneManager.LoadScene("SceneMenuWin");
        } else{
            SceneManager.LoadScene("SceneMenuLoose");
        }
        
    }
}
