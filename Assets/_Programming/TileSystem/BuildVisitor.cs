using UnityEngine;
using System.Collections;

public class BuildVisitor : ConstructionVisitor {
	
	
	public BuildVisitor(Selections selections, TileEditor editor, CellManager manager, Menu menu) : 
		base(selections, editor, manager, menu){
		
		//the build visitor reads from the different states to determine what it should do.
		
	}
	
	override protected void StartVisiting(){
	
	
	
		for(int i = _start[0]; i <= _end[0] ; i++){
			for(int j = _start[1]; j <= _end[1]; j++){
				Tile tile = _manager.GetTile(i, j);
				tile.PassVisitor(this, _tileEditor.toolTarget);
			}
		}
	
	}
	
	override public void Visit(BlockTile blockTile){
		if(toBuild == null) toBuild = PrefabBank.GetBuildable(_menu.GetSelected(CellLayer.Block), CellLayer.Block);
		if(blockTile.BuildBlock(toBuild , false)){
			//Debug.Log("Build succesfull on "  + blockTile);		
		}
		//Debug.Log("Build visitor on " + blockTile.tile + ", " + CellLayer.Block);
	}
	override public void Visit(WallTile wallTile){
		if(toBuild == null) toBuild = PrefabBank.GetBuildable(_menu.GetSelected(CellLayer.Wall), CellLayer.Wall);
		bool horizontal = _start[0] == _end[0];
		
		if(wallTile.BuildWall(horizontal? Wall.Side.West : Wall.Side.North, toBuild)){
			
		}

	}
	override public void Visit(FloorTile floorTile){
		if(toBuild == null) toBuild = PrefabBank.GetBuildable(_menu.GetSelected(CellLayer.Floor), CellLayer.Floor);
		if(floorTile.BuildFloor(toBuild, true)){
			
		}
	}
//	public void Visit(Element element){
//		Debug.Log(element + " has no build visitor implementation");
//	}
}
