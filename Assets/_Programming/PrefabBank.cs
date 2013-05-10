using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabBank : MonoBehaviour{
	private static Dictionary<string, Buildable> wallBank;
	private static Dictionary<string, Buildable> floorBank;
	private static Dictionary<string, Buildable> blockBank;
	
	//prefab bank will point to different banks all when requested;
	private static Dictionary<string, Buildable> prefabBank;
	
	
	public Buildable[] floorTiles;
	public Buildable[] wallModels;
	public Buildable[] blocks;

	private int idCount = 0;
	
	private void Awake(){
		wallBank = new Dictionary<string, Buildable>();
		floorBank = new Dictionary<string, Buildable>();
		blockBank = new Dictionary<string, Buildable>();
		
		AddFloorTiles();
		AddWallModels();
		AddBlocks();
	}
	private void AddWallModels(){
		for(int i = 0; i < wallModels.Length;i++){
			Buildable wall = wallModels[i];
			AssignId(wall);	
			if(wall.layer != CellLayer.Wall)Debug.LogError(wall + " is not a Wall layer object");
			if(wall != null){
				wallBank.Add(wall.name, wall);
			}
		}
	}
	private void AddFloorTiles(){
		for(int i = 0; i < floorTiles.Length;i++){
			Buildable tile = floorTiles[i];
			AssignId(tile);
			if(tile.layer != CellLayer.Floor)Debug.LogError(tile + " is not a Floor layer object");
			if(tile != null){
				floorBank.Add(tile.name, tile);
			}
		}
	}
	private void AddBlocks(){
		for(int i = 0; i < blocks.Length;i++){
			Buildable block = blocks[i];
			AssignId(block);
			if(block.layer != CellLayer.Block)Debug.LogError(block + " is not a Block layer object");
			if(block != null){
				blockBank.Add(block.name, block);
			}
		}
	}
	private void AssignId(Buildable b){
		b.buildableId = idCount;
		idCount ++;
	}
	public static Buildable GetBuildable(string value, CellLayer forLayer){
		PrefabBank.SelectPrefabBank(forLayer);
		return prefabBank[value];
	}
	private static void SelectPrefabBank(CellLayer forLayer){
		//select the prefab bank the player wants to pick from
		switch(forLayer){
			case CellLayer.Floor:
				prefabBank = floorBank;
				break;
			case CellLayer.Wall:
				prefabBank = wallBank;
				break;
			case CellLayer.Block:
				prefabBank = blockBank;
				break;
		}
	}
	public static string[] GetKeys(CellLayer forLayer){
		PrefabBank.SelectPrefabBank(forLayer);
		
		string []prefabNames = new string[prefabBank.Count];
		int i = 0;
		foreach(KeyValuePair<string, Buildable> pair in prefabBank){
			prefabNames[i] = pair.Key;
			i++;
		}
		return prefabNames;
	}
}