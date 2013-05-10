using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[System.Serializable]
public class Tile : PathNode{
	
	private static GameObject holder;
	private GameObject _model;
	
	public static Vector3 DIMENSION = new Vector3(1,0,1);
	
	private Tile[] _neighbours;
	
	public int c;
	public int r; //for debug
	
	private WallTile _wallTile;
	private BlockTile _blockTile;
	private FloorTile _floorTile;
	
	public Tile(int column, int row){
		neighbours = new Tile[9];
		this.column = column;
		this.row = row;
		c = column;
		r = row;
	
		if(holder == null)holder = new GameObject("tileHolder");
		_model = new GameObject(ToString());
		_model.transform.parent = holder.transform;
		_model.transform.position = corner;
		
		_wallTile  = new WallTile(this);
		_blockTile = new BlockTile(this);
		_floorTile = new FloorTile(this);
	}
	
	
	public void Cleanup(){
		//do stuff to destroy the tile here
		GameObject.Destroy(model);
	}
	public Vector3 corner{
		get{return new Vector3(column * DIMENSION.x, 0, row * DIMENSION.z);}
	}
	public Vector3 center{
		get{return new Vector3(column * DIMENSION.x + DIMENSION.x / 2, 0 , row * DIMENSION.z + DIMENSION.z /2);}
	}
	public WallTile wallTile{
		get{return _wallTile;}
	}
	public BlockTile blockTile{
		get{return _blockTile;}
	}
	public FloorTile floorTile{
		get{return _floorTile;}
	}
	public GameObject model{
		get{return _model;}	
	}
	public void DebugColor(Color c, bool colorNeighbours = false){
		
//		Renderer r = floorTile.model.GetComponentInChildren<Renderer>();
//		if(wallTile.wallState != WallState.none)	
//			
//		
//		r.material.color = c;
		//floorTile.model.GetComponentInChildren<TextMesh>().text =  "g " + g + "\nh " + h + "\nf " + f;
		if(colorNeighbours){
			for(int i = 0; i < neighbours.Length; i++){
				Tile tile  = neighbours[i];
				
				if(tile == null)continue;
				
				tile.DebugColor(c);
				
			}
		}
		
	}
	
	override public bool traversable{
		get{ 
			return (blockTile.traversable && wallTile.traversable && floorTile.traversable);// if all are true, return true. else return false
		}
	}
	public bool Build(CellLayer targetLayer){
		bool result = false;
		switch(targetLayer){
			case CellLayer.Block:
				result = blockTile.Build();
				break;
			case CellLayer.Wall:
				result = wallTile.Build();
				break;
			case CellLayer.Floor:
				result = floorTile.Build();
				break;
		}
		
		return result;
	}
	public void PassVisitor(IVisitor visitor, CellLayer targetLayer){
		switch(targetLayer){
			case CellLayer.Block:
				blockTile.Accept(visitor);
				break;
			case CellLayer.Wall:
				wallTile.Accept(visitor);
				break;
			case CellLayer.Floor:
				floorTile.Accept(visitor);
				break;
		}
	}
	public void PassVisitor(IVisitor visitor){
		blockTile.Accept(visitor);
		wallTile.Accept(visitor);
		floorTile.Accept(visitor);
	}
	public Tile[] neighbours{
		get{return _neighbours;}
		set{_neighbours = value;}
	}
	public Tile westNeighbour{
		get{return _neighbours[3];}
	}
	public Tile northNeighbour{
		get{return _neighbours[1];}
	}
	public Tile eastNeighbour{
		get{return _neighbours[5];}
	}
	public Tile southNeighbour{
		get{return _neighbours[7];}
	}
	override public string ToString(){
		return "(c" + column.ToString("00") + ", r" + row.ToString("00") +")";
	}
	public List<Buildable> GetAllBuildables(){
		List<Buildable> buildablesOnTile = new List<Buildable>();
		buildablesOnTile.AddRange(wallTile.buildables);
		buildablesOnTile.AddRange(blockTile.buildables);
		buildablesOnTile.AddRange(floorTile.buildables);
		return buildablesOnTile;
	}
}