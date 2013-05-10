using UnityEngine;
using System.Collections;

public class ConstructionVisitor : IVisitor {
	
	//Data used for building
	protected Selections _selections;
	protected TileEditor _tileEditor;
	protected CellManager _manager;
	protected Menu _menu;
	//where to build
	protected int [] _start = null;
	protected int [] _end = null;
	
	protected Buildable toBuild; //cache toBuild
	
	
	public ConstructionVisitor (Selections selections, TileEditor editor, CellManager manager, Menu menu) {
	//the build visitor reads from the different states to determine what it should do.
		
		this._selections = selections;
		this._tileEditor = editor;
		this._manager = manager;
		this._menu = menu;
		
		
		//determine the start and end of the stuff to build

		int[][] startEnd = null;
		switch(editor.toolTarget){
			case CellLayer.Block: 
				startEnd = selections.clickAndDragSelection;
				if(startEnd != null){
					_start = startEnd[0];
					_end = startEnd[1];
				}
				break;
			case CellLayer.Wall:
				startEnd = selections.lineSelection;
				if(startEnd != null){
					_start = startEnd[0];
					_end = startEnd[1];
				}
				
				break;
			case CellLayer.Floor: 
				startEnd = selections.clickAndDragSelection;
				if(startEnd != null){
					_start = startEnd[0];
					_end = startEnd[1];
				}
				break;
		}
		
		if(_start == null || _end == null){
			Debug.Log("start or end is null, cannot pass visitor to blocks");
			return;
		}
		StartVisiting();
		//pass this object along to the cells in question.
		
	}
	protected virtual void StartVisiting(){
		for(int i = _start[0]; i <= _end[0] ; i++){
			for(int j = _start[1]; j <= _end[1]; j++){
				Tile tile = _manager.GetTile(i, j);
				tile.PassVisitor(this, _tileEditor.toolTarget);
			}
		}
	}
	
	public virtual void Visit(BlockTile blockTile){
		
	}
	public virtual void Visit(WallTile wallTile){
		
	}
	public virtual void Visit(FloorTile floorTile){
	
	}
}
