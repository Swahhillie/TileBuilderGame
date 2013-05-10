using UnityEngine;
using System.Collections;

public class WallTile : TileComponent{
	
	private Wall northWall;
	private Wall eastWall;
	private Wall southWall;
	private Wall westWall;
	
	static bool test = false;
	
	
	
	public WallTile (Tile tile) : base(tile){
		
		//SetWallGo(WallState.North | WallState.East, PrefabBank.GetGO("basicWallPrefab",CellLayer.Wall));
		_model = new GameObject(ToString());
		_model.transform.parent = tile.model.transform;
		_model.transform.localPosition = Vector3.zero;
	}
	public void InitializeWalls(){
		northWall = new Wall(this, Wall.Side.North);
		westWall = new Wall(this, Wall.Side.West);
		if(tile.eastNeighbour != null)
			eastWall = tile.eastNeighbour.wallTile.westWall;
		if(tile.southNeighbour != null)
			southWall = tile.southNeighbour.wallTile.northWall;
	}
	
	public bool DestroyWall(Wall.Side side){
		Wall target = GetSideTarget(side);		
		if(target.wallSet == true){
			_buildables.Remove(target.model);
			target.Unset();
			//return money
			return true;
		}
		else return false;
		
	
	}
	private Wall GetSideTarget(Wall.Side side){
		Wall target = null;
		switch(side){
			case Wall.Side.North: target = northWall; break;
			case Wall.Side.West: target = westWall; break;
			case Wall.Side.East: target = eastWall; break;			
			case Wall.Side.South: target = southWall; break;
		}
		return target;
	}
	public bool BuildWall(Wall.Side side, Buildable toBuild, bool destroyExsiting = false){
		
		Wall target = GetSideTarget(side);
		if(target.wallSet == true && destroyExsiting == false) return false;
		if(target.wallSet == true && destroyExsiting == true){
			//destroy the exsiting wall, return money
			target.Unset();
			
		}
		Buildable newModel = ((GameObject)GameObject.Instantiate(toBuild.gameObject)).GetComponent<Buildable>();
		target.SetGo(newModel);
		_buildables.Add(newModel);
		
		return true; //build succesfull	
	}
	public override void RemoveBuildables ()
	{
		base.RemoveBuildables ();
		northWall.wallSet = false;
		westWall.wallSet = false;		
	}
//	private void TestBuildWall(WallState toBuild){
//		Debug.Log("to build = " + toBuild);
//		Debug.Log("current = " + wallState);
//		Debug.Log("OR operator = " + ((wallState | toBuild) == wallState)); //responds true if wall state contains the thing to build
//		Debug.Log("AND operator = " + ((wallState & toBuild) == wallState)); //responds if they are identical
//		
//		wallState = toBuild | wallState; //add the state to build
//		
//		Debug.Log("after = " + wallState);
//		
//	}
	override public void Accept(IVisitor visitor){
		visitor.Visit(this);
	}
	override public CellLayer GetLayer(){
		return CellLayer.Wall;
	}
	public WallState wallState{
		get{
			WallState state = WallState.none;
			if(north) state = (state | WallState.North);
			if(west) state = (state | WallState.West);
			if(east) state = (state | WallState.East);
			if(south) state = (state | WallState.South);
			return state;
		}
	} 
	public bool north{
		get{
			return (northWall != null) && northWall.wallSet;
		}	
	}
	public bool west{
		get{
			return (westWall != null) && westWall.wallSet;
		}	
	}
	public bool east{
		get{
			return (eastWall != null) && (eastWall.wallSet);
		}
	}
	public bool south{
		get{
			return (southWall != null) && southWall.wallSet;
		}	
	}
	
}
