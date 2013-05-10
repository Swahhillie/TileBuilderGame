using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CellManager : MonoBehaviour{
	//used for int array 0 and 1. meaning X and Y, to specify a coord in the array
	const int X = 0;
	const int Y = 1;
	
	public enum CellManagerState{
		NoTilesGenerated,
		GeneratingTiles,
		Ready
	}
	private CellManagerState _state = CellManagerState.NoTilesGenerated;
	private int _width = 20;
	private int _height = 30;
	
	private Tile[,] _tiles;
	
	private static CellManager manager;
	public int spawnsPerFrame = 30;
	public delegate void OnGenerateLevel ();
	
	private static List<OnGenerateLevel> onGenerateLevelFunctions = new List<OnGenerateLevel>();
	public GUITexture tempLoadingbar;
	
	public void Start(){
		Debug.Log("First Generate Tiles call");
		GenerateTiles();
	}
	//--------------------------------Creation and setup of tiles-------------------------------------
	
	public void GenerateTiles(){
		//generate new set of tiles based on the current settings of the tile generator
		CleanupTiles();
		GenerateTiles(_width, _height);
		
		
	}
	public void GenerateTiles(int width, int height){
		//guard
		if(_state == CellManagerState.GeneratingTiles){
			Debug.LogError("tiles are still being generated");
		}
		//cleanup old tiles. change the settings and call the default generator
		
		//this is also the only way to change the width and height of the array. If it was changed anywhere else that will lead to problems.
		CleanupTiles();
		_width = width;
		_height = height;
		_tiles = new Tile[_width, _height];
		StartCoroutine(GenerateTilesReceiver());
	}
	private IEnumerator GenerateTilesReceiver(){
		//does the actual generation
		tempLoadingbar.enabled = true; //TEMP
		_state = CellManagerState.GeneratingTiles;
		int total = _height * _width;
		for(int i = 0; i < _height; i++){
			for(int j = 0; j < _width; j++){
				_tiles[j,i] = CreateTile(j,i);
				int done = (i * _width + j);
				if( done % spawnsPerFrame == 0){
					float completion = (float)done / (float)total ;
					tempLoadingbar.pixelInset = new Rect(-32, -32, 64 * completion, 64);					//TEMPORARY MAGIC, THIS SHOULDNT BE HERE
					Debug.Log( "progress " + done + " / " + total + ", " + completion * 100.0f + "%");
					yield return null;
					
				}
			}
		}
		tempLoadingbar.enabled = false;	//TEMP	
		
		SetAllNeighbours();
		SetAllWalls();
		foreach(OnGenerateLevel func in onGenerateLevelFunctions) func();
		_state = CellManagerState.Ready;
	}
	private void OnApplicationQuit(){
		GenerateTiles(1,1);
	}
	private Tile CreateTile(int i, int j){
		//do stuff to create a tile here that affect multiple tiles
		Tile tile = new Tile(i, j);
		return tile;
	}
	private void CleanupTiles(){
		//cleanup existing tiles and give a fresh array
		
		if(_tiles != null){
			//if width and height are changed this will fail
			
			for(int i = tiles.GetLowerBound(0) ; i <= tiles.GetUpperBound(0) ; i ++)
				for(int j = tiles.GetLowerBound(1); j <= tiles.GetUpperBound(1); j++){
					_tiles[i,j].Cleanup();
					_tiles[i,j] = null;
				}
		}
		
	}
	private void SetAllNeighbours(){
		foreach(Tile tile in tiles) SetNeighbours(tile);
	}
	private void SetAllWalls(){
	//	System.Array.ForEach<Tile>(_tiles, x=>x.wallTile.InitializeWalls());
		
		for(int i = width -1; i >= 0; i--){
			for(int j = height -1; j>= 0; j--){
				_tiles[i,j].wallTile.InitializeWalls();
			}
		}
	}
	private void SetNeighbours(Tile tile){
	
		//links all neighbouring tiles to the the center tile.
		
		for(int i = 0; i < 9; i++){
			//make all combinations of -1, 0 and 1;
			/*
			[(-1,-1),(0,-1),(1,-1)]
			[(-1, 0),(0, 0),(1, 0)]
			[(-1, 1),(0, 1),(1, 1)]
			
			*/
			
			int checkX = i % 3 - 1 + tile.column;
			int checkY = (i / 3) % 3 - 1 + tile.row;
			
			if(checkX >= 0 && checkX < _width && checkY >= 0 && checkY < _height){
				tile.neighbours[i] = _tiles[checkX, checkY];	
			}
			else tile.neighbours[i] = null;
			
			
		}
	}
	//--------------------------------registering callback functions ---------
	public static void AddGenerateLevelCallback(OnGenerateLevel func){
		Debug.Log("added a function to call after level generation");
		onGenerateLevelFunctions.Add(func);
	}
	
	//--------------------------------access to tiles-------------------------
	
	public Tile GetTile(int i, int j){
		return _tiles[i,j];
	}
	public Tile GetTile(int[] coords){
		if(coords == null) return null;
		return _tiles[coords[X], coords[Y]];
	}
	public Tile[] GetNeighbours(Tile tile){
		return tile.neighbours;
	}
	public Tile[] GetNeighbours(PathNode pn){
		Tile tile = (Tile)pn;
		return tile.neighbours;
	}
	public Tile[] GetAccessableNeighbours(PathNode pn){
		if(_state != CellManagerState.Ready)return null;
		//not done generating
		Tile tile = (Tile)pn;	//origin tile
		Tile[] neighbours = tile.neighbours;
		Tile[] accessable = new Tile[9];
		for(int i = 0 ; i < 9 ; i++){
			Tile dest = neighbours[i]; //destination tile
			
			if(dest == null) continue;
			if(dest.traversable == false) continue;
			
			int dX = i % 3 - 1;
			int dY = (i / 3) % 3 - 1;
			
			if(dX == -1 && (dest.wallTile.east || tile.wallTile.west || !tile.westNeighbour.traversable))continue;
			if(dX == 1 && (dest.wallTile.west || tile.wallTile.east || !tile.eastNeighbour.traversable))continue;
			if(dY == -1 && (dest.wallTile.south || tile.wallTile.north || !tile.northNeighbour.traversable))continue;
			if(dY == 1 && (dest.wallTile.north || tile.wallTile.south || !tile.southNeighbour.traversable))continue;
			
			accessable[i] = dest;
		}
		
		//tl accessable
		
		return accessable;
	}	
	
	//-------------------------- general access --------------------------------
	public CellManagerState state{
		get{return _state;}
	}
	public int width{
		get{ return _width;}
	}
	public int height{
		get{ return _height;}
	}
	public Tile[,] tiles{
		get{return _tiles;}
	}
	public void VisitTiles(ref IVisitor visitor){
		foreach(Tile tile in tiles)tile.PassVisitor(visitor);
	}
	public List<Buildable> GetAllBuildables(){
		List<Buildable> all = new List<Buildable>();
		
		if(_state == CellManagerState.Ready){// if the manager is not ready there are no tiles to be found.
			foreach(Tile tile in tiles)all.AddRange(tile.GetAllBuildables());
		}
		return all;
	}
}
