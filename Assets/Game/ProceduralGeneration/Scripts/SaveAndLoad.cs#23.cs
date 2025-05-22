using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Globalization;
using System.Threading.Tasks;

public class SaveAndLoad : MonoBehaviour
{
    string filePathTerrainData => Application.persistentDataPath + "terrain.csv";
    string filePathSeep => Application.persistentDataPath + "seed.csv";
    string filePathRecord => Application.persistentDataPath + "records.csv";

    void Start(){
        InitFile("TerrainData/terrain", filePathTerrainData);
        InitFile("TerrainData/seed", filePathSeep);
        InitFile("TerrainData/records", filePathRecord);
        
    }

    private void InitFile(string resourcePath, string persistantPath)
    {
        TextAsset mapData = Resources.Load<TextAsset>(resourcePath);

        if (!File.Exists(persistantPath))
        {
            File.WriteAllText(persistantPath, mapData.text);
            Debug.Log($"File copy at : {persistantPath}");
        }
    }


    private void CheckPathOk(string path){
        if (path == ""){
            Debug.Log("ERROR (from-SaveAndLoad.cs) : couldn't find the path with UnityEditor");
        }
    }

    private List<string> GetFileContent(string path){

        List<string> lines = new List<string>{};
        if (File.Exists(path))
        {
            StreamReader reader = new StreamReader(path);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
            reader.Close();
        }
        return lines;
    }

    private List<string> GetBiomeContent(string biomeName){
        List<string> lines = GetFileContent(filePathTerrainData);
        List<string> biomeLines = new List<string>{};

        bool inBiome = false;
        foreach (string line in lines){
            
            if (line.Contains("#" + biomeName + "#")){
                inBiome = true;
                continue;
            } else if(line.Contains("#")){
                inBiome = false;
            }
            
            if (inBiome){
                biomeLines.Add(line);
            }
        }
        return biomeLines;
    }

    /*
    private List<string> BiomeToStr(TBiome biome)
    {
        string FindTilePath(string rootFolder, string tileName)
        {
            // Cherche tous les fichiers dans rootFolder et sous-dossiers
            var files = Directory.GetFiles(rootFolder, "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                // Prend le nom de fichier sans chemin ni extension
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);

                if (fileNameWithoutExt == tileName)
                {
                    return file; // chemin complet du fichier trouvé
                }
            }
            return null; // pas trouvé
        }

        List<string> lines = new List<string> { };
        lines.Add($"#{biome.name}#");
        lines.Add($"{biome.mapMaxHeight},{biome.noiseScale},{biome.probaDecor},{biome.probaGrass},{biome.probaTree}");

        lines.Add("@terrain");
        for (int i = 0; i < biome.map.Count; i++)
        {
            lstDisplayElements element = biome.map[i];
            Grid grid = element.tilemap.layoutGrid;
            string gridName = grid.gameObject.name;
            lines.Add($"{element.name},(,{gridName}");

            for (int j = 0; j < element.lstOfThisElementType.Count; j++)
            {
                displayElement subElement = element.lstOfThisElementType[j];
                string path = AssetDatabase.GetAssetPath(subElement.tile).Replace("Assets/Resources/", "").Replace(".asset", "");
                CheckPathOk(path);
                lines.Add($"{subElement.name},{path},{subElement.minHeight},{subElement.maxHeight}");
            }
            lines.Add(")");
        }
        lines.Add("@endterrain");

        lines.Add("@parallax");
        foreach (TParallaxBackground bg in biome.lstParallaxBackground)
        {
            string texturePath = AssetDatabase.GetAssetPath(bg.texture).Replace("Assets/Resources/", "").Split('.')[0];
            lines.Add($"{bg.name},{texturePath},{bg.speed.ToString(CultureInfo.InvariantCulture)}");
        }
        lines.Add("@endparallax");

        lines.Add("@structure");
        foreach (Tstructures structure in biome.lstStructures)
        {
            string path = AssetDatabase.GetAssetPath(structure.prefab).Replace("Assets/Resources/", "").Split('.')[0];
            lines.Add($"{structure.name},{structure.spawnCord},{path},{structure.length}");
        }
        lines.Add("@endstructure");


        return lines;
    }
    */
    public List<string> LstBiome(){
        List<string> lines = GetFileContent(filePathTerrainData);
        List<string> lstBiome = new List<string>{};
        foreach (string line in lines){
            if (line.Contains('#')){
                lstBiome.Add(line.Split('#')[1]);
            }
        }
        return lstBiome;
    }

    /*
    public void Save(TBiome biomeToSave)
    {
        List<string> fileContent = new List<string> { };
        foreach (string biomeName in LstBiome())
        {
            if (biomeName != biomeToSave.name)
            {
                fileContent.AddRange(BiomeToStr(Load(biomeName)));
            }
        }
        fileContent.AddRange(BiomeToStr(biomeToSave));

        Debug.Log(filePathTerrainData);
        File.WriteAllLines(filePathTerrainData, fileContent);
        Debug.Log("file created at : " + filePathTerrainData);
    }

    public void Delete(string biomeNameToDelete)
    {
        List<string> fileContent = new List<string>{};
        foreach (string biomeName in LstBiome()){
            if (biomeName != biomeNameToDelete){
                fileContent.AddRange(BiomeToStr(Load(biomeName)));
                }
        }
        File.WriteAllLines(filePathTerrainData, fileContent);
    }
    */
    public Dictionary<string, int> BuildDicoIdTerrain(string biomeName){

		Dictionary<string, int> IdTerrainTiles = new Dictionary<string, int>{};

        List<string> strBiome = GetBiomeContent(biomeName);
        bool inDisplayElement = false;
        bool inTerrain = false;
        string line = strBiome[0];
        bool endElement = line.Contains(')');
        string[] lineOfWord = line.Split(',');
        int tileCounter = 0;
        
        for (int i = 1 ; i <  strBiome.Count ; i++){ 
            
            line = strBiome[i];
            endElement = line.Contains(')');
            lineOfWord = line.Split(',');
            if (!inDisplayElement && inTerrain){
                IdTerrainTiles[lineOfWord[0]] = tileCounter;
                tileCounter += 1;
            } 
            if (! endElement && lineOfWord[0] == "terrain"){
                inTerrain = true;
            }
            if (endElement && inTerrain) {
                inTerrain = false;
            }
        }
        return IdTerrainTiles;

    }

    private void LoadTerrain(string biomeName, TBiome biome){
        biome.map = new List<lstDisplayElements>{};
        biome.name = biomeName;

        List<string> strBiome = GetBiomeContent(biome.name);
        bool inDisplayElement = false;
        lstDisplayElements tempLstDisplayElements = new lstDisplayElements();
        displayElement tempDisplayElement = new displayElement();
        string line = strBiome[0];
        bool endElement = line.Contains(')');
        bool inTerrainSection = false;
        string[] lineOfWord = line.Split(',');
        biome.mapMaxHeight = int.Parse(lineOfWord[0]);
        biome.noiseScale = int.Parse(lineOfWord[1]);
        biome.probaDecor = int.Parse(lineOfWord[2]);
        biome.probaGrass = int.Parse(lineOfWord[3]);
        biome.probaTree = int.Parse(lineOfWord[4]);

        for (int i = 1 ; i <  strBiome.Count ; i++){
            line = strBiome[i];

            if (line == "@terrain")
            {
                inTerrainSection = true;
                continue;
            }

            if (line == "@endterrain")
            {
                inTerrainSection = false;
                continue;
            }

            if (inTerrainSection){
                endElement = line.Contains(')');
                lineOfWord = line.Split(',');
                if (!inDisplayElement){
                    tempLstDisplayElements = new lstDisplayElements();
                    tempLstDisplayElements.name = lineOfWord[0];
                    GameObject obj = GameObject.Find(lineOfWord[2]);
                    tempLstDisplayElements.tilemap = obj.GetComponentsInChildren<Tilemap>()[0];
                    tempLstDisplayElements.lstOfThisElementType = new List<displayElement>{};
                } else if (! endElement){
                    tempDisplayElement = new displayElement();
                    tempDisplayElement.name = lineOfWord[0];
                    
                    tempDisplayElement.tile = Resources.Load<TileBase>(lineOfWord[1]);
                    tempDisplayElement.minHeight = int.Parse(lineOfWord[2]);
                    tempDisplayElement.maxHeight = int.Parse(lineOfWord[3]);
                    tempLstDisplayElements.lstOfThisElementType.Add(tempDisplayElement);
                }

                if (line.Contains('(')){inDisplayElement = true;
                } else if (endElement){inDisplayElement = false;
                biome.map.Add(tempLstDisplayElements);
                }
            }
            
        }
    }

    private void LoadParallax(string biomeName, TBiome biome)
    {
        biome.lstParallaxBackground = new List<TParallaxBackground>();
        
        List<string> strBiome = GetBiomeContent(biomeName);
        bool inParallaxSection = false;
        string line = strBiome[0];

        for (int i = 1; i < strBiome.Count; i++)
        {
            line = strBiome[i];

            if (line == "@parallax")
            {
                inParallaxSection = true;
                continue;
            }

            if (line == "@endparallax")
            {
                inParallaxSection = false;
                continue;
            }

            if (inParallaxSection)
            {
                string[] lineOfWord = line.Split(',');
                string name = lineOfWord[0];
                string path = lineOfWord[1];
                float speed = float.Parse(lineOfWord[2], CultureInfo.InvariantCulture);
                Texture2D texture = Resources.Load<Texture2D>(path);
                TParallaxBackground parallax = new TParallaxBackground{};
                parallax.name = name;
                parallax.texture = texture;
                parallax.speed = speed;
                biome.lstParallaxBackground.Add(parallax);
            }
        }
    }


    private void LoadStructures(string biomeName, TBiome biome){
        biome.lstStructures = new List<Tstructures>{};

        List<string> strBiome = GetBiomeContent(biomeName);
        bool inStructureSection = false;
        string line = strBiome[0];

        for (int i = 1; i < strBiome.Count; i++)
        {
            line = strBiome[i];

            if (line == "@structure")
            {
                inStructureSection = true;
                continue;
            }

            if (line == "@endstructure")
            {
                inStructureSection = false;
                continue;
            }

            if (inStructureSection)
            {
                string[] lineOfWord = line.Split(',');
                Tstructures tempStructure = new Tstructures();
                tempStructure.name = lineOfWord[0];
                tempStructure.spawnCord = int.Parse(lineOfWord[1]);
                tempStructure.prefab = Resources.Load<GameObject>(lineOfWord[2]);
                tempStructure.length = int.Parse(lineOfWord[3]);

                biome.lstStructures.Add(tempStructure);
            }
        }
    }

    public TBiome Load(string biomeName){
        TBiome biome = new TBiome();
        LoadTerrain(biomeName, biome);
        LoadStructures(biomeName, biome);
        LoadParallax(biomeName, biome);
        return biome;

    }

    public void SaveSeed(int seed){
        File.WriteAllLines(filePathSeep, new List<string> {$"{seed}"} );
    }

    public int GetSeed(){
        return int.Parse(GetFileContent(filePathSeep)[0].Split(',')[0]);
    }

    private string ConvertTimeToUniversalTime(float bestRecord){
        string bestRecordText = "";
        float nbHours = 0f;
        float nbMinutes = 0f;
        float nbSeconds = 0f;
         // Convert the best record in s to a format 1h 3 mins 5.25s
        if (bestRecord / 3600f >= 1f)
        {
            nbHours = Mathf.Floor(bestRecord / 3600f);
            bestRecordText += $"{nbHours}h ";
        }

        if ((bestRecord - nbHours * 3600f) / 60f >= 1f)
        {
            nbMinutes = Mathf.Floor((bestRecord - nbHours * 3600f) / 60f);
            bestRecordText += $"{nbMinutes}min ";
        }

        nbSeconds = bestRecord - nbHours * 3600f - nbMinutes * 60f;
        if (nbSeconds > 0f || bestRecordText == "")
        {
            bestRecordText += $"{nbSeconds:F2}s";
        }
        return bestRecordText;

    }

    public void SaveRecord(float recordInS){
        List<string> listRecords = GetFileContent(filePathRecord);
        listRecords.Add(recordInS.ToString(CultureInfo.InvariantCulture));
        File.WriteAllLines(filePathRecord, listRecords);
    }

    public string GetLastTime(){
        List<string> listRecords = GetFileContent(filePathRecord);
        return ConvertTimeToUniversalTime(float.Parse(listRecords[listRecords.Count - 1], CultureInfo.InvariantCulture));
    }

    public string GetBestRecord(){
        List<string> listRecords = GetFileContent(filePathRecord);
        // We don't want the first line
        listRecords = listRecords.GetRange(1, listRecords.Count - 1);

        if (listRecords.Count > 0){
                float bestRecord = float.MaxValue;

            foreach(string record in listRecords){
                float tempRecord = float.Parse(record, CultureInfo.InvariantCulture);
                if (tempRecord < bestRecord){
                    bestRecord = tempRecord;
                }
            }           
            return ConvertTimeToUniversalTime(bestRecord);
        } else {
            return "Play to set a record";
        }
        
    }

}
