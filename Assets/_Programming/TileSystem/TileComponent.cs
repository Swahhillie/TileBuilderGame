using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TileComponent : Element {
	
	protected Tile _tile;
	protected GameObject _model;
	
	protected List<Buildable> _buildables = new List<Buildable>();
	
	public TileComponent(Tile tile){
		_tile = tile;
	}
	
	public Tile tile{
		get{return _tile;}
	}
	virtual public bool Build(){
		return false;//must be overwritten
	}
	virtual public bool traversable{
		get{return true;}
	}
	virtual public GameObject model{
		get{return _model;}
	}
	public List<Buildable> buildables{
		get{return _buildables;}
	}
	override public string ToString(){
		return tile.ToString() + ", " + GetLayer().ToString();
	}
	virtual public void RemoveBuildables(){
		for(int i = _buildables.Count -1; i >= 0; i--){
			Buildable b = _buildables[i];
			GameObject.Destroy(b.gameObject);
			_buildables.RemoveAt(i);
		}
	}
	abstract public CellLayer GetLayer();
}