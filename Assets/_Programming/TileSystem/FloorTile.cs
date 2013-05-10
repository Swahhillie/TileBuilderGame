using UnityEngine;
using System.Collections;

public class FloorTile : TileComponent {

	public FloorTile(Tile tile) : base (tile){
		_model = (GameObject)GameObject.Instantiate(PrefabBank.GetBuildable("basicFloorPrefab", CellLayer.Floor).gameObject, tile.corner, Quaternion.identity);
		_model.transform.parent = tile.model.transform;
		_buildables.Add(_model.GetComponent<Buildable>());
	}
	override public void Accept(IVisitor visitor){
		visitor.Visit(this);
	}
	override public CellLayer GetLayer(){
		return CellLayer.Floor;
	}
	public bool BuildFloor(Buildable toBuild, bool overtop = false){
		if(_model != null && overtop == false)return false;
		else if(_model != null && overtop == true){
			//destroy model, possible refund?
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
