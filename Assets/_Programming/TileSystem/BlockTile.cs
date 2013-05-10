using UnityEngine;
using System.Collections;

public class BlockTile : TileComponent {
	
	public BlockTile(Tile tile) : base(tile){
		
	}
	/*
	public bool BuildBlock(string blockName, bool overTop = false){ // maybe usefull function later
		return BuildBlock(PrefabBank.GetGO(blockName, CellLayer.Block), overTop);
	}
	*/
	override public bool Build(){
		//get the info from the rest of the game state to find what to build
		
		return false;
	}
	public bool BuildBlock(Buildable block, bool overTop = false){
		if(_model != null){
			if(!overTop)return false; //cannot build overtop
			_buildables.Remove(_model.GetComponent<Buildable>());
			GameObject.Destroy(_model);
		}
		_model = GameObject.Instantiate(block.gameObject, tile.corner, Quaternion.identity ) as GameObject;
		_model.transform.parent = tile.model.transform;
		_buildables.Add(_model.GetComponent<Buildable>());
		return true;
	}
	override public bool traversable{
		get{return _model == null;}
	}
	override public void Accept(IVisitor visitor){
		visitor.Visit(this);
	}
	override public CellLayer GetLayer(){
		return CellLayer.Block;
	}
}
