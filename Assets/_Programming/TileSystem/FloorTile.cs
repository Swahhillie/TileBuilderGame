using UnityEngine;
using System.Collections;

public class FloorTile : TileComponent {

	public FloorTile(Tile tile) : base (tile){
		
	}
	override public void Accept(IVisitor visitor){
		visitor.Visit(this);
	}
	override public CellLayer GetLayer(){
		return CellLayer.Floor;
	}
	public bool BuildFloor(Buildable toBuild, bool overtop = false){
		if(_model != null){
			if(!overtop)return false; //cannot build overtop
			_buildables.Remove(_model.GetComponent<Buildable>());
			GameObject.Destroy(_model);
		}
		_model = (GameObject)GameObject.Instantiate(toBuild.gameObject, tile.corner, Quaternion.identity);
		_model.transform.parent = tile.model.transform;
		_buildables.Add(_model.GetComponent<Buildable>());
		return true;
		
	}
	override public bool traversable{
		get{return _model != null;}
	}
}
