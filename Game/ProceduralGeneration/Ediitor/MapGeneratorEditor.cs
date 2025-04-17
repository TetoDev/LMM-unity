using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MapDisplay))]
public class MapGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		MapDisplay mapDisplay = (MapDisplay)target;
		SaveAndLoad saveAndLoad = mapDisplay.GetComponent<SaveAndLoad>();
		
		if (DrawDefaultInspector ()) {
			if (mapDisplay.autoUpdate) {
				mapDisplay.GenerateMap ();
			}
		}

		if (GUILayout.Button ("Generate")) {
			mapDisplay.GenerateMap ();
		}

		if (GUILayout.Button ("Save")) {
			mapDisplay.saveAndLoad.Save (mapDisplay.biome);
		}
		
		foreach (string biomeName in mapDisplay.saveAndLoad.LstBiome()){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(biomeName);

			if (GUILayout.Button ("Load")) {
				mapDisplay.biome = mapDisplay.saveAndLoad.Load(biomeName);
				Debug.Log("File loaded");
			} else if (GUILayout.Button ("Delete")){
				bool confirm = EditorUtility.DisplayDialog(
                "Confirmation", 
                "Do you really want to delete this biome ???", 
                "Yes", "No"
				);

				if (confirm)
				{
					mapDisplay.saveAndLoad.Delete(biomeName);
					Debug.Log("Biome deleted");
				}
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}