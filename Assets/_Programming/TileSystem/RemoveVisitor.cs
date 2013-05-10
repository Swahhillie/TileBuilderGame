using UnityEngine;
using System.Collections;

public class RemoveVisitor : ConstructionVisitor {
	
	public RemoveVisitor(Selections selections, TileEditor editor, CellManager manager, Menu menu) : 
		base(selections, editor, manager, menu){
			
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
