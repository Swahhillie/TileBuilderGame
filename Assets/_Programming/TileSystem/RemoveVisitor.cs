using UnityEngine;
using System.Collections;

public class RemoveVisitor : ConstructionVisitor {
	
	public RemoveVisitor(Selections selections, TileEditor editor, CellManager manager, Menu menu) : 
		base(selections, editor, manager, menu){
		
		//override the selection for deleting walls. by default it will use a wall selection. but for removing we use a grid selection
		int[][] startEnd = selections.ClickAndDragSelection;
		_start = startEnd[0];
		_end = startEnd[1];
		
		
		if(IsViable){
			StartVisiting();
		}
		
	}
	
	override public void Visit(BlockTile blockTile){
		blockTile.RemoveBuildables();
	}
	override public void Visit(WallTile wallTile){
		wallTile.RemoveBuildables();
	}
	override public void Visit(FloorTile floorTile){
		floorTile.RemoveBuildables();
	}
}
