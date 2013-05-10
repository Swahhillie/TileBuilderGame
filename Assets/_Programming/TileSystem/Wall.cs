using UnityEngine;
using System.Collections;

public class Wall {
	
	public WallTile parent = null;
	public Buildable model = null;
	public bool wallSet = false;
	
	public enum Side{
		North,
		East,
		South,
		West
	};
	
	private Side side;
	
	public Wall(WallTile parent, Side side){
		this.parent = parent;
		this.side = side;
		
	}
	

	
	public void SetGo(Buildable newModel){
		//new model must be instantiated already
		
		if(wallSet == true){
			Debug.LogError("Wall is already set, should not build");
			return;
		}
		if(newModel == null)Debug.LogError("NEWMODEL CAN NOT BE NULL");
		
		model = newModel;
		model.name = parent.ToString() +", "+ side.ToString();
		model.transform.parent = parent.model.transform;
		model.transform.localPosition = Vector3.zero;
		
		switch(side){
			case Side.North:				
				model.transform.eulerAngles = new Vector3(0,90,0); //rotate 90
				break;
			case Side.West:
				
				//model.transform.localPosition = new Vector3(0,0,Tile.DIMENSION.z); // place at the other corner of the tile
				break;
		}
		wallSet = true;
		
	}
	public void Unset(){
		if(model == null) Debug.LogError("THERE IS NO MODEL TO DESTROY");
		GameObject.Destroy(model.gameObject);
		wallSet = false;
	}
	
}
