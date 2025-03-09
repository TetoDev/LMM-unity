using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEditor;

public class SaveAndLoad : MonoBehaviour
{
    string filePath = "Assets/Game/ProceduralGeneration/TerrainData/terrain.csv";

    static void CheckPathOk(string path){
        if (path == ""){
            Debug.Log("ERROR (from-SaveAndLoad.cs) : couldn't find the path with UnityEditor");
        }
    }

    private List<string> GetFileContent(){
        List<string> lines = new List<string>{};
        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);
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
        List<string> lines = GetFileContent();
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

    private List<string> BiomeToStr(TBiome biome){
        List<string> lines = new List<string>{};
        lines.Add($"#{biome.name}#");
        lines.Add($"{biome.mapMaxHeight}, {biome.noiseScale}");
        for (int i = 0 ; i < biome.map.Count ; i++){
            lstDisplayElements element = biome.map[i];
            Grid grid = element.tilemap.layoutGrid;
            string gridName = grid.gameObject.name;
            lines.Add($"{element.name},(,{gridName}");

            for (int j = 0 ; j < element.lstOfThisElementType.Count ; j++){
                displayElement subElement = element.lstOfThisElementType[j];
                string path = AssetDatabase.GetAssetPath(subElement.tile);
                CheckPathOk(path);
                lines.Add($"{subElement.name},{path},{subElement.minHeight},{subElement.maxHeight}");
            }
            lines.Add(")");
        }
        return lines;
    }

    public List<string> LstBiome(){
        List<string> lines = GetFileContent();
        List<string> lstBiome = new List<string>{};
        foreach (string line in lines){
            if (line.Contains('#')){
                lstBiome.Add(line.Split('#')[1]);
            }
        }
        return lstBiome;
    }

    public void Save(TBiome biomeToSave)
    {
        List<string> fileContent = new List<string>{};
        foreach (string biomeName in LstBiome()){
            if (biomeName != biomeToSave.name){
                fileContent.AddRange(BiomeToStr(Load(biomeName)));
                }
        }
        fileContent.AddRange(BiomeToStr(biomeToSave));

        File.WriteAllLines(filePath, fileContent);
        Debug.Log("file created at : " + filePath);
    }

    public void Delete(string biomeNameToDelete)
    {
        List<string> fileContent = new List<string>{};
        foreach (string biomeName in LstBiome()){
            if (biomeName != biomeNameToDelete){
                fileContent.AddRange(BiomeToStr(Load(biomeName)));
                }
        }
        File.WriteAllLines(filePath, fileContent);
    }
    public List<string> BuildDicoIdTerrain(string biomeName){
        List<string> IdTerrainTiles = new List<string>{};
        List<string> strBiome = GetBiomeContent(biomeName);
        bool inDisplayElement = false;
        bool inTerrain = false;
        string line = strBiome[0];
        bool endElement = line.Contains(')');
        string[] lineOfWord = line.Split(',');
        
        for (int i = 1 ; i <  strBiome.Count ; i++){
            
            line = strBiome[i];
            endElement = line.Contains(')');
            lineOfWord = line.Split(',');
            if (!inDisplayElement && inTerrain){
                IdTerrainTiles.Add(lineOfWord[0]);
                Debug.Log(" --> " + lineOfWord[0]);
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

    public TBiome Load(string biomeName){
        TBiome biome = new TBiome();
        biome.map = new List<lstDisplayElements>{};
        biome.name = biomeName;

        List<string> strBiome = GetBiomeContent(biome.name);
        bool inDisplayElement = false;
        lstDisplayElements tempLstDisplayElements = new lstDisplayElements();
        displayElement tempDisplayElement = new displayElement();
        string line = strBiome[0];
        bool endElement = line.Contains(')');
        string[] lineOfWord = line.Split(',');
        biome.noiseScale = int.Parse(lineOfWord[1]);
        biome.mapMaxHeight = int.Parse(lineOfWord[0]);

        for (int i = 1 ; i <  strBiome.Count ; i++){
            
            line = strBiome[i];
            endElement = line.Contains(')');
            lineOfWord = line.Split(',');
            if (!inDisplayElement){
                tempLstDisplayElements = new lstDisplayElements();
                tempLstDisplayElements.name = lineOfWord[0];
                GameObject obj = GameObject.Find(lineOfWord[2]);
                tempLstDisplayElements.tilemap = obj.GetComponentsInChildren<Tilemap>()[0];
                Debug.Log(tempLstDisplayElements.tilemap.gameObject.name);
                tempLstDisplayElements.lstOfThisElementType = new List<displayElement>{};
            } else if (! endElement){
                tempDisplayElement = new displayElement();
                tempDisplayElement.name = lineOfWord[0];
                tempDisplayElement.tile = AssetDatabase.LoadAssetAtPath<TileBase>(lineOfWord[1]);
                Debug.Log(tempDisplayElement.tile);
                tempDisplayElement.minHeight = int.Parse(lineOfWord[2]);
                tempDisplayElement.maxHeight = int.Parse(lineOfWord[3]);
                tempLstDisplayElements.lstOfThisElementType.Add(tempDisplayElement);
            }

            if (line.Contains('(')){inDisplayElement = true;
            } else if (endElement){inDisplayElement = false;
            biome.map.Add(tempLstDisplayElements);
            }
        }
        return biome;

    }

}
